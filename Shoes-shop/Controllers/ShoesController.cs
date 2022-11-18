using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using Shoes_shop.ViewModels;
using cloudscribe.Pagination.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.AdminRole)]

    public class ShoesController : Controller
    {


        private readonly IMapper _mapper;
        public UserManager<ApplicationUser> UserManager { get; }
        public readonly IBaseRepository<Category> CategoryService;
        public readonly IShoesService ShoesRepository;
        public readonly ICartService CartService;
        private readonly IImageHelper ImageHelper;
        private readonly SignInManager<ApplicationUser> SignInManager;
        public INotyfService _notifyService { get; }



        public ShoesController(IMapper mapper, IShoesService repositoryy,
            IBaseRepository<Category> _CategoryService,
            IImageHelper _ImageHelper, ICartService _CartService,
            UserManager<ApplicationUser> _UserManager,
            SignInManager<ApplicationUser>
            _SignInManager, INotyfService notifyService)
        {
            UserManager = _UserManager;
            ImageHelper = _ImageHelper;
            CartService = _CartService;
            CategoryService = _CategoryService;
            SignInManager = _SignInManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ShoesRepository = repositoryy ?? throw new ArgumentNullException(nameof(repositoryy));
            _notifyService = notifyService;

        }



        public IActionResult Index(string searchString,
            string sortOrder, int filterByCategory,
            int pageSize = 5, int pageNumber = 1)
        {
            _notifyService.Success("List All Shoeses Successfully!", 5);


            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.SizeSortParam = String.IsNullOrEmpty(sortOrder) ? "size_desc" : "";
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StockSortParam = String.IsNullOrEmpty(sortOrder) ? "stock_desc" : "";
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentSearchFilter = searchString;
            ViewBag.CurrentFilterByCategory = filterByCategory;
            ViewData["CategoryId"] = new SelectList(CategoryService.All(), "Id", "Name");

            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var Sh = from s in ShoesRepository.All()
                     select s;

            if (Sh == null)
                return NotFound();

            //sorting
            switch (sortOrder)
            {
                case "price_desc":
                    Sh = ShoesRepository.OrderPriceByDesc();
                    break;
                case "size_desc":
                    Sh = ShoesRepository.OrderSizeByDesc();
                    break;
                case "name_desc":
                    Sh = ShoesRepository.OrderNameByDesc();
                    break;
                case "stock_desc":
                    Sh = ShoesRepository.OrderNoInStockByDesc();
                    break;
                default:
                    Sh = ShoesRepository.OrderById();
                    break;
            }

            //search
            var ShoesCount = Sh.Count();
            if (!String.IsNullOrEmpty(searchString))
            {
                Sh = ShoesRepository.Search(searchString);
                ShoesCount = Sh.Count();
            }
            if (filterByCategory != 0){
                Sh = ShoesRepository.SearchByCategory(filterByCategory);
                ShoesCount = Sh.Count();
            }
         
            var AllShoes = Sh.Skip(excludeRecords).Take(pageSize);

           // int ShoesCount = ShoesRepository.All().Count();

            var result = new PagedResult<Shoes>
            {
                Data = AllShoes.ToList(),
                TotalItems = ShoesCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }

        //get shoes details by id
        public IActionResult Details(int id)
        {
            _notifyService.Information("Shoes Details", 5);

            var shoes = ShoesRepository.Get(id);

            if (shoes == null)
                return NotFound();

            return View(shoes);
        }

        //get create shoes form
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(CategoryService.All(), "Id", "Name");
            return View();
        }

        //post create form of shoes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shoes shoes)
        {
            _notifyService.Information("Shoes Added Successfully", 7);

            if (ModelState.IsValid)
            {
                string UniqueName = ImageHelper.UploadImage(shoes);
                shoes.ImageURL = UniqueName;
                ShoesRepository.Add(shoes);
                ShoesRepository.CommitChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(CategoryService.All(), "Id", "Name");
            return View();

        }

        //get shoes edit form by id
        public IActionResult Edit(int id)
        {

            var shoes = ShoesRepository.Get(id);
            if (shoes == null)
                return NotFound();

            ViewData["CategoryId"] = new SelectList(CategoryService.All(), "Id", "Name");
            return View(shoes);

        }

        //post shoes edit form by id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Shoes shoes)
        {
            _notifyService.Information("Shoes Updated Successfully", 7);

            if (id == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (shoes.ImageFile != null)
                {
                    shoes.ImageURL = ImageHelper.UploadImage(shoes);

                }

                ShoesRepository.Update(shoes);
                ShoesRepository.CommitChanges();
                return RedirectToAction(nameof(Index));

            }
            ViewData["CategoryId"] = new SelectList(CategoryService.All(), "Id", "Name", shoes.CategoryId);
            return View(shoes);
        }

        //get delete shoes by id
        public IActionResult Delete(int id)
        {

            var shoes = ShoesRepository.Get(id);
            if (shoes == null)
                return NotFound();

            return View(shoes);
        }

        //post delete shoes by id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            _notifyService.Error("Shoes Deleted Successfully", 7);


            var shoes = ShoesRepository.Get(id);
            if (shoes == null)
                return NotFound();

            ShoesRepository.Delete(shoes);
            ShoesRepository.CommitChanges();

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult ShoesDetails(int id)
        {
           // ViewData["message"] = ViewBag["massage"];
            var shoes = ShoesRepository.Get(id);
            if (shoes == null)
                return NotFound();
           
            return View(nameof(ShoesDetails), _mapper.Map<ShoesViewModel>(shoes));

        }

    }
}