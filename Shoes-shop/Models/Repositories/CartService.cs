using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;

namespace Shoes_shop.Models.Repositories
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext context;
        private readonly IShoesService shoesService;
        private readonly BaseRepository<Order> orderRepo;
        private readonly BaseRepository<OrderDetail> orderShoesRepo;
        public CartService(ApplicationDbContext context, IShoesService _shoesService, BaseRepository<Order> _orderRepo, BaseRepository<OrderDetail> _orderShoesRepo)
        {
            this.context = context;
            this.shoesService = _shoesService;
            this.orderRepo = _orderRepo;
            this.orderShoesRepo = _orderShoesRepo;
        }
        private bool IsAvailableShoes(int shoesId , int qty)
        {
            //check quantity of shoes is available or not
            return shoesService.Get(shoesId).NumberInStock >= qty;
        }

        public void AddItem(string userId, int shoesID, int qty)
        {
            Shoes shoes = context.Shoes.Find(shoesID);

            if (shoes == null || !IsAvailableShoes(shoesID, qty))
                return;

            var existInCart = context.Carts.FirstOrDefault(c => c.ShoesId == shoesID && c.UserId == userId) != null;
            if (existInCart)
                return;

            Cart cart = new Cart();
            cart.ShoesId = shoesID;
            cart.UserId = userId;
            cart.dateTime = DateTime.Now;
            cart.Quntity = qty;
            cart.TotalPrice = shoes.Price * qty;

            context.Carts.Add(cart);
            shoes.NumberInStock -= qty;
            context.Shoes.Update(shoes);

            context.SaveChanges(true);

        }

        public void ClearCart(string clientId)
        {
            var allItemsInCart = context.Carts.Where(c => c.UserId == clientId).ToList();

            // return all items to stock 
            foreach (var item in allItemsInCart)
                RemoveItem(clientId, item.ShoesId);

            context.SaveChanges(true);
        }

        public void DecreaseItemByOne(string userId, int shoesID)
        {
            Shoes shoes = context.Shoes.Find(shoesID);

            if (shoes == null)
                return;
            Cart cart = context.Carts.FirstOrDefault(c => c.UserId == userId && c.ShoesId == shoesID);
            cart.Quntity --;
            cart.TotalPrice -= shoes.Price;

            if (cart.Quntity == 0)
                context.Carts.Remove(cart);

            shoes.NumberInStock++;
            shoesService.Update(shoes);

            context.SaveChanges(true);  

        }

        public List<Cart> GetAllItems(string userId)
        {
            return context.Carts.Where(c => c.UserId == userId).Include(c => c.Shoes).ToList();
        }

        public void IncreaseItemByOne(string userId, int shoesID)
        {
            Shoes shoes = context.Shoes.Find(shoesID);

            if (shoes == null)
                return;
            Cart cart = context.Carts.FirstOrDefault(c => c.UserId == userId && c.ShoesId == shoesID);

            if (!IsAvailableShoes(shoesID, cart.Quntity +1))
                return;

            cart.Quntity++;
            cart.TotalPrice += shoes.Price;
         

            shoes.NumberInStock--;
            shoesService.Update(shoes);

            context.SaveChanges(true);
        }

        public void RemoveItem(string userId, int shoesID)
        {
            Cart cart = context.Carts.FirstOrDefault(c => c.UserId == userId && c.ShoesId == shoesID);
            Shoes shoes = context.Shoes.Find(shoesID);

            if (shoes == null && cart == null)
                return;
            // remove the shoes from the cart and increase NumberInStock by the removed quantity
            context.Carts.Remove(cart);
            shoes.NumberInStock += cart.Quntity;
            shoesService.Update(shoes);

            context.SaveChanges(true);
        }

        public void ToOrder(string userId)
        {
            var items = context.Carts.Where(c => c.UserId == userId).ToList();

            // calculate order total price
            double orderTotal = items.Sum(item => item.TotalPrice);

            // create new order
            Order order = new Order() { UserId = userId, dateTime = DateTime.Now, TotalPrice = orderTotal };
            order = orderRepo.Add(order);

            // add all shoes to order
            foreach (var item in items)
            {
                OrderDetail orderDetails = new OrderDetail() { OrderId = order.Id, ShoesId = item.ShoesId, Quantity = item.Quntity };
                orderShoesRepo.Add(orderDetails);
            }

            // remove all items from stock
            context.RemoveRange(items);

        
            context.SaveChanges(true);
        }
     
    }
}
