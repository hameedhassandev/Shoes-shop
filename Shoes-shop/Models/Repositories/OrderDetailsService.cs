using Microsoft.EntityFrameworkCore;
using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly ApplicationDbContext Context;
        private readonly IShoesService shoesService;

        public OrderDetailsService(ApplicationDbContext _context, IShoesService _shoesServic)
        {
            Context = _context;
            shoesService = _shoesServic;
        }


        public OrderDetail Add(OrderDetail entity)
        {
            Context.Add(entity);    
            Context.SaveChanges();

            //when add order details on database remove qty of shoes fron stock 
            var shoes = shoesService.Get(entity.ShoesId);
            shoes.NumberInStock -= entity.Quantity;

            Context.Shoes.Update(shoes);

            return entity;  
        }

        public IEnumerable<OrderDetail> Find(Expression<Func<OrderDetail, bool>> predicate)
        {
            return Context.OrderDetails.Include(op => op.Shoes).Where(predicate).ToList();
        }



    }
}
