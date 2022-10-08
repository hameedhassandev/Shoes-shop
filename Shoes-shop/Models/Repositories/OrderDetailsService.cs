using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly ApplicationDbContext Context;
        public OrderDetailsService(ApplicationDbContext context)
        {
            Context = context;
        }


        public OrderDetail Add(OrderDetail entity)
        {
            Context.Add(entity);    
            Context.SaveChanges();
            return entity;  
        }

        public IEnumerable<OrderDetail> Find(Expression<Func<OrderDetail, bool>> predicate)
        {
            return Context.OrderDetails.Include(op => op.Shoes).Where(predicate).ToList();
        }



    }
}
