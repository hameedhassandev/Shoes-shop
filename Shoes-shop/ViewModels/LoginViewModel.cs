using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RemomberMe { get; set; }
    }
}
