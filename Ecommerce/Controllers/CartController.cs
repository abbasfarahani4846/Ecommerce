using Ecommerce.Models.db;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Newtonsoft.Json.Linq;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        private OnlineShopContext _context;
        public CartController(OnlineShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult AddToCart(int productId, int count)
        //{
        //    if (count == 0)
        //    {
        //        return Redirect("/");
        //    }

        //    var product = _context.Products.FirstOrDefault(x => x.Id == productId);
        //    if (product == null)
        //    {
        //        return Redirect("/");
        //    }

        //    HttpContext.Session.Set<List<SalesCartItemsModel>>("cartItems", itemsList);
        //    var value = HttpContext.Session.Get<List<SalesCartItemsModel>>("cartItems");
        //}
    }
}
