using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using Shoes_shop.ViewModels;
using System.Linq.Expressions;
using cloudscribe.Pagination.Models;

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

        public ShoesController(IMapper mapper, IShoesService repositoryy,
            IBaseRepository<Category> _CategoryService,
            IImageHelper _ImageHelper, ICartService _CartService, 
            UserManager<ApplicationUser> _UserManager)
        {
            UserManager = _UserManager;
            ImageHelper = _ImageHelper;
            CartService = _CartService;
            CategoryService = _CategoryService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ShoesRepository = repositoryy ?? throw new ArgumentNullException(nameof(repositoryy));
        }



        public IActionResult Index(int pageSize = 5, int pageNumber = 1)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var AllShoes = ShoesRepository.All()
                                          .Skip(excludeRecords)
                                          .Take(pageSize);

            int ShoesCount = ShoesRepository.All().Count();
            ViewBag.ShoesCount = ShoesCount;

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