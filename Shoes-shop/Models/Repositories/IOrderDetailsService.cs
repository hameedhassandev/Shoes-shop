using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public interface IOrderDetailsService
    {
        public OrderDetail Add(OrderDetail entity);
        public IEnumerable<OrderDetail> Find(Expression<Func<OrderDetail, bool>> predicate);


    }
}
