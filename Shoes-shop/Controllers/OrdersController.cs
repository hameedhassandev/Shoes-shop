using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using System.Linq.Expressions;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.UserRole)]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<IdentityUser> UserManager;
        private static string userId = "";
        public OrdersController(IOrderService _orderService, UserManager<IdentityUser> _UserManager)
        {
            orderService = _orderService;
            UserManager = _UserManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            userId = user.Id;

            Expression<Func<Order, bool>> predicate = o => o.UserId == userId;
            return View(orderService.Find(predicate));
        }
    }
}
