using Shoes_shop.Data;
using System.Linq;

namespace Shoes_shop.Models.Repositories
{
    public class ShippingService : IShippingService
    {
        private readonly ApplicationDbContext Context;

        public ShippingService(ApplicationDbContext _context)
        {
            Context = _context;
        }   

        public Shipping AddShipping(string userId, Shipping shipping)
        {
            Shipping sh = new Shipping();
            sh.UserId = userId;
            sh.Address = shipping.Address;
            sh.Contact = shipping.Contact;
            sh.ExtraDetails = shipping.ExtraDetails;

            Context.shipping.Add(sh);

            Context.SaveChanges();
            return sh;  
        }

        public Shipping GetShipping(string userId)
        {
            return Context.shipping.SingleOrDefault(s => s.UserId == userId);
        }


        public Shipping UpdateShipping(Shipping shipping)
        {

            Context.shipping.Update(shipping);
            Context.SaveChanges();

            return shipping;
        }

        public Shipping DeleteShipping(Shipping shipping)
        {

            Context.shipping.Remove(shipping);
            Context.SaveChanges();

            return shipping;
        }
    }
}
