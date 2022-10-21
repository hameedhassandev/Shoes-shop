using Microsoft.AspNetCore.Authorization;
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
        private readonly UserManager<IdentityUser> UserManager;
        private static string userId = "";
        private readonly ICartService cartService;
        private readonly IShippingService shippingService;

        public CartsController(IShoesService _shoesService, UserManager<IdentityUser> _userManager, ICartService _cartService, IShippingService _shippingService)
        {
            shoesService = _shoesService;
            UserManager = _userManager;
            cartService = _cartService;
            shippingService = _shippingService;
        }

        [Route("Carts/my-cart/")]
        public async Task<IActionResult> MyCart()
        {
           var user = await UserManager.FindByNameAsync(User.Identity.Name);
           userId = user.Id;

            var allIncarts = cartService.GetAllItems(user.Id);
            var totalCartPrice = allIncarts.Sum(t => t.TotalPrice);
            ViewBag.totalCartPrice = totalCartPrice;
            return View(allIncarts);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(ShoesViewModel model)
        {
            var massageValue = ValidationMassage(model.Id, model.Quantity);
            var shoes = shoesService.Get(model.Id);

            if (massageValue != "")
            {
                TempData["message"] = massageValue;
                return RedirectToAction("ShoesDetails", "Shoes", new { id = model.Id });
            }
            //in case no validation massage
            cartService.AddItem(userId, model.Id, model.Quantity);
            TempData["message"] = "Added to Cart Successfully!";
            return RedirectToAction("ShoesDetails", "Shoes", new { id = model.Id });
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

        public IActionResult ToOrder()
        {
            cartService.ToOrder(userId);

            return RedirectToAction(nameof(MyCart));
        }
    }
}
