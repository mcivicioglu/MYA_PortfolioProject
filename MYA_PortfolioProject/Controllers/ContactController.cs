using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Entities;

namespace MYA_PortfolioProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly PortfolioContext _context;

        public ContactController(PortfolioContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(Message message, string? website)
        {
            // Honeypot alanı doluysa istek bir bottan gelmiştir: kaydetmeden başarılı yanıt dön.
            if (!string.IsNullOrWhiteSpace(website))
                return Json(new { success = true, message = "Mesajınız alındı. Teşekkürler!" });

            ModelState.Remove(nameof(Message.SendDate));
            ModelState.Remove(nameof(Message.IsRead));

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value!.Errors.First().ErrorMessage);

                return BadRequest(new { success = false, message = "Lütfen formdaki hataları düzeltin.", errors });
            }

            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();

            return Json(new { success = true, message = "Mesajınız başarıyla gönderildi. En kısa sürede dönüş yapacağım." });
        }
    }
}
