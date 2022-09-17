using Microsoft.AspNetCore.Identity;

namespace Shoes_shop.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int ShoesId { get; set; }
        public DateTime dateTime { get; set; }
        public int Quntity { get; set; }
        public double TotalPrice { get; set; }
        public virtual Shoes? Shoes { get; set; }

        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
    }
}
