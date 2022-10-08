using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public interface IOrderService
    {
        public Order Add(Order entity);
        public IEnumerable<Order> All();
        public Order Delete(Order entity);
        public  IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate);
        public Order GetOrder(int id);
        public Order Update(Order entity);





    }
}
