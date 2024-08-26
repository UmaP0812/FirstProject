using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using BulkyWeb.DataAccess.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)

        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj= _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
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

        // return View(CompanyViewModel);



        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View(CompanyObj);
            }
        }





        /*  public IActionResult Create(CompanyViewModel CompanyViewModel)
          {
              if (ModelState.IsValid)
              {
                  _unitOfWork.Company.Add(CompanyViewModel.Company);
                  _unitOfWork.Save();
                  TempData["Success"] = "Company Created successfully";
                  return RedirectToAction("Index");
              }
              else
              {
                  CompanyViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

                  return View(CompanyViewModel);
              }
          }*/

        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyFromDb == null)
            {
                return NotFound();
            }

            return View(CompanyFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Company Updated successfully";
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
            Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyFromDb == null)
            {
                return NotFound();
            }

            return View(CompanyFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Company Deleted successfully";
            return RedirectToAction("Index");
        }*/


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = objCompanyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}
#endregion



/* public class CompanyController : Controller
 {
     private readonly ApplicationDbContext _db;
     public CompanyController(ApplicationDbContext db) 
     { 
         _db = db;
     }
     public IActionResult Index()
     {
         List<Company> objCompanyList = _db.Categories.ToList();
         return View(objCompanyList);
     }

     public IActionResult Create()
     {
         return View();
     }
     [HttpPost]
     public IActionResult Create(Company obj)
     {
         if (ModelState.IsValid)
         {
            *//* _unitOfWork.Company.Add(obj);
             _unitOfWork.Save();*//*
             _db.Categories.Add(obj);
             _db.SaveChanges();
             TempData["Success"] = "Company Created successfully";
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
        Company CompanyFromDb = _db.Categories.Find(id);
         if (CompanyFromDb == null)
         {
             return NotFound();
         }

         return View(CompanyFromDb);
     }
     [HttpPost]
     public IActionResult Edit(Company obj)
     {
         if (ModelState.IsValid)
         {
             //_unitOfWork.Company.Update(obj);
            // _unitOfWork.Save();
            _db.Categories.Update(obj);
             _db.SaveChanges();
             TempData["Success"] = "Company Updated successfully";
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
         Company CompanyFromDb = _db.Categories.Find(id);
         if (CompanyFromDb == null)
         {
             return NotFound();
         }

         return View(CompanyFromDb);
     }
     [HttpPost, ActionName("Delete")]
     public IActionResult DeletePost(int id)
     {
         Company obj = _db.Categories.Find(id);
         if (obj == null)
         {
             return NotFound();
         }
         *//* Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
          if (obj == null)
          {
              return NotFound();
          }
          _unitOfWork.Company.Remove(obj);
          _unitOfWork.Save();*//*
         _db.Categories.Remove(obj);
         _db.SaveChanges();
         TempData["Success"] = "Company Deleted successfully";
         return RedirectToAction("Index");
     }

 }
}
*/