using AdvancedASP.NETCore3.DataAccess.Data.Repository.IRepository;
using AdvancedASP.NETCore3.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedASP.NETCore3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ServiceController(IUnitOfWork unitOfWork,IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM serviceVM = new ServiceVM()
            {

                Service = new Models.Service(),
                CategoryList=_unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList=_unitOfWork.Frequency.GetFrequencyListForDropDown(),

            
            };
            if(id!=null)
            {
                serviceVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }
            return View(serviceVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceVM serviceVM)
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(serviceVM.Service.Id==0)
                {
                    //new service
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);
                    using(var filestream=new FileStream(Path.Combine(uploads,filename+extension),FileMode.Create))
                    {
                        files[0].CopyTo(filestream);

                    }
                    serviceVM.Service.ImageUrl = @"\images\services\" + filename + extension;
                    _unitOfWork.Service.Add(serviceVM.Service);
                }
                else
                {
                    //edit serivce
                    var serviceFromDb = _unitOfWork.Service.Get(serviceVM.Service.Id);
                    if(files.Count>0)
                    {
                        string filename = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);
                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart
                            ('\\')
                            );
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var filestream = new FileStream(Path.Combine(uploads, filename + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);

                        }
                        serviceVM.Service.ImageUrl = @"\images\services\" + filename + extension_new;
                    }
                    else
                    {
                        serviceVM.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }
                    _unitOfWork.Service.Update(serviceVM.Service);

                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                serviceVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                serviceVM.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();

                return View(serviceVM);
            }
        }

        [HttpDelete]

        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart
                            ('\\')
                            );
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while delete" });
            }
            _unitOfWork.Service.Remove(serviceFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });

        }

        #region API Calls

        public IActionResult GetAll()
        {
            var data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency");

            return Json(new { data });
        }

        #endregion

    }
}
