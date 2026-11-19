using System.ComponentModel.DataAnnotations;

namespace MYA_PortfolioProject.Entities
{
    public class Certificate
    {
        public int CertificateId { get; set; }

        [Required(ErrorMessage = "Sertifika/Kurs adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Sertifika adı en fazla 150 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Kurum adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kurum adı en fazla 100 karakter olabilir.")]
        public string Institution { get; set; }
        [Required(ErrorMessage = "Tarih zorunludur.")]
        [StringLength(50, ErrorMessage = "Tarih alanı en fazla 50 karakter olabilir.")]
        public string Date { get; set; }

        [StringLength(500, ErrorMessage = "Sertifika bağlantısı en fazla 500 karakter olabilir.")]
        public string? CertificateUrl { get; set; }
    }
}