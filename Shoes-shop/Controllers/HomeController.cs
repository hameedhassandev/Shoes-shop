using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using System.Data;
using System.Diagnostics;

namespace Shoes_shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IShoesService ShoesService;
        public readonly IBaseRepository<Category> CategoryService;

        public HomeController(ILogger<HomeController> logger, IShoesService _ShoesService, IBaseRepository<Category> _CategoryService)
        {
            _logger = logger;
            ShoesService = _ShoesService;
            CategoryService = _CategoryService;
        }
        public IActionResult Index()
        {
           

            if (User.IsInRole(RolesName.AdminRole))
            {
                return RedirectToAction("Dashboard", "Account");
            }
            return View();
        }

        public IActionResult Shop(int filterByCategory, int filterBySize, 
            double filterByPrice ,string search_query, int pageSize = 12, int pageNumber = 1)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;

            var TempShoeses = from s in ShoesService.All()
                              select s;

           
            if (TempShoeses == null)
                return NotFound();

            var ShoesCount = ShoesService.All().Count();
            var maxPrice = ShoesService.MaxPrice();
            var minPrice = ShoesService.MinPrice();
            var avgPrice = ShoesService.AvgPrice();
            var maxSize = ShoesService.MaxSize();
            var minSize = ShoesService.MinSize();

            ViewData["CategoryId"] = new SelectList(CategoryService.All(), "Id", "Name");


            ViewBag.maxPrice = maxPrice;
            ViewBag.minPrice = minPrice;
            ViewBag.avgPrice = avgPrice;
            ViewBag.maxSize = maxSize;
            ViewBag.minSize = minSize;
            ViewBag.CurrentFilterByCategory = filterByCategory;
            ViewBag.filterBySize = filterBySize;
            ViewBag.filterByPrice = filterByPrice;
            ViewBag.search_query = search_query;


            if (!String.IsNullOrEmpty(search_query))
            {
                TempShoeses = ShoesService.Search(search_query);
                ShoesCount = TempShoeses.Count();
            }


            if (filterByCategory !=0 && filterByPrice !=0 && filterBySize != 0)
            {
                TempShoeses = ShoesService.SearchByCategory(filterByCategory).Where(p => p.Price <= filterByPrice && p.Size == filterBySize);
                ShoesCount = TempShoeses.Count();
            }else if(filterByCategory != 0 && filterByPrice != 0)
            {  
                TempShoeses = ShoesService.SearchByCategory(filterByCategory).Where(p => p.Price <= filterByPrice);
                ShoesCount = TempShoeses.Count();

            }else if(filterByCategory != 0 && filterBySize != 0)
            {
                TempShoeses = ShoesService.SearchByCategory(filterByCategory).Where(p => p.Size == filterBySize);
                ShoesCount = TempShoeses.Count();

            }else if (filterBySize != 0 && filterByPrice != 0)
            {
                TempShoeses = ShoesService.All().Where(p => p.Size == filterBySize && p.Price <= filterByPrice);
                ShoesCount = TempShoeses.Count();
            }
            else if (filterByCategory != 0)
            {
                TempShoeses = ShoesService.SearchByCategory(filterByCategory);
                ShoesCount = TempShoeses.Count();
            }
            else if (filterByPrice != 0)
            {
                TempShoeses = ShoesService.All().Where(p=>p.Price <= filterByPrice);
                ShoesCount = TempShoeses.Count();
            }
            else if (filterBySize != 0)
            {
                TempShoeses = ShoesService.All().Where(p => p.Size == filterBySize);
                ShoesCount = TempShoeses.Count();
            }
           
        

            var AllShoesAfterFilter = TempShoeses.Skip(excludeRecords).Take(pageSize);
            var result = new PagedResult<Shoes>
            {
                Data = AllShoesAfterFilter.ToList(),
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