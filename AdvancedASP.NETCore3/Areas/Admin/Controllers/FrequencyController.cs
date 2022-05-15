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
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if(id==null)
            {
                return View(frequency);

            }
            frequency = _unitOfWork.Frequency.Get(id.GetValueOrDefault());
            if(frequency==null)
            {
                return NotFound();
            }
            return View(frequency);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if(ModelState.IsValid)
            {
                if(frequency.Id==0)
                {
                    _unitOfWork.Frequency.Add(frequency);
                }
                else
                {
                    _unitOfWork.Frequency.Update(frequency);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _unitOfWork.Frequency.GetAll();
            return Json(new { data });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Frequency.Get(id);
            if(objFromDb==null)
            {
                return Json(new { success = false, message = "Error while delete" });
            }
            _unitOfWork.Frequency.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
