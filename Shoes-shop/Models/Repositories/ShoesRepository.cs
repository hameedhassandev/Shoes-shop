using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shoes_shop.Data;
using Shoes_shop.Helpers;
using System;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class ShoesRepository : GenericRepository<Shoes>, IShoesRepository
    {
        public ShoesRepository(ApplicationDbContext _Context) : base(_Context)
        {
        }

        public override IEnumerable<Shoes> All()
        {
            return Context.Shoes.Include(p => p.Category).ToList();
        }
        public override Shoes Get(int id)
        {
            var product = Context.Shoes.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return product;

        }

        public IEnumerable<Shoes> GetRelatedShoes(Expression<Func<Shoes, bool>> predicate)
        {
            return Context.Shoes.AsQueryable().Where(predicate).Take(ConstantNumbers.RELATED_SHOES_NO);
        }

        public IEnumerable<Shoes> GetShoesWitPaging(ExpressionStarter<Shoes> predicate, int pageNumber)
        {
            return Context.Shoes.Where(predicate)
             .Skip((pageNumber - 1) * ConstantNumbers.SHOES_NO).Take(ConstantNumbers.SHOES_NO);
        }

        public int GetTotalShoesPages(ExpressionStarter<Shoes> predicate)
        {
            return (int)Math.Ceiling((decimal)Context.Shoes.Where(predicate).Count() / ConstantNumbers.SHOES_NO);
        }
    }
}
