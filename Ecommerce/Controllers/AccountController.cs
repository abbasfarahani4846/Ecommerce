using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
