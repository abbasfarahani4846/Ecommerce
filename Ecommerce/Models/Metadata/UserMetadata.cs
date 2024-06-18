using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Metadata
{
    public class UserMetadata
    {
        [Display(Name = "asdasdasdasdasdasd")]
        [StringLength(100)]
        public string? Username { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        // The class body can be empty or contain additional properties or methods.
    }
}
