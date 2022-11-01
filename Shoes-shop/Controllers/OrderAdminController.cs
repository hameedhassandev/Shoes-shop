using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models.Repositories;
using Shoes_shop.Models;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.AdminRole)]
    public class OrderAdminController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IOrderDetailsService orderDetailsService;
        
        public OrderAdminController(IOrderService _orderService, IOrderDetailsService _orderDetailsService)
        {
            orderService = _orderService;
            orderDetailsService = _orderDetailsService;
        }

        public IActionResult Index()
        {
            var allOrders = orderService.All();

            return View(allOrders); 
        }

        public IActionResult ConfirmedOrders()
        {
            var allConfirmedOrders = orderService.AllConfirmed();

            return View(allConfirmedOrders);
        }

        public IActionResult Details(int id)
        {
            var OrderDetails = orderDetailsService.Find(id);
            if (OrderDetails == null)
                return NotFound();

            
            ViewBag.orderId = id;

            return View(OrderDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOrder(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            order.IsConfirmed = true;
            orderService.Update(order);

            return RedirectToAction(nameof(Index));
        }


    }
}
