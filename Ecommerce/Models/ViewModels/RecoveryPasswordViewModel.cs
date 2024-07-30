using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels
{
    public class RecoveryPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
