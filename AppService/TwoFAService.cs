using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArivalBank2FATask.AppService
{
    public class TwoFAService : ITwoFAService
    {
        private readonly ISmsSender _smsSender;
        private readonly TwoFaConfig _config;
        private readonly Dictionary<string, List<string>> _activeCodes;
        private readonly ILogger<TwoFAService> _logger;

        public TwoFAService(IOptions<TwoFaConfig> config, ISmsSender smsSender, ILogger<TwoFAService> logger)
        {
            _config = config.Value;
            _smsSender = smsSender;
            _logger = logger;
            _activeCodes = new Dictionary<string, List<string>>();
        }

        public bool SendConfirmationCode(string phoneNumber)
        {

            // Check if the user has too many active codes
            if (_activeCodes.ContainsKey(phoneNumber) && _activeCodes[phoneNumber].Count >= _config.MaxConcurrentCodesPerPhone)
            {
                _logger.LogWarning("User has too many active codes: {phoneNumber}", phoneNumber);
                return false;
            }

            // Generate a random code
            string code = GenerateRandomCode();

            // Store the code along with the timestamp
            if (!_activeCodes.ContainsKey(phoneNumber))
            {
                _activeCodes[phoneNumber] = new List<string>();
            }
            _activeCodes[phoneNumber].Add($"{code}|{DateTime.Now}");

            // Log the code
            _logger.LogInformation("Code: {code} sent to phone number: {phoneNumber}",code, phoneNumber);

            // Send the code to the user via SMS
            _smsSender.SendSms(phoneNumber, $"Your confirmation code is: {code}");

            return true;
        }

        public bool VerifyConfirmationCode(string phoneNumber, string code)
        {

            // Check if the user has any active codes
            if (!_activeCodes.ContainsKey(phoneNumber) || _activeCodes[phoneNumber].Count == 0)
            {
                _logger.LogWarning("User has no active codes: {phoneNumber}", phoneNumber);
                return false;
            }

            // Get the first code and timestamp
            string[] codeWithTimestamp = _activeCodes[phoneNumber][0].Split('|');
            string storedCode = codeWithTimestamp[0];
            DateTime timestamp = DateTime.Parse(codeWithTimestamp[1]);

            // Check if the code is valid
            if (storedCode == code && timestamp.Subtract(DateTime.Now).TotalMinutes <= _config.CodeLifetimeMinutes)
            {
                // Remove the code from the active list
                _activeCodes[phoneNumber].RemoveAt(0);

                return true;
            }
            else
            {
                _logger.LogWarning("Invalid code or code expired: {phoneNumber} {code}", phoneNumber, code);
                return false;
            }
        }

        private string GenerateRandomCode()
        {
            // Generate a random 6-digit code
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

    }
}
