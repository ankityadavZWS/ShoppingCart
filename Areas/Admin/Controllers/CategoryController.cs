using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Repository;
using ShoppingCart.DataAccess.ViewModels;

namespace MVC_Sample.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Categories = _unitOfWork.CategoryRepository.GetAll();
            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryViewModel categoryViewModel = new();

            if (id == null || id == 0)
            {
                return View(categoryViewModel);
            }
            else
            {
                categoryViewModel.Category = _unitOfWork.CategoryRepository.GetT(x => x.Id == id);
                if (categoryViewModel.Category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(categoryViewModel);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                if (categoryViewModel.Category.Id == 0)
                {
                    _unitOfWork.CategoryRepository.Add(categoryViewModel.Category);
                    TempData["Success"] = "Category added successfully!";
                }
                else
                {
                    _unitOfWork.CategoryRepository.Update(categoryViewModel.Category);
                    TempData["Success"] = "Category updated successfully!";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = _unitOfWork.CategoryRepository.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteData(int? id)
        {
            var category = _unitOfWork.CategoryRepository.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Save();

            TempData["Success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
