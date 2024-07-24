﻿using Ecommerce.Models.db;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Text.RegularExpressions;

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

            ViewData["newProducts"] = _context.Products.Where(x => x.Id != id).Take(6).OrderByDescending(x => x.Id).ToList();

            ViewData["comments"] = _context.Comments.Where(x => x.ProductId == id).OrderByDescending(x => x.CreateDate).ToList();
            return View(product);
        }
        [HttpPost]
        public IActionResult SubmitComment(string name, string email, string comment, int productId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(comment) && productId != 0)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    TempData["ErrorMessage"] = "Email is not valid";
                    return RedirectToAction("ProductDetails", new { id = productId });
                }

                Comment newComment = new Comment();
                newComment.Name = name;
                newComment.Email = email;
                newComment.CommentText = comment;
                newComment.ProductId = productId;
                newComment.CreateDate = DateTime.Now;

                _context.Comments.Add(newComment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Youre comment submited success fully";
                return RedirectToAction("ProductDetails", new { id = productId });
            }

            TempData["ErrorMessage"] = "Please complete youre information";
            return RedirectToAction("ProductDetails", new { id = productId });
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