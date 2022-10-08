using Shoes_shop.Data;
using System;

namespace Shoes_shop.Models.Repositories
{
    public class CategoryService : BaseRepository<Category>
    {
        public CategoryService(ApplicationDbContext _Context) : base(_Context)
        {
        }
    }
}
