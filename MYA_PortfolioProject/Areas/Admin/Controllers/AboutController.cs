using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Hakkımda tek kayıttan oluşur: portre görseli ve özgeçmiş (PDF) dosya olarak yüklenir.
    public class AboutController : AdminBaseController
    {
        private const string UploadFolder = "about";

        public AboutController(PortfolioContext context, IFileService files) : base(context, files) { }

        [HttpGet]
        public IActionResult UpdateAbout()
        {
            return View(_context.Abouts.FirstOrDefault() ?? new About());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAbout(About model, IFormFile? imageFile, bool removeImage, IFormFile? cvFile, bool removeCv)
        {
            var hasImage = ValidateUpload(imageFile, FileKinds.Images, nameof(imageFile));
            var hasCv = ValidateUpload(cvFile, FileKinds.Documents, nameof(cvFile));
            var existing = _context.Abouts.FirstOrDefault();

            if (!ModelState.IsValid)
            {
                model.MainImageUrl = existing?.MainImageUrl;
                model.AboutCVUrl = existing?.AboutCVUrl;
                return View(model);
            }

            if (existing == null)
            {
                existing = new About();
                _context.Abouts.Add(existing);
            }

            existing.NameSurname = model.NameSurname;
            existing.Profession = model.Profession;
            existing.Location = model.Location;
            existing.Title = model.Title;
            existing.Description = model.Description;

            var oldImage = await ApplyUploadAsync(imageFile, hasImage, removeImage, UploadFolder,
                () => existing.MainImageUrl, v => existing.MainImageUrl = v);
            var oldCv = await ApplyUploadAsync(cvFile, hasCv, removeCv, UploadFolder,
                () => existing.AboutCVUrl, v => existing.AboutCVUrl = v);

            await _context.SaveChangesAsync();
            _files.Delete(oldImage);
            _files.Delete(oldCv);

            Success("Hakkımda bilgileri kaydedildi.");
            return RedirectToAction(nameof(UpdateAbout));
        }
    }
}
