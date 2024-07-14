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

        public IActionResult SearchProducts(string searchText)
        {
            var products = _context.Products
                .Where(x =>
                    EF.Functions.Like(x.Title, "%" + searchText + "%") ||
                    EF.Functions.Like(x.Tags, "%" + searchText + "%")
                )
                .OrderBy(x => x.Title)
                .ToList();

            return View("Index", products);
        }
        public IActionResult ProductDetails(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["gallery"] = _context.ProductGaleries.Where(x => x.ProductId == id).ToList();
            //if (!string.IsNullOrEmpty(product.Tags))
            //{
            //    var tags = product.Tags.Split(',')
            //                .Select(tag => tag.Trim())
            //                .ToList();

            //    var relatedProducts = _context.Products
            //        .Where(x => x.Id != id && x.Tags != null && tags.Any(tag => x.Tags.Contains(tag)))
            //        .ToList();

            //    ViewData["relatedProducts"] = relatedProducts;
            //}
            return View(product);
        }

        // public IActionResult ProductsByCategory(string? category)
        // {
        //     var products = _context.Products
        //         .Where(x => x.Category == category)
        //         .OrderByDescending(x => x.Id)
        //         .ToList();
        //
        //     return View("Index", products);
        // }
        //public IActionResult Index(string? category)
        //{
        //    var products = _context.Products.OrderByDescending(x => x.Id).AsQueryable();

        //    if (!string.IsNullOrEmpty(category))
        //    {
        //        products = products.Where(x => x.Category.Contains(category));
        //    }

        //    return View(products.ToList());
        //}

    }
}