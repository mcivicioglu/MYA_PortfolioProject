using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYA_PortfolioProject.Context;


namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultSkillComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultSkillComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Skills.Include(x => x.SkillItems).OrderByDescending(x => x.SkillValue).ThenBy(x => x.Title).ToList();
            return View(values);
        }
    }
}
