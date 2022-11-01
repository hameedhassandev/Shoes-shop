using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using Shoes_shop.ViewModels;
using System.Linq.Expressions;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.UserRole)]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IOrderDetailsService orderDetailsService;
        private readonly UserManager<ApplicationUser> UserManager;
        private static string userId = "";
        public OrdersController(IOrderService _orderService, UserManager<ApplicationUser> _UserManager, IOrderDetailsService _orderDetailsService)
        {
            orderService = _orderService;
            UserManager = _UserManager;
            orderDetailsService = _orderDetailsService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            userId = user.Id;

            Expression<Func<Order, bool>> predicate = o => o.UserId == userId;

            var model = orderService.Find(predicate);
            
            var orderDetails = orderDetailsService.Find(7);
            ViewBag.orderDetails = orderDetails;
            return View(model);

            
        }


        public IActionResult Details (int id)
        {
          var OrderDetails = orderDetailsService.Find(id);
            if (OrderDetails == null)

                return NotFound();
            return View(OrderDetails);
        }


        //get form of shipping
        public IActionResult EdieShipping(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            var orderVM  = new OrderShipingVM
            { 
                Id = order.Id,  
                Address = order.ShippingAddress,
                Contact = order.Contact,
            };

            return View(orderVM);  
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditeShippingConfirm(int id,OrderShipingVM model)
        {
           var order  = orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                order.ShippingAddress = model.Address;
                order.Contact = model.Contact;
                orderService.Update(order);
            }
            
            return RedirectToAction(nameof(Index));
        }


    }
}
