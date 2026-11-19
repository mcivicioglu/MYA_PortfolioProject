using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin controller'larının ortak tabanı: Area, DbContext, dosya servisi ve bildirim yardımcıları.
    /// </summary>
    [Area("Admin")]
    public abstract class AdminBaseController : Controller
    {
        protected readonly PortfolioContext _context;
        protected readonly IFileService _files;

        protected AdminBaseController(PortfolioContext context, IFileService files)
        {
            _context = context;
            _files = files;
        }

        protected void Success(string message) => TempData["SuccessMessage"] = message;
        protected void Error(string message) => TempData["ErrorMessage"] = message;

        /// <summary>
        /// Yeni bir dosya seçildiyse doğrular. Seçildi ve geçerliyse true döner;
        /// geçersizse ModelState'e <paramref name="fieldName"/> için hata ekler.
        /// </summary>
        protected bool ValidateUpload(IFormFile? file, string[] allowedExtensions, string fieldName)
        {
            if (file is not { Length: > 0 })
                return false;

            var error = _files.Validate(file, allowedExtensions);
            if (error != null)
            {
                ModelState.AddModelError(fieldName, error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kayıttaki dosya alanını günceller: yeni dosya varsa kaydeder, "kaldır" işaretliyse boşaltır.
        /// Silinmesi gereken eski dosyanın yolunu döner (kayıt başarılı olduktan sonra <see cref="IFileService.Delete"/> ile silinmeli).
        /// </summary>
        protected async Task<string?> ApplyUploadAsync(IFormFile? file, bool hasValidFile, bool remove, string folder,
            Func<string?> getter, Action<string?> setter)
        {
            var oldPath = getter();

            if (hasValidFile)
            {
                setter(await _files.SaveAsync(file!, folder));
                return oldPath;
            }

            if (remove)
            {
                setter(null);
                return oldPath;
            }

            return null;
        }
    }
}
