using Ecommerce.Models.db;
using Ecommerce.Models.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Newtonsoft.Json;
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
        /// <summary>
        /// افزودن یا بروزرسانی سبد خرید
        /// </summary>
        /// <param name="productId">ایدی محصول</param>
        /// <param name="count">تعداد مورد نظر. اگر صفر باشه یعنی قصد حذف رو داره. این مورد دستی توسط خودمون کنترل میشه</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateCart(int productId, int count)
        {
            if (count == 0)
            {
                return Redirect("/");
            }

            var product = _context.Products.FirstOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return Redirect("/");
            }
            //گرفتن لیست محصولات در سبد از تابع مخصوص اینکار
            var cartItems = GetCartItems();

            var foundProductInCart = cartItems.FirstOrDefault(x => x.ProductId == productId);

            //اگر محصول پیدا بشه یعنی تو سبدش هست و قصد داره تعداد  خرید رو تغییر بده
            if (foundProductInCart != null)
            {
                // اگر بزرگتر از صفر باشه یعنی قصد بروز رسانی تعداد رو در غیر اینصورت از سبد حذف میشه.
                if (count > 0)
                {
                    foundProductInCart.Count = count;
                }
                else
                {
                    cartItems.Remove(foundProductInCart);
                }

            }
            else
            {
                var newCartItem = new CartViewModel() { };
                newCartItem.ProductId = productId;
                newCartItem.Count = count;

                cartItems.Add(newCartItem);
            }


            return new JsonResult(cartItems.Count());
        }
        public IActionResult GetSmallCart()
        {
            var cartItems = GetCartItems();

            if (cartItems.Any() == false)
            {
                return View();
            }

            var cartItemProductIds = cartItems.Select(x => x.ProductId);

            var products = _context.Products.Where(p => cartItemProductIds.Contains(p.Id)).Select(ps => new ProductCartViewModel
            {
                Id = ps.Id,
                ImageName = ps.ImageName,
                Price = ps.Price - ps.Discount ?? 0,
                Title = ps.Title,
                Count = cartItems.Single(x => x.ProductId == ps.Id).Count,
                RowSumPrice = (ps.Price - ps.Discount ?? 0) * cartItems.Single(x => x.ProductId == ps.Id).Count,
            }).ToList();

            return PartialView("_SmallCartPartialView", products);
        }

        public List<CartViewModel> GetCartItems()
        {
            List<CartViewModel> cartList = new List<CartViewModel>();

            var prevCartItemsString = Request.Cookies["Cart"];

            //اگر نال نباشه یعنی سبدش خالی نیست و باید تبدیلش کنیم به لیستی از ویو مدل در غیر اینصورت لیست خالی سبد بر میگردونیم
            if (!string.IsNullOrEmpty(prevCartItemsString))
            {
                cartList = JsonConvert.DeserializeObject<List<CartViewModel>>(prevCartItemsString);
            }

            return cartList;
        }
    }
}
