using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Yetenek grupları (Skill) ve her grubun altındaki teknolojiler (SkillItem) aynı controller'da yönetilir.
    public class SkillController : AdminBaseController
    {
        public SkillController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult SkillList()
        {
            var values = _context.Skills.Include(x => x.SkillItems)
                .OrderByDescending(x => x.SkillValue).ThenBy(x => x.Title)
                .ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSkill() => View(new Skill { IconURL = "bi-code-slash", SkillValue = 50 });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSkill(Skill model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IconURL = NormalizeIcon(model.IconURL);
            _context.Skills.Add(model);
            await _context.SaveChangesAsync();

            Success("Yetenek grubu eklendi. Şimdi altına teknolojileri ekleyebilirsiniz.");
            return RedirectToAction(nameof(UpdateSkill), new { id = model.SkillId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSkill(int id)
        {
            var value = await _context.Skills.Include(x => x.SkillItems).FirstOrDefaultAsync(x => x.SkillId == id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSkill(Skill model)
        {
            var existing = await _context.Skills.Include(x => x.SkillItems).FirstOrDefaultAsync(x => x.SkillId == model.SkillId);
            if (existing == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.SkillItems = existing.SkillItems;
                return View(model);
            }

            existing.Title = model.Title;
            existing.Description = model.Description;
            existing.IconURL = NormalizeIcon(model.IconURL);
            existing.SkillValue = model.SkillValue;
            await _context.SaveChangesAsync();

            Success("Yetenek grubu güncellendi.");
            return RedirectToAction(nameof(UpdateSkill), new { id = model.SkillId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var value = await _context.Skills.FindAsync(id);
            if (value != null)
            {
                _context.Skills.Remove(value); // alt teknolojiler veritabanında kademeli olarak silinir
                await _context.SaveChangesAsync();
                Success("Yetenek grubu ve teknolojileri silindi.");
            }
            return RedirectToAction(nameof(SkillList));
        }

        // ---------- SkillItem ----------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSkillItem(int skillId, string skillItemTitle)
        {
            var title = skillItemTitle?.Trim();
            if (string.IsNullOrEmpty(title) || title.Length > 100)
                Error("Teknoloji adı 1 ile 100 karakter arasında olmalıdır.");
            else if (!_context.Skills.Any(x => x.SkillId == skillId))
                return NotFound();
            else
            {
                _context.SkillItems.Add(new SkillItem { SkillId = skillId, SkillItemTitle = title });
                await _context.SaveChangesAsync();
                Success($"“{title}” eklendi.");
            }
            return RedirectToAction(nameof(UpdateSkill), new { id = skillId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSkillItem(int id, string skillItemTitle)
        {
            var value = await _context.SkillItems.FindAsync(id);
            if (value == null) return NotFound();

            var title = skillItemTitle?.Trim();
            if (string.IsNullOrEmpty(title) || title.Length > 100)
                Error("Teknoloji adı 1 ile 100 karakter arasında olmalıdır.");
            else
            {
                value.SkillItemTitle = title;
                await _context.SaveChangesAsync();
                Success("Teknoloji güncellendi.");
            }
            return RedirectToAction(nameof(UpdateSkill), new { id = value.SkillId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkillItem(int id)
        {
            var value = await _context.SkillItems.FindAsync(id);
            if (value == null) return NotFound();

            _context.SkillItems.Remove(value);
            await _context.SaveChangesAsync();
            Success("Teknoloji silindi.");
            return RedirectToAction(nameof(UpdateSkill), new { id = value.SkillId });
        }

        // "bi bi-braces", "bi-braces" veya "braces" girişlerini "bi-braces" olarak saklar.
        private static string? NormalizeIcon(string? icon)
        {
            var clean = (icon ?? string.Empty).Replace("bi bi-", "").Replace("bi-", "").Replace("bi ", "").Trim();
            return string.IsNullOrEmpty(clean) ? null : "bi-" + clean;
        }
    }
}
