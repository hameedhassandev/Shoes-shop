using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using System.Diagnostics;

namespace Shoes_shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IShoesService ShoesService;

        public HomeController(ILogger<HomeController> logger, IShoesService _ShoesService)
        {
            _logger = logger;
            ShoesService = _ShoesService;
        }
        public IActionResult Index()
        {
            if (User.IsInRole(RolesName.AdminRole))
            {
                return RedirectToAction("Dashboard", "Account");
            }
            return View();
        }
        public IActionResult Shop(int pageSize=12, int pageNumber=1)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var allShoeses = ShoesService.All().ToList().Skip(excludeRecords)
                                          .Take(pageSize); ;
            if (allShoeses == null)
                return NotFound();

            var maxPrice = ShoesService.MaxPrice();
            var minPrice = ShoesService.MinPrice();
            var avgPrice = ShoesService.AvgPrice();
            var maxSize = ShoesService.MaxSize();
            var minSize = ShoesService.MinSize();

            ViewBag.maxPrice = maxPrice; 
            ViewBag.minPrice = minPrice;
            ViewBag.avgPrice = avgPrice;
            ViewBag.maxSize = maxSize;
            ViewBag.minSize = minSize;

            int ShoesCount = ShoesService.All().Count();

            var result = new PagedResult<Shoes>
            {
                Data = allShoeses.ToList(),
                TotalItems = ShoesCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}