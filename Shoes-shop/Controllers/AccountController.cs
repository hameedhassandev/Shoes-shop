using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using Shoes_shop.ViewModels;
using System.Security.Principal;

namespace Shoes_shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly ILogger<AccountController> Logger;
        private readonly IShoesService shoesService;
        public readonly IBaseRepository<Category> CategoryService;
        private readonly IOrderService orderService;

        public AccountController(UserManager<ApplicationUser> _UserManager,
            SignInManager<ApplicationUser> _SignInManager, ILogger<AccountController> _Logger,
            IShoesService _shoesService,
             IBaseRepository<Category> _CategoryService,
             IOrderService _orderService)
        {
            UserManager = _UserManager;
            SignInManager = _SignInManager;
            Logger = _Logger;
            shoesService = _shoesService;
            CategoryService = _CategoryService;
            orderService = _orderService;
        }

        [Authorize(Roles =RolesName.AdminRole)]
        public IActionResult Dashboard()
        {
            var shoesCount = shoesService.All().Count();
            var categoryCount = CategoryService.All().Count();
            var orderCount = orderService.All().Count();
            var orderConfirmedCount = orderService.OrderReports().Count();
            ViewBag.shoesCount = shoesCount;
            ViewBag.categoryCount = categoryCount;
            ViewBag.orderCount = orderCount;
            ViewBag.orderConfirmedCount = orderConfirmedCount;

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByNameAsync(model.Email);

                if (user != null) 
                {
                    var roles = await UserManager.GetRolesAsync(user);

                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RemomberMe, false);
                    if (result.Succeeded)
                    {

                        if (roles[0] == RolesName.AdminRole)
                        {
                            return RedirectToAction("Dashboard", "Account");

                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                    ModelState.AddModelError("", "Invalid username or password");

            }
            return View(model);
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName  = model.FullName,
                    Address = model.Address,
                    Contact = model.Contact,
                    Gender = model.Gender
                    
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //to assign role to user where register
                    await UserManager.AddToRoleAsync(user, RolesName.UserRole);

                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }


            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }






    }
}
