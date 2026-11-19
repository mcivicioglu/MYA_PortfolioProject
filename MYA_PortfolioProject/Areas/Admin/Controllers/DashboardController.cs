using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYA_PortfolioProject.Areas.Admin.Models;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private static readonly string[] DayLabels = { "Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt" };

        public DashboardController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                NameSurname = _context.Abouts.Select(x => x.NameSurname).FirstOrDefault() ?? "Admin",
                TotalProjects = _context.Projects.Count(),
                ActiveProjects = _context.Projects.Count(x => x.IsActive),
                SkillCount = _context.Skills.Count(),
                SkillItemCount = _context.SkillItems.Count(),
                ExperienceCount = _context.Experiences.Count(),
                EducationCount = _context.Educations.Count(),
                CertificateCount = _context.Certificates.Count(),
                TotalMessages = _context.Messages.Count(),
                UnreadMessages = _context.Messages.Count(x => !x.IsRead),

                LastMessages = _context.Messages
                    .OrderByDescending(x => x.SendDate).ThenByDescending(x => x.MessageId)
                    .Take(5).ToList(),

                LastProjects = _context.Projects.Include(x => x.Category)
                    .OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.ProjectId)
                    .Take(4).ToList(),

                CategorySummary = _context.Categories
                    .Select(c => new CategoryProjectCount { CategoryName = c.CategoryName, ProjectCount = c.Projects.Count })
                    .OrderByDescending(x => x.ProjectCount)
                    .ToList(),

                Last7Days = GetLast7Days()
            };

            // Sitede boş görünecek tekil bölümler: dashboard'da hatırlatma olarak gösterilir.
            if (!_context.HeroSections.Any()) model.MissingSections.Add("Hero");
            if (!_context.Abouts.Any()) model.MissingSections.Add("Hakkımda");
            if (!_context.Contacts.Any()) model.MissingSections.Add("İletişim Bilgileri");

            return View(model);
        }

        // Son 7 günün her günü için gelen mesaj sayısı (mesaj yoksa 0).
        private List<DailyMessageCount> GetLast7Days()
        {
            var today = DateTime.Today;
            var start = today.AddDays(-6);

            var countsByDay = _context.Messages
                .Where(x => x.SendDate >= start)
                .Select(x => x.SendDate)
                .AsEnumerable()
                .GroupBy(x => x.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            return Enumerable.Range(0, 7)
                .Select(i => start.AddDays(i))
                .Select(day => new DailyMessageCount
                {
                    Label = DayLabels[(int)day.DayOfWeek],
                    Count = countsByDay.TryGetValue(day, out var c) ? c : 0,
                    IsToday = day == today
                })
                .ToList();
        }
    }
}
