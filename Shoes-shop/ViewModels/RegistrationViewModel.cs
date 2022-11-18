using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not Matching")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [MaxLength(120)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(120)]
        public string Address { get; set; }

        [Required]
        [MaxLength(17)]
        [Display(Name = "Mobile Number")]
        public string Contact { get; set; }


        [Required]     
        public string Gender { get; set; }


    }
}
