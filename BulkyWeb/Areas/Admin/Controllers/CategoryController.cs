using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)

        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}






/* public class CategoryController : Controller
 {
     private readonly ApplicationDbContext _db;
     public CategoryController(ApplicationDbContext db) 
     { 
         _db = db;
     }
     public IActionResult Index()
     {
         List<Category> objCategoryList = _db.Categories.ToList();
         return View(objCategoryList);
     }

     public IActionResult Create()
     {
         return View();
     }
     [HttpPost]
     public IActionResult Create(Category obj)
     {
         if (ModelState.IsValid)
         {
            *//* _unitOfWork.Category.Add(obj);
             _unitOfWork.Save();*//*
             _db.Categories.Add(obj);
             _db.SaveChanges();
             TempData["Success"] = "Category Created successfully";
             return RedirectToAction("Index");
         }
         return View();
     }

     public IActionResult Edit(int id)
     {
         if (id == null)
         {
             return NotFound();
         }
        Category categoryFromDb = _db.Categories.Find(id);
         if (categoryFromDb == null)
         {
             return NotFound();
         }

         return View(categoryFromDb);
     }
     [HttpPost]
     public IActionResult Edit(Category obj)
     {
         if (ModelState.IsValid)
         {
             //_unitOfWork.Category.Update(obj);
            // _unitOfWork.Save();
            _db.Categories.Update(obj);
             _db.SaveChanges();
             TempData["Success"] = "Category Updated successfully";
             return RedirectToAction("Index");
         }
         return View();
     }

     public IActionResult Delete(int id)
     {
         if (id == null)
         {
             return NotFound();
         }
         Category categoryFromDb = _db.Categories.Find(id);
         if (categoryFromDb == null)
         {
             return NotFound();
         }

         return View(categoryFromDb);
     }
     [HttpPost, ActionName("Delete")]
     public IActionResult DeletePost(int id)
     {
         Category obj = _db.Categories.Find(id);
         if (obj == null)
         {
             return NotFound();
         }
         *//* Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
          if (obj == null)
          {
              return NotFound();
          }
          _unitOfWork.Category.Remove(obj);
          _unitOfWork.Save();*//*
         _db.Categories.Remove(obj);
         _db.SaveChanges();
         TempData["Success"] = "Category Deleted successfully";
         return RedirectToAction("Index");
     }

 }
}
*/