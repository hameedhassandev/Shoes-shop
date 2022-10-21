using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using System.Data;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.UserRole)]
    public class ShippingController : Controller
    {
        private readonly IShippingService shippingService;
        private readonly UserManager<IdentityUser> UserManager;

        public ShippingController(IShippingService _shippingService, UserManager<IdentityUser> _UserManager)
        {
            shippingService = _shippingService;
            UserManager = _UserManager;
        }
    
        //get/shipping/ form 
        public IActionResult MyShipping()
        {
            return View();
        }


        public IActionResult ShippingDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMyShipping(Shipping model)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            
                Shipping shipping = new Shipping();
                shipping.Address = model.Address;
                shipping.Contact = model.Contact;
                shipping.ExtraDetails = model.ExtraDetails;
            if (ModelState.IsValid)
            {
                var result = shippingService.AddShipping(user.Id, shipping);

                return Ok(result);
            }
            return Redirect(nameof(MyShipping));
        }
    }
}
