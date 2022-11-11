﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using Shoes_shop.ViewModels;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.UserRole)]
    public class CartsController : Controller
    {
        private readonly IShoesService shoesService;
        private readonly UserManager<ApplicationUser> UserManager;
        private static string userId = "";
        private readonly ICartService cartService;
        private static string userAdress = "";
        private static string contact = "";
        public CartsController(IShoesService _shoesService, UserManager<ApplicationUser> _userManager, ICartService _cartService)
        {
            shoesService = _shoesService;
            UserManager = _userManager;
            cartService = _cartService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            userId = user.Id;
            userAdress = user.Address;
            contact = user.Contact;
            return View();
        }

        [Route("Carts/my-cart/")]
        public IActionResult MyCart()
        { 
            var allIncarts = cartService.GetAllItems(userId);
            var totalCartPrice = allIncarts.Sum(t => t.TotalPrice);
            ViewBag.totalCartPrice = totalCartPrice;
            ViewBag.userAdress = userAdress;
            ViewBag.contact = contact;
            return PartialView("_cart",allIncarts);
        }

        private string ValidationMassage(int shoesID, int qty)
        {
            string massage = string.Empty;
            var shoes = shoesService.Get(shoesID);

            if (shoes != null)
            {
                if (qty == 0) massage = "Quantity of shoes cannot be zero!";
                else if (qty < 0) massage = "Not eligible to add negative qty";
                else if (shoes.NumberInStock == 0) massage = "Shoes is out of stock!";
                else if (shoes.NumberInStock < qty) massage = "Quantity is not available!";
                else if (User.Identity.Name == null) massage = "Unauthorized, kindly sign in";  
            }
            return massage;
        }


        [Route("Carts/AddOneToCart/{shoesId:int}")]
        ///Carts/AddOneToCart/${shoesId}
        public JsonResult AddOneToCart([FromRoute] int shoesId)
        {
            // var result =  cartService.AddItem('0288af05-f000-4e3c-b70d-e1d4b3e3c14e', c.ShoesId, c.Qty);
            cartService.AddItem("0288af05-f000-4e3c-b70d-e1d4b3e3c14e", shoesId, 1);
            return Json("student saved successfully");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //Carts/AddToCart/
        [Route("Carts/AddToCart/")]
        public async Task<IActionResult> AddToCart([FromBody] CartsVM c)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
           // var massageValue = ValidationMassage(model.Id, model.Quantity);
            var shoes = shoesService.Get(c.ShoesId);

     /*       if (massageValue != "")
            {
                TempData["message"] = massageValue;
                return RedirectToAction("ShoesDetails", "Shoes", new { id = model.Id });
            }*/

            //in case no validation massage
            cartService.AddItem(user.Id, c.ShoesId, c.Qty);
            TempData["message"] = "Added to Cart Successfully!";
            return RedirectToAction("ShoesDetails", "Shoes", new { id = c.ShoesId });
        }

       
        [Route("Carts/Remove/{ShoesId:int}")]
        public IActionResult Remove([FromRoute] int ShoesId)
        {
          
            cartService.RemoveItem(userId, ShoesId);
            return Ok();
        }

        [HttpPost]
        [Route("Carts/Decrease/")]
        public IActionResult Decrease([FromBody] Cart c)
        {
            cartService.DecreaseItemByOne(userId, c.ShoesId);
            return Ok();
        }

        [HttpPost]
        [Route("Carts/Increase/")]
        public IActionResult Increase([FromBody] Cart c)
        {
            cartService.IncreaseItemByOne(userId, c.ShoesId);
            return Ok();
        }

        [Route("Carts/Clear-Cart/")]
        public IActionResult ClearCart()
        {
            cartService.ClearCart(userId);
            return Ok();
        }

        public IActionResult ToOrder()
        {
            
            cartService.ToOrder(userId,userAdress,contact);

            return RedirectToAction("Index", "Orders");
        }
    }
}
