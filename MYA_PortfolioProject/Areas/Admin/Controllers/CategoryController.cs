using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Proje kategorileri. Ekleme formu listenin içindedir.
    public class CategoryController : AdminBaseController
    {
        public CategoryController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult CategoryList()
        {
            FillProjectCounts();
            return View(_context.Categories.OrderBy(x => x.CategoryName).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            ValidateUnique(model);
            if (!ModelState.IsValid)
            {
                FillProjectCounts();
                ViewBag.NewCategory = model;
                return View(nameof(CategoryList), _context.Categories.OrderBy(x => x.CategoryName).ToList());
            }

            model.CategoryName = model.CategoryName.Trim();
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            Success("Kategori eklendi.");
            return RedirectToAction(nameof(CategoryList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var value = await _context.Categories.FindAsync(id);
            return value == null ? NotFound() : View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(Category model)
        {
            ValidateUnique(model);
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _context.Categories.FindAsync(model.CategoryId);
            if (existing == null) return NotFound();

            existing.CategoryName = model.CategoryName.Trim();
            await _context.SaveChangesAsync();
            Success("Kategori güncellendi.");
            return RedirectToAction(nameof(CategoryList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var value = await _context.Categories.FindAsync(id);
            if (value == null)
                return RedirectToAction(nameof(CategoryList));

            // Veritabanında silme kademeli (cascade); kategoriyle birlikte projeler de silinmesin diye engellenir.
            var projectCount = _context.Projects.Count(x => x.CategoryId == id);
            if (projectCount > 0)
            {
                Error($"“{value.CategoryName}” kategorisinde {projectCount} proje var. Önce projeleri başka kategoriye taşıyın.");
                return RedirectToAction(nameof(CategoryList));
            }

            _context.Categories.Remove(value);
            await _context.SaveChangesAsync();
            Success("Kategori silindi.");
            return RedirectToAction(nameof(CategoryList));
        }

        private void FillProjectCounts()
        {
            ViewBag.ProjectCounts = _context.Projects
                .GroupBy(x => x.CategoryId)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionary(x => x.Key, x => x.Count);
        }

        private void ValidateUnique(Category model)
        {
            var name = model.CategoryName?.Trim();
            if (!string.IsNullOrEmpty(name) && _context.Categories.Any(x => x.CategoryName == name && x.CategoryId != model.CategoryId))
                ModelState.AddModelError(nameof(Category.CategoryName), "Bu adda bir kategori zaten var.");
        }
    }
}
