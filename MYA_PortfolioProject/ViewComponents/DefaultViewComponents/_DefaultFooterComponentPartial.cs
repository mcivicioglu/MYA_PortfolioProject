using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultFooterComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultFooterComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.NameSurname = _context.Abouts.Select(x => x.NameSurname).FirstOrDefault() ?? "Mahmut Çivicioğlu";
            return View();
        }
    }
}
