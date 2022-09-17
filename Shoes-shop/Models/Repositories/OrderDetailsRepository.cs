using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailsRepository(ApplicationDbContext _Context) : base(_Context)
        {
        }


        public override OrderDetail Add(OrderDetail entity)
        {
            Context.Add(entity);    
            Context.SaveChanges();
            return entity;  
        }

        public override IEnumerable<OrderDetail> Find(Expression<Func<OrderDetail, bool>> predicate)
        {
            return Context.OrderDetails.Include(op => op.Shoes).Where(predicate).ToList();
        }



    }
}
