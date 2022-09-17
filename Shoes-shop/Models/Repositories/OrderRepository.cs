using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ApplicationDbContext _Context) : base(_Context)
        {
        }

        public override Order Add(Order entity) 
        { 
            Context.orders.Add(entity);
            Context.SaveChanges();
            return Context.orders.FirstOrDefault(o => o.UserId == entity.UserId && o.dateTime == entity.dateTime);
        }

        public override IEnumerable<Order> All()
        {
            return Context.orders.ToList();
        }

        public override Order Delete(Order entity)
        {
            Context.orders.Remove(entity);
            Context.SaveChanges();
            return entity;
        }

        //order by desc using datetime  
        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
           return Context.orders.Include(o => o.OrderDetails).Where(predicate).OrderByDescending(o => o.dateTime).ToList();
        }

        public override Order Get(int id)
        {
           return Context.orders.Include(o=>o.User).FirstOrDefault(o => o.Id == id); 
        }

        public override Order Update(Order entity)
        {
            var order = Context.orders.Attach(entity);
            order.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Context.SaveChanges();
            return entity;

        }


    }
}
