using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models.db;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.IO;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannersController : Controller
    {
        private readonly OnlineShopContext _context;

        public BannersController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Banners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banners.ToListAsync());
        }

        // GET: Admin/Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Admin/Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageName,Priority,Link,Position")] Banner banner, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    banner.ImageName = Guid.NewGuid() + Path.GetExtension(image.FileName);

                    //ذخیره تصویر اصلی
                    string ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" +
                                                    banner.ImageName);
                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                }


                _context.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Admin/Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageName,Priority,Link,Position")] Banner banner, IFormFile? image)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        //حذف عکس اصلی
                        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" +
                                                     banner.ImageName)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" +
                                                     banner.ImageName));
                        }
                        

                        banner.ImageName = Guid.NewGuid() + Path.GetExtension(image.FileName);

                        //ذخیره تصویر اصلی
                        string ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" +
                                                        banner.ImageName);
                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                    }


                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.Id))
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
            return View(banner);
        }

        // GET: Admin/Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                //حذف عکس اصلی
                if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" +
                                             banner.ImageName)))
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" +
                                             banner.ImageName));
                }

                _context.Banners.Remove(banner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}
