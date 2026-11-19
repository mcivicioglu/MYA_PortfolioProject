using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeroSectionComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultHeroSectionComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var heroSection = _context.HeroSections.FirstOrDefault();
            ViewBag.greeting = heroSection?.Greeting ?? "Merhaba, ben Mahmut";
            ViewBag.title = heroSection?.Title ?? "Fikirleri Dijital Deneyimlere Dönüştürüyorum.";
            ViewBag.description = heroSection?.Description ?? "Modern yazılım teknolojileriyle yenilikçi çözümler geliştiriyor, bilgi ve deneyimlerimi paylaşarak teknoloji dünyasına değer katıyorum.";
            ViewBag.imageUrl = heroSection?.ImageURL;

            var nameSurname = _context.Abouts.Select(x => x.NameSurname).FirstOrDefault() ?? "Mahmut Çivicioğlu";
            ViewBag.nameSurname = nameSurname;
            ViewBag.initials = string.Concat(nameSurname
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .Select(x => char.ToUpper(x[0], new System.Globalization.CultureInfo("tr-TR"))));

            var phrases = _context.HeroTitles.Where(x => x.IsActive).OrderBy(x => x.Order).Select(x => x.Title).ToList();
            ViewBag.phrases = phrases.Count > 0 ? phrases : new List<string> { "Yazılım Geliştirici", "Web Tasarımcı", "Teknoloji Meraklısı" };
            return View(heroSection);
        }
    }
}
