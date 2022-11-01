using Shoes_shop.Models;

namespace Shoes_shop.ViewModels
{
    public class ShoesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;

        public double Price { get; set; }

        public int NumberInStock { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }
        public string ImageURL { get; set; } = string.Empty;

        public Category? category { get; set; }
    }
}
