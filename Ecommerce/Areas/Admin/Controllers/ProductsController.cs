using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models.db;
using System.Reflection;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult DeleteGallery(int id)
        {
            var gallery = _context.ProductGaleries.FirstOrDefault(x => x.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                  gallery.ImageName)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                         gallery.ImageName));
            }
            _context.Remove(gallery);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = gallery.ProductId });
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Text,Price,Discount,ImageName,Qty,Tags")] Product product, IFormFile? image, IFormFile[]? gallery)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageName = Guid.NewGuid() + Path.GetExtension(image.FileName);

                    //ذخیره تصویر اصلی
                    string ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                    product.ImageName);
                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                }

                _context.Add(product);
                await _context.SaveChangesAsync();

                if (gallery != null)
                {
                    foreach (var item in gallery)
                    {
                        var newGallery = new ProductGalery();
                        newGallery.ProductId = product.Id;

                        newGallery.ImageName = Guid.NewGuid() + Path.GetExtension(item.FileName);

                        //ذخیره تصویر اصلی
                        string ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                        newGallery.ImageName);
                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }


                        _context.ProductGaleries.Add(newGallery);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["gallery"] = _context.ProductGaleries.Where(x => x.ProductId == product.Id).ToList();
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Text,Price,Discount,ImageName,Qty,Tags")] Product product, IFormFile? image, IFormFile[]? gallery)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                   product.ImageName)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                     product.ImageName));
                        }

                        product.ImageName = Guid.NewGuid() + Path.GetExtension(image.FileName);

                        //ذخیره تصویر اصلی
                        string ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                        product.ImageName);
                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                    }


                    if (gallery != null)
                    {
                        foreach (var item in gallery)
                        {

                            var imgName = Guid.NewGuid() + Path.GetExtension(item.FileName);

                            //ذخیره تصویر اصلی
                            string ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                            imgName);
                            using (var stream = new FileStream(ImagePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }

                            _context.ProductGaleries.Add(new ProductGalery
                            {
                                ImageName = imgName,
                                ProductId = product.Id
                            });
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                product.ImageName)))
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                             product.ImageName));
                }

                var galleries = _context.ProductGaleries.Where(x => x.ProductId == id).ToList();
                if (galleries != null)
                {
                    foreach (var item in galleries)
                    {
                        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                               item.ImageName)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/products/" +
                                                     item.ImageName));
                        }
                    }

                    _context.ProductGaleries.RemoveRange(galleries);
                }

                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
