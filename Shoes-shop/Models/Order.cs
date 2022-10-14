using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.Models
{
    public class Order
    {
        
        public int Id { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}