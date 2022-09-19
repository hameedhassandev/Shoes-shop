using Microsoft.AspNetCore.Mvc;

namespace Shoes_shop.Controllers
{
    public class ShoesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
