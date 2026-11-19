using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Sitedeki iletişim bölümünün metinleri ve iletişim bilgileri (tek kayıt).
    public class ContactController : AdminBaseController
    {
        public ContactController(PortfolioContext context, IFileService files) : base(context, files) { }

        [HttpGet]
        public IActionResult UpdateContact()
        {
            return View(_context.Contacts.FirstOrDefault() ?? new Contact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContact(Contact model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = _context.Contacts.FirstOrDefault();
            if (existing == null)
            {
                existing = new Contact();
                _context.Contacts.Add(existing);
            }

            existing.Title = model.Title;
            existing.Description = model.Description;
            existing.Email = model.Email;
            existing.PhoneNumber = model.PhoneNumber;
            existing.Location = model.Location;

            await _context.SaveChangesAsync();
            Success("İletişim bilgileri kaydedildi.");
            return RedirectToAction(nameof(UpdateContact));
        }
    }
}
