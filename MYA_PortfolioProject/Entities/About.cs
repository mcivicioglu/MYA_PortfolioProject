using System.ComponentModel.DataAnnotations;

namespace MYA_PortfolioProject.Entities
{
    public class About
    {
        public int AboutId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Hakkımda metni zorunludur.")]
        [StringLength(2000, ErrorMessage = "Hakkımda metni en fazla 2000 karakter olabilir.")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Görsel yolu en fazla 500 karakter olabilir.")]
        public string? MainImageUrl { get; set; }

        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir.")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Konum zorunludur.")]
        [StringLength(100, ErrorMessage = "Konum en fazla 100 karakter olabilir.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Meslek/unvan zorunludur.")]
        [StringLength(100, ErrorMessage = "Meslek/unvan en fazla 100 karakter olabilir.")]
        public string Profession { get; set; }

        [StringLength(500, ErrorMessage = "CV bağlantısı en fazla 500 karakter olabilir.")]
        public string? AboutCVUrl { get; set; }
    }
}
