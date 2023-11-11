using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ArivalBank2FATask.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArivalBank2FATask.AppService
{
    public class TwoFAService : ITwoFAService
    {
        private readonly ISmsSender _smsSender;
        private readonly TwoFaConfig _config;
        private readonly ActiveCodeDbContext _dbContext;
        private readonly ILogger<TwoFAService> _logger;

        public TwoFAService(IOptions<TwoFaConfig> config, ISmsSender smsSender, ActiveCodeDbContext dbContext, ILogger<TwoFAService> logger)
        {
            _config = config.Value;
            _smsSender = smsSender;
            _dbContext = dbContext;
            _logger = logger;
        }

        public bool SendConfirmationCode(string phoneNumber)
        {
            // Check if the user has too many active codes
            int activeCodeCount = _dbContext.ActiveCodes.Count(ac => ac.PhoneNumber == phoneNumber);
            if (activeCodeCount >= _config.MaxConcurrentCodesPerPhone)
            {
                _logger.LogWarning("User has too many active codes: {phoneNumber}", phoneNumber);
                return false;
            }

            // Generate a random code
            string code = GenerateRandomCode();

            // Store the code in the database with a UTC timestamp
            _dbContext.ActiveCodes.Add(new ActiveCode { PhoneNumber = phoneNumber, Code = code, Timestamp = DateTime.UtcNow });
            _dbContext.SaveChanges();

            // Log the code
            _logger.LogInformation("Code: {code} sent to phone number: {phoneNumber}", code, phoneNumber);

            // Send the code to the user via SMS
            _smsSender.SendSms(phoneNumber, $"Your confirmation code is: {code}");

            return true;
        }

        //public bool VerifyConfirmationCode(string phoneNumber, string code)
        //{
        //    // Check if the user has any active codes
        //    var activeCode = _dbContext.ActiveCodes
        //        .Where(ac => ac.PhoneNumber == phoneNumber)
        //        .OrderBy(ac => ac.Timestamp)
        //        .FirstOrDefault();

        //    if (activeCode == null || activeCode.Code != code || activeCode.Timestamp.AddMinutes(_config.CodeLifetimeMinutes) < DateTime.Now)
        //    {
        //        _logger.LogWarning("Invalid code or code expired: {phoneNumber} {code}", phoneNumber, code);
        //        return false;
        //    }

        //    // Remove the code from the database
        //    _dbContext.ActiveCodes.Remove(activeCode);
        //    _dbContext.SaveChanges();

        //    return true;
       // }


        public bool VerifyConfirmationCode(string phoneNumber, string code)
        {
            // Check if the user has any active codes
            var activeCode = _dbContext.ActiveCodes
                .Where(ac => ac.PhoneNumber == phoneNumber)
                .OrderBy(ac => ac.Timestamp)
                .FirstOrDefault();

            if (activeCode == null || activeCode.Code != code)
            {
                _logger.LogWarning("Invalid code or code not found: {phoneNumber} {code}", phoneNumber, code);
                return false;
            }

            // Ensure that the timestamp is in UTC
            DateTime utcNow = DateTime.UtcNow;
            DateTime codeExpirationTimeUtc = activeCode.Timestamp.AddMinutes(_config.CodeLifetimeMinutes);

            if (codeExpirationTimeUtc < utcNow)
            {
                _logger.LogWarning("Code expired: {phoneNumber} {code}", phoneNumber, code);

                // Remove the code from the database
                _dbContext.ActiveCodes.Remove(activeCode);
                _dbContext.SaveChanges();
                return false;
            }

            //Remove the code from the database
            _dbContext.ActiveCodes.Remove(activeCode);
            _dbContext.SaveChanges();

            return true;
        }



        private string GenerateRandomCode()
        {
            // Generate a random 6-digit code
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }

}
