using System.ComponentModel.DataAnnotations;

namespace KitchenStory.Models
{
    public class ResetPasswordModel
    {
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,16}$", ErrorMessage = "Invalid Password")]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ContactNo { get; set; }
    }
}