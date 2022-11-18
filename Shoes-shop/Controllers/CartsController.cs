using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly UserManager<ApplicationUser> UserManager;
        private static string userId = "";
        private readonly ICartService cartService;
        private static string userAdress = "";
        private static string contact = "";
        public INotyfService _notifyService { get; }

        public CartsController(IShoesService _shoesService, UserManager<ApplicationUser> _userManager,
            ICartService _cartService, INotyfService notifyService)
        {
            shoesService = _shoesService;
            UserManager = _userManager;
            cartService = _cartService;
            _notifyService = notifyService;
        }

        public async Task<IActionResult> Index()
        {
            _notifyService.Success("Welcom In Your Cart..!", 5);

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


        [Route("Carts/AddOneToCart/{shoesId:int}")]
        ///Carts/AddOneToCart/${shoesId}
        public async Task<JsonResult> AddOneToCart([FromRoute] int shoesId)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Json("You Are Not Auth Sign Up");

            }

            var isExist =  cartService.existIncart(shoesId, user.Id);
            if(isExist)
                return Json("Alerdy in your cart!");


            // var result =  cartService.AddItem('0288af05-f000-4e3c-b70d-e1d4b3e3c14e', c.ShoesId, c.Qty);
            cartService.AddItem(user.Id, shoesId, 1);

            return Json("Shoes Added successfully to your cart");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //Carts/AddToCart/
        [Route("Carts/AddToCart/")]
        public async Task<IActionResult> AddToCart(ShoesViewModel c)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if(user == null)
            {
                return RedirectToAction("ShoesDetails", "Shoes", new { id = c.Id });

            }

            var isExist = cartService.existIncart(c.Id,user.Id);
            if (isExist)
            {
                _notifyService.Warning("Exist In Your Cart", 5);

                return RedirectToAction("ShoesDetails", "Shoes", new { id = c.Id });
            }
            if(c.Quantity == 0)
            {
                _notifyService.Warning("Qty Not Allow To Be Zero", 5);

                return RedirectToAction("ShoesDetails", "Shoes", new { id = c.Id });
            }
           

            cartService.AddItem(user.Id, c.Id, c.Quantity);
            _notifyService.Success("Added to Cart Successfully!", 5);
            return RedirectToAction("ShoesDetails", "Shoes", new { id = c.Id });
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

        [Route("Carts/Count-Cart/")]

        public async Task<JsonResult> UserCartCount()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            
            var count = cartService.UserCartCount(user.Id);

            ViewBag.count = count;
            return Json(count);    

        }

        public IActionResult ToOrder()
        {
            
            cartService.ToOrder(userId,userAdress,contact);

            return RedirectToAction("Index", "Orders");
        }
    }
}
