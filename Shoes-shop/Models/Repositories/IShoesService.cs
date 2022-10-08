using LinqKit;
using System.Linq.Expressions;


namespace Shoes_shop.Models.Repositories
{
    public interface IShoesService : IBaseRepository<Shoes>
    {
        public IEnumerable<Shoes> GetShoesWitPaging(ExpressionStarter<Shoes> predicate, int pageNumber);
        public int GetTotalShoesPages(ExpressionStarter<Shoes> predicate);
        public IEnumerable<Shoes> GetRelatedShoes(Expression<Func<Shoes, bool>> predicate);
    }
}
