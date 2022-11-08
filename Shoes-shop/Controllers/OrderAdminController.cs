﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models.Repositories;
using Shoes_shop.Models;
using Shoes_shop.ViewModels;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.AdminRole)]
    public class OrderAdminController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IOrderDetailsService orderDetailsService;
        private readonly UserManager<ApplicationUser> UserManager;


        public OrderAdminController(IOrderService _orderService, IOrderDetailsService _orderDetailsService, UserManager<ApplicationUser> _UserManager)
        {
            orderService = _orderService;
            orderDetailsService = _orderDetailsService;
            UserManager = _UserManager;
        }

        public IActionResult Index()
        {
            var allOrders = orderService.All();

            return View(allOrders); 
        }

        public async Task<IActionResult> OrderDetails(int id)
        {

            var model = orderService.GetOrder(id);
            if (model == null)
                return NotFound();
            var orderDetails = orderDetailsService.Find(id).ToList();
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var fullName = user.FullName;

            var orderVM = new OrderAndDetailsVM
            {
                Id = model.Id,
                dateTime = model.dateTime,
                TotalPrice = model.TotalPrice,
                ShippingAddress = model.ShippingAddress,
                Contact = model.Contact,
                FullNam = fullName,
                IsConfirmed = model.IsConfirmed,
                IsShippedAndPay = model.IsShippedAndPay,
                OrderDetails = orderDetails,
            };

            return View(orderVM);
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