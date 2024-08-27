using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models.db;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SettingsController : Controller
    {
        private readonly OnlineShopContext _context;

        public SettingsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Settings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Settings.FirstAsync());
        }

        // GET: Admin/Settings/Edit/5
        public async Task<IActionResult> Edit()
        {

            var setting = await _context.Settings.FirstAsync();
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        // POST: Admin/Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Shipping,Title,Address,Phone,CopyRight,Instagram,FaceBook,GooglePlus,Youtube,Twitter,Logo")] Setting setting,IFormFile? newLogo)
        {
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newLogo != null)
                    {
                        setting.Logo = Guid.NewGuid() + Path.GetExtension(newLogo.FileName);
                        //------------------------------------------------
                        string d = Directory.GetCurrentDirectory();
                        string fn = d + "\\wwwroot\\images\\" + setting.Logo;
                        //------------------------------------------------

                        using (var stream = new FileStream(fn, FileMode.Create))
                        {
                            newLogo.CopyTo(stream);
                        }

                    }

                    _context.Update(setting);
                    await _context.SaveChangesAsync();

                    ViewData["message"] = "Setting saved";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(setting);
        }

    
        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
