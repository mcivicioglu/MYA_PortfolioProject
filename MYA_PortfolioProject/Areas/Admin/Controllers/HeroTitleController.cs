using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Hero'daki yazı animasyonunda sırayla gösterilen unvanlar.
    public class HeroTitleController : AdminBaseController
    {
        public HeroTitleController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult HeroTitleList()
        {
            return View(_context.HeroTitles.OrderBy(x => x.Order).ThenBy(x => x.HeroTitleId).ToList());
        }

        [HttpGet]
        public IActionResult CreateHeroTitle()
        {
            var nextOrder = (_context.HeroTitles.Max(x => (int?)x.Order) ?? 0) + 1;
            return View(new HeroTitle { Order = Math.Min(nextOrder, 100), IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHeroTitle(HeroTitle model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.HeroTitles.Add(model);
            await _context.SaveChangesAsync();
            Success("Unvan eklendi.");
            return RedirectToAction(nameof(HeroTitleList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateHeroTitle(int id)
        {
            var value = await _context.HeroTitles.FindAsync(id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHeroTitle(HeroTitle model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.HeroTitles.Update(model);
            await _context.SaveChangesAsync();
            Success("Unvan güncellendi.");
            return RedirectToAction(nameof(HeroTitleList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleHeroTitleStatus(int id)
        {
            var value = await _context.HeroTitles.FindAsync(id);
            if (value == null) return NotFound();

            value.IsActive = !value.IsActive;
            await _context.SaveChangesAsync();
            Success(value.IsActive ? "Unvan animasyona eklendi." : "Unvan animasyondan çıkarıldı.");
            return RedirectToAction(nameof(HeroTitleList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHeroTitle(int id)
        {
            var value = await _context.HeroTitles.FindAsync(id);
            if (value != null)
            {
                _context.HeroTitles.Remove(value);
                await _context.SaveChangesAsync();
                Success("Unvan silindi.");
            }
            return RedirectToAction(nameof(HeroTitleList));
        }
    }
}
