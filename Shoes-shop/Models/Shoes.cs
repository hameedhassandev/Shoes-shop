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

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;
        public double Price { get; set; }
        public string ImageURL { get; set; } = string.Empty;

        [Display(Name = "Shoes Of The Week")]
        public bool IsShoesOfTheWeek { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Number In Stock")]
        public int NumberInStock { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Cart>? carts { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
