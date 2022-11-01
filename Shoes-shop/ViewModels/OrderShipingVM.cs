using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.ViewModels
{
    public class OrderShipingVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Shipping Address")]
        [MaxLength(220)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Mobile To Contact")]
        [MaxLength(15)]
        public string Contact { get; set; } = string.Empty;
    }
}
