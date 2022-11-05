using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shoes_shop.Helpers;
using Shoes_shop.Models;
using Shoes_shop.Models.Repositories;
using System.Linq.Expressions;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.AdminRole)]

    public class CategoriesController : Controller
    {

        private readonly IBaseRepository<Category> _context;

        public CategoriesController(IBaseRepository<Category> context)
        {
            _context = context; 
        }

        public IActionResult Index(int pageSize = 1, int pageNumber = 1)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var categories = _context.All().Skip(excludeRecords).Take(pageSize);
            int categoriesCount = _context.All().Count();

            var result = new PagedResult<Category>
            {
                Data = categories.ToList(),
                TotalItems = categoriesCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }

        //get details by id
        public IActionResult Details(int id)
        {
            var category = _context.Get(id);

            if (category == null)
                return NotFound();

            return View(category);  

        }

        //get create category form
        public IActionResult Create()
        {
            return View();
        }

        //post to create category form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                _context.CommitChanges();
                return RedirectToAction("Index");

            }
            return View(category);

        }

        //get edit category form with id
        public IActionResult Edit(int id)
        {
            var category = _context.Get(id);
            if (category == null)
                return NotFound();


            return View(category);
        }

        //post to edit category form with id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id == null)
                return NotFound();

            var cat = _context.Get(id);

            cat.Name = category.Name;
            cat.Description = category.Description;

            if (ModelState.IsValid)
            {
                _context.CommitChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        //get delete category with id
        public IActionResult Delete(int id)
        {
            var category = _context.Get(id);
            if (category == null)
                return NotFound();


            return View(category);
        }

        //post delete category with id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {

            var category = _context.Get(id);
            if (category == null)
                return NotFound();

            _context.Delete(category);
            _context.CommitChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Exsist(string Name, int id)
        {
            Expression<Func<Category, bool>> predicate = c => c.Name == Name;
            var category = _context.FindOne(predicate);

            if (category == null)
                return Json(true);
            else
            {
                if (category.Id == id)
                    return Json(true);
                return Json(false);
            }

        }

    }
}
