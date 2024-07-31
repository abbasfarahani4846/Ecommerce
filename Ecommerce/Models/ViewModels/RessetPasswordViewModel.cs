using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels
{
    public class RessetPasswordViewModel
    {
        public string Email { get; set; }
        [Required]
        [Display(Name = "Recovery Code")]

        public int? RecoveryCode { get; set; }
        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }
}
