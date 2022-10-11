using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext Context;
        private readonly IShoesService shoesService;
        public OrderService(ApplicationDbContext context, IShoesService _shoesService)
        {
            Context = context;
            shoesService = _shoesService;
        }

        public Order Add(Order entity,int shoesId,int qty) 
        { 
            Context.orders.Add(entity);
            var shoes = shoesService.Get(shoesId);

            shoes.NumberInStock -= qty;
            Context.Shoes.Update(shoes);

            Context.SaveChanges();
            return Context.orders.FirstOrDefault(o => o.UserId == entity.UserId && o.dateTime == entity.dateTime);
        }

        public IEnumerable<Order> All()
        {
            return Context.orders.ToList();
        }

        public Order Delete(Order entity)
        {
            Context.orders.Remove(entity);
            Context.SaveChanges();
            return entity;
        }

        //order by desc using datetime  
        public IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
           return Context.orders.Include(o => o.OrderDetails).Where(predicate).OrderByDescending(o => o.dateTime).ToList();
        }


        public Order GetOrder(int id)
        {
           return Context.orders.Include(o=>o.User).FirstOrDefault(o => o.Id == id); 
        }

        public Order Update(Order entity)
        {
            var order = Context.orders.Attach(entity);
            order.State = EntityState.Modified;
            Context.SaveChanges();
            return entity;

        }


    }
}
