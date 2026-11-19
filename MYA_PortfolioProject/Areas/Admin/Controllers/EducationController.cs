using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    public class EducationController : AdminBaseController
    {
        public EducationController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult EducationList()
        {
            // Sitedeki sıralamayla aynı: devam edenler önce, sonra başlangıç yılına göre yeniden eskiye.
            return View(_context.Educations.OrderBy(x => x.EndYear.HasValue).ThenByDescending(x => x.StartYear).ToList());
        }

        [HttpGet]
        public IActionResult CreateEducation() => View(new Education { StartYear = DateTime.Now.Year });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEducation(Education model)
        {
            ValidateYears(model);
            if (!ModelState.IsValid)
                return View(model);

            _context.Educations.Add(model);
            await _context.SaveChangesAsync();
            Success("Eğitim bilgisi eklendi.");
            return RedirectToAction(nameof(EducationList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEducation(int id)
        {
            var value = await _context.Educations.FindAsync(id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEducation(Education model)
        {
            ValidateYears(model);
            if (!ModelState.IsValid)
                return View(model);

            _context.Educations.Update(model);
            await _context.SaveChangesAsync();
            Success("Eğitim bilgisi güncellendi.");
            return RedirectToAction(nameof(EducationList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var value = await _context.Educations.FindAsync(id);
            if (value != null)
            {
                _context.Educations.Remove(value);
                await _context.SaveChangesAsync();
                Success("Eğitim bilgisi silindi.");
            }
            return RedirectToAction(nameof(EducationList));
        }

        private void ValidateYears(Education model)
        {
            if (model.EndYear.HasValue && model.EndYear < model.StartYear)
                ModelState.AddModelError(nameof(Education.EndYear), "Bitiş yılı, başlangıç yılından önce olamaz.");
        }
    }
}
