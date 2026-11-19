using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeaderComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultHeaderComponentPartial(PortfolioContext context)
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
