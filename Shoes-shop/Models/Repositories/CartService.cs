using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;

namespace Shoes_shop.Models.Repositories
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext context;
        private readonly IShoesService shoesService;
        private readonly IOrderService orderService;
        private readonly IOrderDetailsService orderDetailsService;
        public CartService(ApplicationDbContext _context, IShoesService _shoesService, IOrderService _orderService, IOrderDetailsService _orderDetailsService)
        {
            context = _context;
            shoesService = _shoesService;
            orderService = _orderService;
            orderDetailsService = _orderDetailsService;
        }
        private bool IsAvailableShoes(int shoesId , int qty)
        {
            //check quantity of shoes is available or not
            return shoesService.Get(shoesId).NumberInStock >= qty;
        }


        public bool existIncart(int shoesID, string userId)
        {
            var existInCart = context.Carts.FirstOrDefault(c => c.ShoesId == shoesID && c.UserId == userId) != null;
            if (existInCart)
                return true;

            return false;
        }

        public void AddItem(string userId, int shoesID, int qty)
        { 
            var shoes = context.Shoes.FirstOrDefault(s=>s.Id == shoesID);

            if (shoes == null || !IsAvailableShoes(shoesID, qty))
                return;

            Cart cart = new Cart();
            cart.ShoesId = shoesID;
            cart.UserId = userId;
            cart.dateTime = DateTime.Now;
            cart.Quntity = qty;
            cart.TotalPrice = shoes.Price * qty;

            context.Carts.Add(cart);
            context.SaveChanges(true);

        }

        public void ClearCart(string userId)
        {
            var allItemsInCart = context.Carts.Where(c => c.UserId == userId).ToList();

            // return all items to stock 
            foreach (var item in allItemsInCart)
                RemoveItem(userId, item.ShoesId);

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
         

            context.SaveChanges(true);
        }

        public void RemoveItem(string userId, int shoesID)
        {
            Cart cart = context.Carts.FirstOrDefault(c => c.UserId == userId && c.ShoesId == shoesID);
            Shoes shoes = context.Shoes.Find(shoesID);

            if (shoes == null && cart == null)
                return;
            context.Carts.Remove(cart);

            context.SaveChanges(true);
        }

        public void ToOrder(string userId,string userAdress, string UserContact)
        {
            
            var items = context.Carts.Where(c => c.UserId == userId).ToList();

            // calculate order total price
            double orderTotal = items.Sum(item => item.TotalPrice);

            // create new order
            Order order = new Order()
            { UserId = userId, dateTime = DateTime.Now, TotalPrice = orderTotal,ShippingAddress = userAdress, Contact= UserContact };
            order = orderService.Add(order);

            // add all shoes to order
            foreach (var item in items)
            {
                OrderDetail orderDetails = new OrderDetail() 
                { OrderId = order.Id, ShoesId = item.ShoesId, Quantity = item.Quntity };
                orderDetailsService.Add(orderDetails);
            }

            // remove all items from stock
            context.RemoveRange(items);

            context.SaveChanges(true);
        }


        public int UserCartCount(string userId)
        {
            return context.Carts.Where(c=>c.UserId == userId).Count();
        }

        public object existIncart(int shoesId, Task<string> userid)
        {
            throw new NotImplementedException();
        }
    }
}
