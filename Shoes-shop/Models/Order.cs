using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.Models
{
    public class Order
    {
        
        public int Id { get; set; }
        public DateTime dateTime { get; set; }      
        public double TotalPrice { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}