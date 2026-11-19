using System.ComponentModel.DataAnnotations;

namespace MYA_PortfolioProject.Entities
{
    public class SocialLink
    {
        public int SocialLinkId { get; set; }

        [Required(ErrorMessage = "Platform adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Platform adı en fazla 50 karakter olabilir.")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "Bağlantı zorunludur.")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz (https://...).")]
        [StringLength(300, ErrorMessage = "Bağlantı en fazla 300 karakter olabilir.")]
        public string Url { get; set; }

        [Required(ErrorMessage = "İkon zorunludur.")]
        [StringLength(50, ErrorMessage = "İkon en fazla 50 karakter olabilir.")]
        public string Icon { get; set; }
    }
}
