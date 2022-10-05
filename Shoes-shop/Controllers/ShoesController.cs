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
    public class ShoesController : Controller
    {

        private readonly IImageHandler ImageHandler;

        private readonly IMapper _mapper;
        public UserManager<IdentityUser> UserManager { get; }
        private IBaseRepository<Category> CategoryRepository { get; }
        public readonly IShoesRepository ShoesRepository;
        public readonly ICartRepository CartRepository;
        public ShoesController(IMapper mapper, IShoesRepository repositoryy,
            IBaseRepository<Category> categoryContext,
            IImageHandler imageHandler,
            ICartRepository _CartRepository
            , UserManager<IdentityUser> _UserManager
            )
        {
            UserManager = _UserManager;
            ImageHandler = imageHandler;
            CartRepository = _CartRepository;
            CategoryRepository = categoryContext;
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
            ViewData["CategoryId"] = new SelectList(CategoryRepository.All(), "Id", "Name");
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

            ViewData["CategoryId"] = new SelectList(CategoryRepository.All(), "Id", "Name");
            return View();

        }

        //get shoes edit form by id
        public IActionResult Edit(int id)
        {
            var shoes = ShoesRepository.Get(id);
            if (shoes == null)
                return NotFound();

            ViewData["CategoryId"] = new SelectList(CategoryRepository.All(), "Id", "Name");
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
            ViewData["CategoryId"] = new SelectList(CategoryRepository.All(), "Id", "Name", shoes.CategoryId);
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
            //make exp. 
            Expression<Func<Shoes,bool>> predicate = e=> e.CategoryId == shoes.CategoryId && e.Id != shoes.Id;

            var relatedShoes = ShoesRepository.GetRelatedShoes(predicate)
            .Select(e => _mapper.Map<ShoesViewModel>(e));

            ViewBag.RelatedShoes = relatedShoes;

            return View("ShoesDetails", _mapper.Map<ShoesViewModel>(shoes));

        }


    }


   
}
