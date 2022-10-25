namespace Shoes_shop.Models.Repositories
{
    public interface ICartService
    {
        public List<Cart> GetAllItems(string userId);
        public void AddItem(string userId, int shoesID, int qty);
        public void RemoveItem(string userId, int shoesID);
        public void IncreaseItemByOne(string userId, int shoesID);
        public void DecreaseItemByOne(string userId, int shoesID);
        public void ClearCart(string userId);
        public void ToOrder(string userId, string userAdress, string UserContact);
            
            }
}
