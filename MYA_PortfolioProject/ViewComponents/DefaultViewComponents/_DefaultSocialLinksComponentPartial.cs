using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultSocialLinksComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultSocialLinksComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        /// <param name="style">"icons": yuvarlak ikon listesi (hero, footer) · "list": iletişim listesindeki li öğeleri</param>
        public IViewComponentResult Invoke(string style = "icons")
        {
            ViewBag.Style = style;
            var values = _context.SocialLinks.OrderBy(x => x.SocialLinkId).ToList();
            return View(values);
        }
    }
}
