using Ecommerce.Models.db;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

using System.Security.Claims;
using System.Text.RegularExpressions;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

using System.Net.Mail;
using System.Net;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private OnlineShopContext _context;
        public AccountController(OnlineShopContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(user);
            }

            user.Email = user.Email.Trim();
            var prevUser = _context.Users.Any(x => x.Email == user.Email);
            if (prevUser)
            {
                ModelState.AddModelError("Email", "Email is used");
                return View(user);
            }

            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Password = user.Password.Trim();
            user.FullName = user.FullName.Trim();
            user.RecoveryCode = 0;

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var findUser = _context.Users.FirstOrDefault(x => x.Email == user.Email.Trim() && x.Password == user.Password.Trim());
            if (findUser == null)
            {
                ModelState.AddModelError("Email", "User not exists");
                return View(user);
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, findUser.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, findUser.FullName));
            claims.Add(new Claim(ClaimTypes.Email, findUser.Email));
            claims.Add(new Claim(ClaimTypes.Role, findUser.IsAdmin ? "admin" : "user"));

            // Create an identity based on the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create a principal based on the identity
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user with the created principal
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            return Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult RecoveryPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecoveryPassword(RecoveryPasswordViewModel recoveryPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(recoveryPassword.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(recoveryPassword);
            }

            var findUser = _context.Users.FirstOrDefault(x => x.Email == recoveryPassword.Email.Trim());
            if (findUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View(recoveryPassword);
            }

            findUser.RecoveryCode = new Random().Next(10000, 100000);
            _context.Users.Update(findUser);
            _context.SaveChanges();

            // Set up the email details
            string to = recoveryPassword.Email; // To address
            string from = "emailsendertest0055@gmail.com"; // From address
            string fromPass = "fflf cwva cbmn bpgb";
            string subject = "Recovery code";
            string body = "youre recovery code:" + findUser.RecoveryCode;

            // Create a new MailMessage object
            MailMessage mail = new MailMessage(from, to, subject, body);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, fromPass)
            };

            using (var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            }
            )
            {
                smtp.Send(message);
            }

            var ressetPasswordModel = new RessetPasswordViewModel();
            ressetPasswordModel.Email = findUser.Email;

            return View("RessetPassword", ressetPasswordModel);
        }
      
        [HttpPost]
        public IActionResult RessetPassword(RessetPasswordViewModel ressetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(ressetPassword);
            }

            var findUser = _context.Users.FirstOrDefault(x => x.Email == ressetPassword.Email && x.RecoveryCode == ressetPassword.RecoveryCode);
            if (findUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View(ressetPassword);
            }

            findUser.Password = ressetPassword.NewPassword;

            _context.Users.Update(findUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
