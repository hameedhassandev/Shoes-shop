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

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.AdminRole)]

    public class ShoesController : Controller
    {

        private readonly IImageHandler ImageHandler;

        private readonly IMapper _mapper;
        public UserManager<IdentityUser> UserManager { get; }
        public readonly IBaseRepository<Category> CategoryService;

       // public readonly CategoryService CategoryService;
        public readonly IShoesService ShoesRepository;
        public readonly ICartService CartRepository;
        public ShoesController(IMapper mapper, IShoesService repositoryy,
            IBaseRepository<Category> _CategoryService,
            IImageHandler imageHandler,
            ICartService _CartRepository,
            UserManager<IdentityUser> _UserManager
            )
        {
            UserManager = _UserManager;
            ImageHandler = imageHandler;
            CartRepository = _CartRepository;
            CategoryService = _CategoryService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ShoesRepository = repositoryy ?? throw new ArgumentNullException(nameof(repositoryy));
        }


        public IActionResult Index()
        {
            var AllShoes = ShoesRepository.All();
            return View(AllShoes);
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
                string UniqueName = ImageHandler.UploadImage(shoes);
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
                    shoes.ImageURL = ImageHandler.UploadImage(shoes);

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
            ViewData["addMessage"] = TempData["message"];
            var shoes = ShoesRepository.Get(id);
            //make exp. to get all related shoeses with the same category exept selected shoes
            Expression<Func<Shoes, bool>> predicate = e => e.CategoryId == shoes.CategoryId && e.Id != shoes.Id;

            var relatedShoes = ShoesRepository.GetRelatedShoes(predicate)
            .Select(e => _mapper.Map<ShoesViewModel>(e));

            ViewBag.RelatedShoes = relatedShoes;

            return View(nameof(ShoesDetails), _mapper.Map<ShoesViewModel>(shoes));

        }

        private string ValidationMassage(int shoesID, int qty)
        {
            string massage = string.Empty;
            var shoes = ShoesRepository.Get(shoesID);
            if (qty == 0)
            {
                massage = "Quantity of shoes cannot be zero!";

            }
            else if (shoes.NumberInStock == 0)
            {
                massage = "Shoes is out of stock!";

            }
            else if (shoes.NumberInStock < qty)
            {
                massage = "Quantity is not available!";

            }
            else if (User.Identity.Name == null)
            {
                massage = "Unauthorized, kindly sign in";
            }
            return massage;
        }

        public async Task<IActionResult> AddToCart(int shoesID, int qty)
        {
            var massageValue = ValidationMassage(shoesID, qty);
            var shoes = ShoesRepository.Get(shoesID);

            if (massageValue != null)
                TempData["message"] = massageValue;


            //in case no validation massage
            var user = await UserManager.FindByNameAsync(User.Identity.Name);

            CartRepository.AddItem(user.Id, shoesID, qty);
            TempData["message"] = "Added to Cart Successfully!";

            return RedirectToAction(nameof(ShoesDetails), new { id = shoes.Id });

        }



    }
}