using MYA_PortfolioProject.Entities;

namespace MYA_PortfolioProject.Areas.Admin.Models
{
    // Farklı tablolardan gelen özet verileri tek ekrana taşır.
    public class DashboardViewModel
    {
        public string NameSurname { get; set; } = "Admin";

        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int SkillCount { get; set; }
        public int SkillItemCount { get; set; }
        public int ExperienceCount { get; set; }
        public int EducationCount { get; set; }
        public int CertificateCount { get; set; }
        public int TotalMessages { get; set; }
        public int UnreadMessages { get; set; }

        public List<Message> LastMessages { get; set; } = new();
        public List<Project> LastProjects { get; set; } = new();
        public List<CategoryProjectCount> CategorySummary { get; set; } = new();
        public List<DailyMessageCount> Last7Days { get; set; } = new();
        public List<string> MissingSections { get; set; } = new();

        public int ReadRate => TotalMessages == 0 ? 0 : (TotalMessages - UnreadMessages) * 100 / TotalMessages;
    }

    public class CategoryProjectCount
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProjectCount { get; set; }
    }

    public class DailyMessageCount
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool IsToday { get; set; }
    }
}
