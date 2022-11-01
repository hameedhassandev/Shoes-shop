using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext Context;

        public OrderService(ApplicationDbContext context, IShoesService _shoesService)
        {
            Context = context;
        }

        public Order Add(Order entity) 
        { 
            Context.orders.Add(entity);
            Context.SaveChanges();
            return Context.orders.FirstOrDefault(o => o.UserId == entity.UserId && o.dateTime == entity.dateTime);
        }


        public IEnumerable<Order> All()
        { 
            return Context.orders.Include(o=>o.User).ToList();
        }
        public IEnumerable<Order> AllConfirmed()
        {
            return Context.orders.Include(o => o.User).Where(o => o.IsConfirmed == true && o.IsShippedAndPay == false).ToList();
        }

        public IEnumerable<Order> AllShippedAndPay()
        {
            return Context.orders.Include(o => o.User).Where(o => o.IsShippedAndPay == true).ToList();
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
            Context.orders.Update(entity);
            Context.SaveChanges();
            return entity;

        }


    }
}
