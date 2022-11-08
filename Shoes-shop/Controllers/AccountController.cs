using Microsoft.AspNetCore.Mvc;

namespace Shoes_shop.Controllers
{
    public class AccountController : Controller
    {
       public IActionResult Login()
        {
            return View();
        }
    }
}
