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
        public ApplicationUser? User { get; set; }

        [Required]
        public string ShippingAddress { get; set; } = string.Empty;


        [Required]
        public string Contact { get; set; } = string.Empty;

        public bool IsConfirmed { get; set; }
        public bool IsShippedAndPay { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}