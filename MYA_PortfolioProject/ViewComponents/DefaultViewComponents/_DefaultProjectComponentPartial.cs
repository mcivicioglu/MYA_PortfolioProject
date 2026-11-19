using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultProjectComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultProjectComponentPartial(PortfolioContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            int? categoryId = int.TryParse(HttpContext.Request.Query["category"], out var parsed) ? parsed : null;
            var sort = HttpContext.Request.Query["sort"].ToString();


            var query = _context.Projects.Include(x => x.Category).Where(x => x.IsActive);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            query = sort switch
            {
                "oldest" => query.OrderBy(x => x.CreatedDate).ThenBy(x => x.ProjectId),
                "title" => query.OrderBy(x => x.ProjectTitle),
                _ => query.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.ProjectId)
            };

            ViewBag.Categories = _context.Categories
                .Where(c => c.Projects.Any(p => p.IsActive))
                .OrderBy(c => c.CategoryName)
                .ToList();
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedSort = sort is "oldest" or "title" ? sort : "newest";

            return View(query.ToList());
        }
    }
}
