
using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultCertificateComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultCertificateComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Certificates
                .OrderByDescending(x => x.CertificateId)
                .ToList();

            return View(values);
        }
    }
}