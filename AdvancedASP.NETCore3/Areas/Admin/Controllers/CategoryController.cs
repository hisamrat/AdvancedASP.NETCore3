using AdvancedASP.NETCore3.DataAccess.Data.Repository.IRepository;
using AdvancedASP.NETCore3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedASP.NETCore3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if(id==null)
            {
                return View(category);
            }
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if(category==null)
            {
                return NotFound();
            }
            return View(category);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Id==0)
                {
                    _unitOfWork.Category.Add(category);
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
            
        }











        #region API CALLS


        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _unitOfWork.Category.GetAll();
            return Json(new { data });
        }

        [HttpDelete]

        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Category.Get(id);
            if(objFromDb==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });


        }
        #endregion

    }
}
