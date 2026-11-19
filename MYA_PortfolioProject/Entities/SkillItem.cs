using System.ComponentModel.DataAnnotations;

namespace MYA_PortfolioProject.Entities
{
    public class SkillItem
    {
        public int SkillItemId { get; set; }

        [Required(ErrorMessage = "Madde adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Madde adı en fazla 100 karakter olabilir.")]
        public string SkillItemTitle { get; set; }
        [StringLength(150)]
        public string? IconUrl { get; set; }

        public int SkillId { get; set; }

        public Skill? Skill { get; set; }
    }
}
