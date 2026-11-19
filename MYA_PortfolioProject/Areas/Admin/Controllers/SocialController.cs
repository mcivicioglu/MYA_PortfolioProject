using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Hero, iletişim ve footer'da gösterilen sosyal medya bağlantıları.
    public class SocialController : AdminBaseController
    {
        public SocialController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult SocialList()
        {
            return View(_context.SocialLinks.OrderBy(x => x.SocialLinkId).ToList());
        }

        [HttpGet]
        public IActionResult CreateSocial() => View(new SocialLink());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSocial(SocialLink model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Icon = NormalizeIcon(model.Icon);
            _context.SocialLinks.Add(model);
            await _context.SaveChangesAsync();
            Success("Bağlantı eklendi.");
            return RedirectToAction(nameof(SocialList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSocial(int id)
        {
            var value = await _context.SocialLinks.FindAsync(id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSocial(SocialLink model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Icon = NormalizeIcon(model.Icon);
            _context.SocialLinks.Update(model);
            await _context.SaveChangesAsync();
            Success("Bağlantı güncellendi.");
            return RedirectToAction(nameof(SocialList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSocial(int id)
        {
            var value = await _context.SocialLinks.FindAsync(id);
            if (value != null)
            {
                _context.SocialLinks.Remove(value);
                await _context.SaveChangesAsync();
                Success("Bağlantı silindi.");
            }
            return RedirectToAction(nameof(SocialList));
        }

        // "bi bi-github", "bi-github" veya "github" girişlerini "bi-github" olarak saklar.
        private static string NormalizeIcon(string icon)
        {
            var clean = (icon ?? string.Empty).Replace("bi bi-", "").Replace("bi-", "").Replace("bi ", "").Trim();
            return string.IsNullOrEmpty(clean) ? "bi-link-45deg" : "bi-" + clean;
        }
    }
}
