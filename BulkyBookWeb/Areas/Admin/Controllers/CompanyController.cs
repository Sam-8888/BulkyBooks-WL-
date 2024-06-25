using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public CompanyController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        //GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
           
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitofWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }
           
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {

            if (ModelState.IsValid)
            {
                
                if (obj.Id == 0)
                {
                    _unitofWork.Company.Add(obj);
                    TempData["success"] = "Company created succesfully!";
                }
                else 
                {
                    _unitofWork.Company.Update(obj);
                    TempData["success"] = "Company updated succesfully!";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
       

        #region APICalls

        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitofWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var company = _unitofWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (company == null)
            {
                return Json(new { success = false,message="Error in deleting" });
            }
            _unitofWork.Company.Remove(company);
            _unitofWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
