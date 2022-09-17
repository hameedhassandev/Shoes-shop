using Shoes_shop.Data;
using System;

namespace Shoes_shop.Models.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext _Context) : base(_Context)
        {
        }
    }
}
