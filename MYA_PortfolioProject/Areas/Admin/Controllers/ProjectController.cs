using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    public class ProjectController : AdminBaseController
    {
        private const string UploadFolder = "projects";

        public ProjectController(PortfolioContext context, IFileService files) : base(context, files) { }

        // Admin listesi aktif ve pasif tüm projeleri gösterir.
        public IActionResult ProjectList(int? categoryId, string sort = "newest")
        {
            var query = _context.Projects.Include(x => x.Category).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            query = sort switch
            {
                "oldest" => query.OrderBy(x => x.CreatedDate).ThenBy(x => x.ProjectId),
                "title" => query.OrderBy(x => x.ProjectTitle),
                _ => query.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.ProjectId)
            };

            FillCategories(categoryId);
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedSort = sort;
            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            if (!_context.Categories.Any())
            {
                Error("Proje eklemeden önce en az bir kategori oluşturun.");
                return RedirectToAction("CategoryList", "Category");
            }

            FillCategories();
            return View(new Project());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(Project model, IFormFile? imageFile)
        {
            var hasImage = ValidateUpload(imageFile, FileKinds.Images, nameof(imageFile));
            ValidateCategory(model.CategoryId);

            if (!ModelState.IsValid)
            {
                FillCategories(model.CategoryId);
                return View(model);
            }

            if (hasImage)
                model.ImageUrl = await _files.SaveAsync(imageFile!, UploadFolder);
            model.CreatedDate = DateTime.Now;

            _context.Projects.Add(model);
            await _context.SaveChangesAsync();
            Success("Proje eklendi.");
            return RedirectToAction(nameof(ProjectList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var value = await _context.Projects.FindAsync(id);
            if (value == null) return NotFound();

            FillCategories(value.CategoryId);
            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(Project model, IFormFile? imageFile, bool removeImage)
        {
            var existing = await _context.Projects.FindAsync(model.ProjectId);
            if (existing == null) return NotFound();

            var hasImage = ValidateUpload(imageFile, FileKinds.Images, nameof(imageFile));
            ValidateCategory(model.CategoryId);

            if (!ModelState.IsValid)
            {
                model.ImageUrl = existing.ImageUrl; // formdaki mevcut görsel önizlemesi kaybolmasın
                FillCategories(model.CategoryId);
                return View(model);
            }

            // CreatedDate gibi formda olmayan alanlar ezilmesin diye alanlar tek tek kopyalanır.
            existing.ProjectTitle = model.ProjectTitle;
            existing.Description = model.Description;
            existing.Technology = model.Technology;
            existing.ProjectUrl = model.ProjectUrl;
            existing.CategoryId = model.CategoryId;
            existing.IsActive = model.IsActive;

            var oldImage = await ApplyUploadAsync(imageFile, hasImage, removeImage, UploadFolder,
                () => existing.ImageUrl, v => existing.ImageUrl = v);

            await _context.SaveChangesAsync();
            _files.Delete(oldImage);

            Success("Proje güncellendi.");
            return RedirectToAction(nameof(ProjectList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleProjectStatus(int id)
        {
            var value = await _context.Projects.FindAsync(id);
            if (value == null) return NotFound();

            value.IsActive = !value.IsActive;
            await _context.SaveChangesAsync();
            Success(value.IsActive ? "Proje sitede listelenecek." : "Proje sitede gizlendi.");
            return RedirectToAction(nameof(ProjectList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var value = await _context.Projects.FindAsync(id);
            if (value != null)
            {
                var image = value.ImageUrl;
                _context.Projects.Remove(value);
                await _context.SaveChangesAsync();
                _files.Delete(image);
                Success("Proje silindi.");
            }
            return RedirectToAction(nameof(ProjectList));
        }

        private void FillCategories(int? selectedId = null)
        {
            ViewBag.Categories = new SelectList(_context.Categories.OrderBy(x => x.CategoryName).ToList(),
                nameof(Category.CategoryId), nameof(Category.CategoryName), selectedId);
        }

        // Listede olmayan bir id gönderilirse veritabanı hatası yerine form hatası gösterilir.
        private void ValidateCategory(int categoryId)
        {
            if (categoryId > 0 && !_context.Categories.Any(x => x.CategoryId == categoryId))
                ModelState.AddModelError(nameof(Project.CategoryId), "Seçilen kategori bulunamadı.");
        }
    }
}
