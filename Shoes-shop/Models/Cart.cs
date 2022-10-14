using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public int ShoesId { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public int Quntity { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        public virtual Shoes? Shoes { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
    }
}
