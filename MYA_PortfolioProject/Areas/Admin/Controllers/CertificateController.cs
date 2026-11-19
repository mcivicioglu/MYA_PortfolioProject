using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Sertifika belgesi dosya (görsel/PDF) olarak yüklenebilir ya da dış bağlantı olarak girilebilir.
    public class CertificateController : AdminBaseController
    {
        private const string UploadFolder = "certificates";

        public CertificateController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult CertificateList()
        {
            return View(_context.Certificates.OrderByDescending(x => x.CertificateId).ToList());
        }

        [HttpGet]
        public IActionResult CreateCertificate() => View(new Certificate());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCertificate(Certificate model, IFormFile? certificateFile)
        {
            var hasFile = ValidateUpload(certificateFile, FileKinds.ImagesAndDocuments, nameof(certificateFile));
            if (!ModelState.IsValid)
                return View(model);

            if (hasFile)
                model.CertificateUrl = await _files.SaveAsync(certificateFile!, UploadFolder);

            _context.Certificates.Add(model);
            await _context.SaveChangesAsync();
            Success("Sertifika eklendi.");
            return RedirectToAction(nameof(CertificateList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCertificate(int id)
        {
            var value = await _context.Certificates.FindAsync(id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCertificate(Certificate model, IFormFile? certificateFile, bool removeFile)
        {
            var existing = await _context.Certificates.FindAsync(model.CertificateId);
            if (existing == null) return NotFound();

            var hasFile = ValidateUpload(certificateFile, FileKinds.ImagesAndDocuments, nameof(certificateFile));
            if (!ModelState.IsValid)
                return View(model);

            var oldUrl = existing.CertificateUrl;
            existing.Title = model.Title;
            existing.Institution = model.Institution;
            existing.Date = model.Date;

            if (hasFile)
                existing.CertificateUrl = await _files.SaveAsync(certificateFile!, UploadFolder);
            else if (removeFile)
                existing.CertificateUrl = null;
            else
                existing.CertificateUrl = model.CertificateUrl; // dış bağlantı formdan düzenlenebilir

            await _context.SaveChangesAsync();
            if (oldUrl != existing.CertificateUrl)
                _files.Delete(oldUrl); // yalnızca /uploads altındaysa siler

            Success("Sertifika güncellendi.");
            return RedirectToAction(nameof(CertificateList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            var value = await _context.Certificates.FindAsync(id);
            if (value != null)
            {
                var url = value.CertificateUrl;
                _context.Certificates.Remove(value);
                await _context.SaveChangesAsync();
                _files.Delete(url);
                Success("Sertifika silindi.");
            }
            return RedirectToAction(nameof(CertificateList));
        }
    }
}
