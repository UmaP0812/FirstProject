using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using BulkyWeb.DataAccess.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)

        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //create
                return View(productViewModel);
            }
            else
            {
                //update
                productViewModel.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productViewModel);
            }
        }
        /*IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
          .GetAll().Select(u => new SelectListItem
          {
              Text = u.Name,
              Value = u.Id.ToString()
          });*/
        //  ViewBag.CategoryList = CategoryList;
        //ViewData["CategoryList"] = CategoryList;

        // return View(productViewModel);

        

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product"); //+ productViewModel.Product.Id;
                                                                                       // string finalPath = Path.Combine(wwwRootPath, productPath);
                    /*if(!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                       {
                       //deleete old img
                       var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                   if(System.IO.File.Exists(oldImagePath))

                       {
                           System.IO.File.Delete(oldImagePath);
                       }
                   }*/

                    /*if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);*/

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productViewModel.Product.ImageUrl = @"images\product\" + fileName;
                }
                /*if (productViewModel.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productViewModel.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productViewModel.Product);
                }*/
                //_unitOfWork.Save();
                /*  ProductImage productImage = new()
                  {
                      ImageUrl = @"\" + productPath + @"\" + fileName,
                      ProductId = productViewModel.Product.Id,
                  };*/

                //if (productViewModel.Product.ProductImages == null)
                //    productViewModel.Product.ProductImages = new List<ProductImage>();

                //productViewModel.Product.ProductImages.Add(productImage);


                _unitOfWork.Product.Update(productViewModel.Product);
                _unitOfWork.Save();

                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");

            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productViewModel);
            }
        }




        //public IActionResult Create(ProductViewModel productViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(productViewModel.Product);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });

        //        return View(productViewModel);
        //    }
        //}

        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product ProductFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

       /* public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product ProductFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted successfully";
            return RedirectToAction("Index");
        }*/


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}
#endregion



/* public class ProductController : Controller
 {
     private readonly ApplicationDbContext _db;
     public ProductController(ApplicationDbContext db) 
     { 
         _db = db;
     }
     public IActionResult Index()
     {
         List<Product> objProductList = _db.Categories.ToList();
         return View(objProductList);
     }

     public IActionResult Create()
     {
         return View();
     }
     [HttpPost]
     public IActionResult Create(Product obj)
     {
         if (ModelState.IsValid)
         {
            *//* _unitOfWork.Product.Add(obj);
             _unitOfWork.Save();*//*
             _db.Categories.Add(obj);
             _db.SaveChanges();
             TempData["Success"] = "Product Created successfully";
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
        Product ProductFromDb = _db.Categories.Find(id);
         if (ProductFromDb == null)
         {
             return NotFound();
         }

         return View(ProductFromDb);
     }
     [HttpPost]
     public IActionResult Edit(Product obj)
     {
         if (ModelState.IsValid)
         {
             //_unitOfWork.Product.Update(obj);
            // _unitOfWork.Save();
            _db.Categories.Update(obj);
             _db.SaveChanges();
             TempData["Success"] = "Product Updated successfully";
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
         Product ProductFromDb = _db.Categories.Find(id);
         if (ProductFromDb == null)
         {
             return NotFound();
         }

         return View(ProductFromDb);
     }
     [HttpPost, ActionName("Delete")]
     public IActionResult DeletePost(int id)
     {
         Product obj = _db.Categories.Find(id);
         if (obj == null)
         {
             return NotFound();
         }
         *//* Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
          if (obj == null)
          {
              return NotFound();
          }
          _unitOfWork.Product.Remove(obj);
          _unitOfWork.Save();*//*
         _db.Categories.Remove(obj);
         _db.SaveChanges();
         TempData["Success"] = "Product Deleted successfully";
         return RedirectToAction("Index");
     }

 }
}
*/