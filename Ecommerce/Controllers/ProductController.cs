using Ecommerce.Models.db;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly OnlineShopContext _context;
        public ProductController(OnlineShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.OrderByDescending(x => x.Id).ToList();

            return View(products);
        }
        public IActionResult ProductsByCategory(string? category)
        {
            var products = _context.Products
                .Where(x => x.Category == category)
                .OrderByDescending(x => x.Id)
                .ToList();

            return View("Index", products);
        }
        //public IActionResult Index(string? category)
        //{
        //    var products = _context.Products.OrderByDescending(x => x.Id).AsQueryable();

        //    if (!string.IsNullOrEmpty(category))
        //    {
        //        products = products.Where(x => x.Category.Contains(category));
        //    }

        //    return View(products.ToList());
        //}
        //public IActionResult SingleProduct(int productId)
        //{
        //    Product product = _context.Products.FirstOrDefault(product => product.Id == productId);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["gallery"] = _context.ProductGaleries.Where(x => x.ProductId == productId).ToList();
        //    return View(product);
        //}
    }
}
