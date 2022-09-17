using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoes_shop.Models
{
    public class Shoes
    {
  
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string ShortDescription { get; set; } = string.Empty;
        public double Price { get; set; }
        public string ImageURL { get; set; } = string.Empty;    
        public bool IsShoesOfTheWeek { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }
        public int NumberInStock { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Cart>? carts { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
