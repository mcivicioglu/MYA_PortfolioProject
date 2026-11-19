using System.ComponentModel.DataAnnotations;

namespace MYA_PortfolioProject.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Proje adı en fazla 100 karakter olabilir.")]
        public string ProjectTitle { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Görsel yolu en fazla 500 karakter olabilir.")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Kullanılan teknolojiler zorunludur.")]
        [StringLength(200, ErrorMessage = "Teknoloji alanı en fazla 200 karakter olabilir.")]
        public string Technology { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL giriniz (https://...).")]
        [StringLength(500, ErrorMessage = "Proje bağlantısı en fazla 500 karakter olabilir.")]
        public string? ProjectUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçiniz.")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
