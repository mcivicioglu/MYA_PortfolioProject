namespace MYA_PortfolioProject.Areas.Admin.Models
{
    /// <summary>_FileInput partial'ının parametreleri.</summary>
    public class FileInputModel
    {
        /// <summary>Formdaki input adı; controller'daki IFormFile parametresiyle aynı olmalı.</summary>
        public string Name { get; set; } = "imageFile";
        public string Label { get; set; } = "Görsel";
        public string? CurrentUrl { get; set; }
        public string Accept { get; set; } = ".jpg,.jpeg,.png,.webp,.gif";
        public string Hint { get; set; } = "JPG, PNG, WEBP veya GIF · en fazla 5 MB";
        public bool IsImage { get; set; } = true;
        public bool Portrait { get; set; }

        /// <summary>Doluysa "mevcut dosyayı kaldır" kutusu gösterilir; bool parametre adı.</summary>
        public string? RemoveName { get; set; }
    }
}
