using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _hostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }


        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitofWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() }),
                CoverTypeList = _unitofWork.CoverType.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() })
            };
            if (id == null || id == 0)
            {
                //Create product
                //ViewBag.CatList = CatList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitofWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
           // var category = _db.Categories.Find(id);
           
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwrootpath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootpath, @"Images\Products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldpath = Path.Combine(wwwrootpath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldpath))
                        {
                            System.IO.File.Delete(oldpath);
                        }
                    }

                    using(var FS = new FileStream(Path.Combine(uploads, fileName+ extension),FileMode.Create))
                    {
                        file.CopyTo(FS);
                    }
                    obj.Product.ImageUrl = @"\Images\Products\" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _unitofWork.Product.Add(obj.Product);
                }
                else 
                {
                    _unitofWork.Product.Update(obj.Product);
                }
                _unitofWork.Save();
                TempData["success"] = "Product created succesfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
       

        #region APICalls

        [HttpGet]
        public IActionResult GetAll()
        {
            var productsList = _unitofWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productsList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitofWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (product == null)
            {
                return Json(new { success = false,message="Error in deleting" });
            }
            var oldpath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldpath))
            {
                System.IO.File.Delete(oldpath);
            }
            _unitofWork.Product.Remove(product);
            _unitofWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
