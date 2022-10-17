using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models.Repositories;
using Shoes_shop.ViewModels;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.UserRole)]
    public class CartsController : Controller
    {
        private readonly IShoesService shoesService;
        private readonly UserManager<IdentityUser> UserManager;

        private readonly ICartService cartService;  

        public CartsController(IShoesService _shoesService, UserManager<IdentityUser> _userManager, ICartService _cartService)
        {
            shoesService = _shoesService;
            UserManager = _userManager;
            cartService = _cartService; 
        }

        public async Task<IActionResult> MyCart()
        {
           var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

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
                if (qty == 0)
                {
                    massage = "Quantity of shoes cannot be zero!";

                }
                else if (shoes.NumberInStock == 0)
                {
                    massage = "Shoes is out of stock!";

                }
                else if (shoes.NumberInStock < qty)
                {
                    massage = "Quantity is not available!";

                }
                else if (User.Identity.Name == null)
                {
                    massage = "Unauthorized, kindly sign in";
                }
            }

            return massage;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(ShoesViewModel model)
        {
            var massageValue = ValidationMassage(model.Id, model.Quantity);
            var shoes = shoesService.Get(model.Id);

            if (massageValue != "")
            {
                TempData["message"] = massageValue;
                return RedirectToAction("ShoesDetails", "Shoes", new { id = model.Id });
            }
           

            //in case no validation massage
            var user = await UserManager.FindByNameAsync(User.Identity.Name);

            cartService.AddItem(user.Id, model.Id, model.Quantity);
            TempData["message"] = "Added to Cart Successfully!";

            return RedirectToAction("ShoesDetails", "Shoes", new { id = model.Id });

        }
        public async Task<IActionResult> RemoveFromCart(int shoesId)
        {
            var shoes = shoesService.Get(shoesId);
            if (shoes == null)
                return NotFound();

            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (User.Identity.Name == null)
                TempData["message"] = "Not Authorized, SignIn...";


            cartService.RemoveItem(user.Id, shoesId);

            return View(nameof(MyCart));

        }


    }
}
