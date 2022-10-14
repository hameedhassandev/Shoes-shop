using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.Models
{
    public class Category
    {
        [BindNever]
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        [Remote(controller: "Categories", action: "Exsist", ErrorMessage = "This category already exsist", AdditionalFields = "Id")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        [Required]
        
        public string Description { get; set; } = string.Empty;

        public ICollection<Shoes>? Shoes { get; set; }
    }
}
