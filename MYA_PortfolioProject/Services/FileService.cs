namespace MYA_PortfolioProject.Services
{
    public static class FileKinds
    {
        // SVG bilerek yok: içine script gömülebildiği için yüklemeye kapalı.
        public static readonly string[] Images = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        public static readonly string[] Documents = { ".pdf" };
        public static readonly string[] ImagesAndDocuments = Images.Concat(Documents).ToArray();

        public static string Accept(string[] extensions) => string.Join(",", extensions);
    }

    public interface IFileService
    {
        /// <summary>Geçerliyse null, değilse kullanıcıya gösterilecek hata mesajını döner.</summary>
        string? Validate(IFormFile file, string[] allowedExtensions, long maxBytes = FileService.DefaultMaxBytes);

        /// <summary>Dosyayı wwwroot/uploads/{folder} altına rastgele bir adla kaydeder ve "/uploads/..." yolunu döner.</summary>
        Task<string> SaveAsync(IFormFile file, string folder);

        /// <summary>Yalnızca wwwroot/uploads altındaki dosyaları siler; dış bağlantıları ve diğer yolları yok sayar.</summary>
        void Delete(string? relativePath);
    }

    public class FileService : IFileService
    {
        public const long DefaultMaxBytes = 5 * 1024 * 1024; // 5 MB

        private readonly string _uploadsRoot;

        public FileService(IWebHostEnvironment env)
        {
            _uploadsRoot = Path.GetFullPath(Path.Combine(env.WebRootPath, "uploads"));
        }

        public string? Validate(IFormFile file, string[] allowedExtensions, long maxBytes = DefaultMaxBytes)
        {
            if (file.Length == 0)
                return "Seçilen dosya boş.";

            if (file.Length > maxBytes)
                return $"Dosya en fazla {maxBytes / 1024 / 1024} MB olabilir.";

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return "İzin verilen dosya türleri: " + string.Join(", ", allowedExtensions).ToUpperInvariant().Replace(".", "");

            if (!HasValidSignature(file, extension))
                return "Dosya içeriği, uzantısıyla uyuşmuyor.";

            return null;
        }

        public async Task<string> SaveAsync(IFormFile file, string folder)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var safeFolder = string.Concat(folder.Where(char.IsLetterOrDigit)).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{extension}"; // kullanıcının verdiği dosya adı hiç kullanılmaz

            var directory = Path.Combine(_uploadsRoot, safeFolder);
            Directory.CreateDirectory(directory);

            await using var stream = new FileStream(Path.Combine(directory, fileName), FileMode.CreateNew);
            await file.CopyToAsync(stream);

            return $"/uploads/{safeFolder}/{fileName}";
        }

        public void Delete(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath) || !relativePath.StartsWith("/uploads/"))
                return;

            var fullPath = Path.GetFullPath(Path.Combine(_uploadsRoot, relativePath["/uploads/".Length..]));
            if (fullPath.StartsWith(_uploadsRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && File.Exists(fullPath))
                File.Delete(fullPath);
        }

        // Uzantı kolayca değiştirilebildiği için dosyanın ilk baytlarına (imza) da bakılır.
        private static bool HasValidSignature(IFormFile file, string extension)
        {
            using var stream = file.OpenReadStream();
            var header = new byte[12];
            var read = stream.Read(header, 0, header.Length);
            if (read < 4)
                return false;

            return extension switch
            {
                ".jpg" or ".jpeg" => header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
                ".png" => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47,
                ".gif" => header[0] == 'G' && header[1] == 'I' && header[2] == 'F',
                ".webp" => read == 12 && header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F'
                           && header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P',
                ".pdf" => header[0] == '%' && header[1] == 'P' && header[2] == 'D' && header[3] == 'F',
                _ => false
            };
        }
    }
}
