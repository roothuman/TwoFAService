using ArivalBank2FATask.AppService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArivalBank2FATask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorAuthController : ControllerBase
    {
        private readonly ITwoFAService _twoFAService;

        public TwoFactorAuthController(ITwoFAService twoFAService)
        {
            _twoFAService = twoFAService;
        }

        [HttpPost("send-code")]
        public  IActionResult SendConfirmationCode([FromBody] SendCodeRequest request)
        {
            try
            {
                bool success =  _twoFAService.SendConfirmationCode(request.PhoneNumber);
                if (success)
                {
                    return Ok(new { Message = "Code sent successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Too many active codes for this phone." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpPost("check-code")]
        public  IActionResult CheckConfirmationCode([FromBody] CheckCodeRequest request)
        {
            try
            {
                bool success =  _twoFAService.VerifyConfirmationCode(request.PhoneNumber, request.Code);
                if (success)
                {
                    return Ok(new { Message = "Code is valid." });
                }
                else
                {
                    return BadRequest(new { Message = "Invalid code." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }
    }
}
