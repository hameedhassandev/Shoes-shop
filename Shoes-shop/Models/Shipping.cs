using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.Models
{
    public class Shipping
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        [MaxLength(120)]
        [Required]
        public string Address { get; set; } = string.Empty;

        [MaxLength(12)]
        [Required]
        public string Contact { get; set; } = string.Empty;

        [MaxLength(200)]
        [Required]
        public string ExtraDetails { get; set; } = string.Empty;
    }
}
