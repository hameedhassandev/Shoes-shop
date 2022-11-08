using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
        public IActionResult Shop(int pageSize=9, int pageNumber=1)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var allShoeses = ShoesService.All().ToList().Skip(excludeRecords)
                                          .Take(pageSize); ;
            if (allShoeses == null)
                return NotFound();


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