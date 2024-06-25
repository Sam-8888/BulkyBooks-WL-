using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]  
    public class CoverTypeController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public CoverTypeController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            //var catList = _db.Categories.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() });
            IEnumerable<CoverType> objCoverTypeList = _unitofWork.CoverType.GetAll();
            //var objCategoryList = _db.Categories.ToList();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "Cover type created succesfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
           // var category = _db.Categories.Find(id);
           var coverType = _unitofWork.CoverType.GetFirstOrDefault(u => u.Id == id);//if found more than one, will return the first one
            //_db.Categories.SingleOrDefault(u => u.Id = id);//will cause an exception of more than one is found
            if(coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "Cover type updated succesfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverType = _unitofWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var coverType = _unitofWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            _unitofWork.CoverType.Remove(coverType);
            _unitofWork.Save();
            TempData["success"] = "Cover type deleted succesfully!";
            return RedirectToAction("Index");

        }
    }
}
