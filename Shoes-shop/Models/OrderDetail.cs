namespace Shoes_shop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int ShoesId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public Shoes? Shoes { get; set; }
        public Order? Order { get; set; }

    }
}
