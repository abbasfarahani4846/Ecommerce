using Ecommerce.Models.db;

using Microsoft.AspNetCore.Mvc;

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
            List<Product> products = _context.Products.OrderByDescending(x => x.Id).ToList();
            return View(products);
        }
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
