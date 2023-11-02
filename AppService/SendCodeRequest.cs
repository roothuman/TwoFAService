using System.ComponentModel.DataAnnotations;

namespace ArivalBank2FATask.Controllers
{
    public class SendCodeRequest
    {
        /// <summary>
        /// The phone number to which the 2FA code should be sent.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?\d{1,4}[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$",
            ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
    }

    public class CheckCodeRequest
    {
        /// <summary>
        /// The phone number to which the 2FA code should be sent.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?\d{1,4}[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$",
            ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
