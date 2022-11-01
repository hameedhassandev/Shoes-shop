using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shoes_shop.Data;
using System;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class ShoesService : BaseRepository<Shoes>, IShoesService
    {
        public ShoesService(ApplicationDbContext _Context) : base(_Context)
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

   
   
    }
}
