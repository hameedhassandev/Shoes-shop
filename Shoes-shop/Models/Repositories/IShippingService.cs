namespace Shoes_shop.Models.Repositories
{
    public interface IShippingService
    {
        public Shipping AddShipping(string userId, Shipping shipping);

        public Shipping GetShipping(string userId);

        public Shipping UpdateShipping(Shipping shipping);

        public Shipping DeleteShipping(Shipping shipping);
    }
}
