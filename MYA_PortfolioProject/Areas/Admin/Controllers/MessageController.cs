using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Services;

namespace MYA_PortfolioProject.Areas.Admin.Controllers
{
    // Gelen kutusu: mesaj içeriği düzenlenmez; okundu durumu yönetilir ve mesaj silinebilir.
    public class MessageController : AdminBaseController
    {
        public MessageController(PortfolioContext context, IFileService files) : base(context, files) { }

        public IActionResult MessageList(string? search, string status = "all")
        {
            var query = _context.Messages.AsQueryable();

            var term = search?.Trim();
            if (!string.IsNullOrEmpty(term))
                query = query.Where(x => x.SenderName.Contains(term) || x.SenderSubject.Contains(term) || x.SenderEmail.Contains(term));

            query = status switch
            {
                "unread" => query.Where(x => !x.IsRead),
                "read" => query.Where(x => x.IsRead),
                _ => query
            };

            ViewBag.Search = term;
            ViewBag.Status = status;
            ViewBag.TotalCount = _context.Messages.Count();
            ViewBag.UnreadCount = _context.Messages.Count(x => !x.IsRead);

            return View(query.OrderByDescending(x => x.SendDate).ThenByDescending(x => x.MessageId).ToList());
        }

        // Pencere (modal) içeriği için JSON döner.
        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            return Json(new
            {
                id = message.MessageId,
                senderName = message.SenderName,
                senderEmail = message.SenderEmail,
                senderSubject = message.SenderSubject,
                content = message.Content,
                sendDate = message.SendDate.ToString("dd.MM.yyyy HH:mm"),
                isRead = message.IsRead
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetReadStatus(int id, bool isRead, string? returnStatus)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            message.IsRead = isRead;
            await _context.SaveChangesAsync();

            // fetch ile çağrıldığında JSON, normal form gönderiminde listeye yönlendirme döner.
            if (Request.Headers["X-Requested-With"] == "fetch")
                return Json(new { success = true, isRead = message.IsRead, unreadCount = _context.Messages.Count(x => !x.IsRead) });

            Success(isRead ? "Mesaj okundu olarak işaretlendi." : "Mesaj okunmadı olarak işaretlendi.");
            return RedirectToAction(nameof(MessageList), new { status = returnStatus });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var unread = _context.Messages.Where(x => !x.IsRead).ToList();
            unread.ForEach(x => x.IsRead = true);
            await _context.SaveChangesAsync();

            Success(unread.Count > 0 ? $"{unread.Count} mesaj okundu olarak işaretlendi." : "Okunmamış mesaj yok.");
            return RedirectToAction(nameof(MessageList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int id, string? returnStatus)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                Success("Mesaj silindi.");
            }
            return RedirectToAction(nameof(MessageList), new { status = returnStatus });
        }
    }
}
