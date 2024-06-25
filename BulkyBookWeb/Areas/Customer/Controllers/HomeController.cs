using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _unitofWork;
        public HomeController(ILogger<HomeController> logger, IUnitofWork unitOfWork)
        {
            _logger = logger;
            _unitofWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitofWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(productList);
        }

        public IActionResult Details(int productid)
        {
            ShoppingCart cartobj = new()
            {
                Count = 1,
                ProductId = productid,
                Product = _unitofWork.Product.GetFirstOrDefault(u => u.Id == productid, includeProperties: "Category,CoverType")
            };
            return View(cartobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCart cart = _unitofWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == shoppingCart.ApplicationUserId &&
            u.ProductId == shoppingCart.ProductId);

            if (cart == null)
            {
                _unitofWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitofWork.ShoppingCart.IncrementCount(cart, shoppingCart.Count);
            }
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}