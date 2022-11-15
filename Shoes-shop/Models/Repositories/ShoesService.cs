using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shoes_shop.Data;
using System;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

  
        public  IEnumerable<Shoes> OrderPriceByDesc()
        {
            return Context.Shoes.Include(p => p.Category).OrderByDescending(p => p.Price).ToList();

        }
    
        public IEnumerable<Shoes> OrderSizeByDesc()
        {
            return Context.Shoes.Include(p => p.Category).OrderByDescending(p => p.Size).ToList();

        }
        public IEnumerable<Shoes> OrderNameByDesc()
        {
            return Context.Shoes.Include(p => p.Category).OrderByDescending(p => p.Name).ToList();

        }
        public IEnumerable<Shoes> OrderNoInStockByDesc()
        {
            return Context.Shoes.Include(p => p.Category).OrderByDescending(p => p.NumberInStock).ToList();

        }


        public IEnumerable<Shoes> OrderById()
        {
            return Context.Shoes.Include(p => p.Category).OrderBy(p => p.Id).ToList();

        }

        public IEnumerable<Shoes> Search(string query)
        {
            return Context.Shoes.Include(p => p.Category)
                .Where(p=>p.Name.Contains(query) ||
                p.ShortDescription.Contains(query) ||
                p.Category.Name.Contains(query) /*||
                p.Price == int.Parse(query) ||
                p.Size == int.Parse(query)*/
                ).ToList();

        }

        public IEnumerable<Shoes> SearchByCategory(int categoryId)
        {
            return Context.Shoes.Include(p => p.Category)
               .Where(p => p.CategoryId.Equals(categoryId)).ToList();
        }



        public override Shoes Get(int id)
        {
            var product = Context.Shoes.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return product;

        }


        public double MaxPrice()
        {
            return Context.Shoes.Max(p => p.Price);
        }
        public double MinPrice()
        {
            return Context.Shoes.Min(p => p.Price);

        }
        public double AvgPrice()
        {
            return Context.Shoes.Average(p => p.Price);

        }


        public int MaxSize()
        {
            return Context.Shoes.Max(p => p.Size);
        }
        public int MinSize()
        {
            return Context.Shoes.Min(p => p.Size);
        }


    }
}
