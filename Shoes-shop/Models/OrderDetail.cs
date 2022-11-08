using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int ShoesId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public Shoes Shoes { get; set; }
        public Order? Order { get; set; }

    }
}
