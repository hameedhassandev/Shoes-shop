using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public interface IOrderService
    {
        public Order Add(Order entity);
        public IEnumerable<Order> All();
        public IEnumerable<Order> OrderReports();
        public IEnumerable<Order> OrderPdfReports();
        public IEnumerable<Order> Find(string userID);
        public Order GetOrder(int id);
        public Order Update(Order entity);





    }
}
