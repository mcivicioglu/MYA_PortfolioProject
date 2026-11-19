using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Hero tek kayıttan oluşur: kayıt yoksa ilk kaydetmede oluşturulur.
    public class HeroController : AdminBaseController
    {
        private const string UploadFolder = "hero";

        public HeroController(PortfolioContext context, IFileService files) : base(context, files) { }

        [HttpGet]
        public IActionResult UpdateHero()
        {
            return View(_context.HeroSections.FirstOrDefault() ?? new HeroSection());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHero(HeroSection model, IFormFile? imageFile, bool removeImage)
        {
            var hasFile = ValidateUpload(imageFile, FileKinds.Images, nameof(imageFile));
            var existing = _context.HeroSections.FirstOrDefault();

            if (!ModelState.IsValid)
            {
                model.ImageURL = existing?.ImageURL;
                return View(model);
            }

            if (existing == null)
            {
                existing = new HeroSection();
                _context.HeroSections.Add(existing);
            }

            existing.Greeting = model.Greeting;
            existing.Title = model.Title;
            existing.Description = model.Description;
            var oldFile = await ApplyUploadAsync(imageFile, hasFile, removeImage, UploadFolder,
                () => existing.ImageURL, v => existing.ImageURL = v);

            await _context.SaveChangesAsync();
            _files.Delete(oldFile);

            Success("Hero bölümü kaydedildi.");
            return RedirectToAction(nameof(UpdateHero));
        }
    }
}
