using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoes_shop.Models
{
    public class Shoes
    {
  
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        [Required]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageURL { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Shoes Of The Week")]
        public bool IsShoesOfTheWeek { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Number In Stock")]
        [Required]
        public int NumberInStock { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Cart>? carts { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
