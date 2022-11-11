using LinqKit;
using System.Linq.Expressions;


namespace Shoes_shop.Models.Repositories
{
    public interface IShoesService : IBaseRepository<Shoes>
    {
        public IEnumerable<Shoes> OrderPriceByDesc();
        public IEnumerable<Shoes> OrderSizeByDesc();
        public IEnumerable<Shoes> OrderNoInStockByDesc();
        public IEnumerable<Shoes> OrderById();
        public IEnumerable<Shoes> OrderNameByDesc();
        public IEnumerable<Shoes> Search(string query);
        public IEnumerable<Shoes> SearchByCategory(int categoryId);
        public double MaxPrice();
        public double MinPrice();
        public double AvgPrice();
        public int MaxSize();
        public int MinSize();


    }
}
