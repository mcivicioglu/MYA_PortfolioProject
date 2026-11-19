using System.ComponentModel.DataAnnotations;

namespace MYA_PortfolioProject.Entities
{
    public class Education
    {
        public int EducationId { get; set; }

        [Required(ErrorMessage = "Okul adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Okul adı en fazla 150 karakter olabilir.")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "Bölüm zorunludur.")]
        [StringLength(150, ErrorMessage = "Bölüm en fazla 150 karakter olabilir.")]
        public string Department { get; set; }

        [Range(1965, 2026, ErrorMessage = "Başlangıç yılı 1965 ile 2026 arasında olmalıdır.")]
        public int StartYear { get; set; }

        [Range(1970, 2026, ErrorMessage = "Bitiş yılı 1970 ile 2026 arasında olmalıdır.")]
        public int? EndYear { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }
    }
}
