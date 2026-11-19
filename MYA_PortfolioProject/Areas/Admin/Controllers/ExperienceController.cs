using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    public class ExperienceController : AdminBaseController
    {
        public ExperienceController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult ExperienceList()
        {
            return View(_context.Experiences.OrderByDescending(x => x.IsActive).ThenBy(x => x.ExperienceId).ToList());
        }

        [HttpGet]
        public IActionResult CreateExperience() => View(new Experience { IsActive = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExperience(Experience model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Experiences.Add(model);
            await _context.SaveChangesAsync();
            Success("Deneyim eklendi.");
            return RedirectToAction(nameof(ExperienceList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateExperience(int id)
        {
            var value = await _context.Experiences.FindAsync(id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateExperience(Experience model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Experiences.Update(model);
            await _context.SaveChangesAsync();
            Success("Deneyim güncellendi.");
            return RedirectToAction(nameof(ExperienceList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleExperienceStatus(int id)
        {
            var value = await _context.Experiences.FindAsync(id);
            if (value == null) return NotFound();

            value.IsActive = !value.IsActive;
            await _context.SaveChangesAsync();
            Success(value.IsActive ? "Deneyim sitede gösterilecek." : "Deneyim sitede gizlendi.");
            return RedirectToAction(nameof(ExperienceList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var value = await _context.Experiences.FindAsync(id);
            if (value != null)
            {
                _context.Experiences.Remove(value);
                await _context.SaveChangesAsync();
                Success("Deneyim silindi.");
            }
            return RedirectToAction(nameof(ExperienceList));
        }
    }
}
