using Microsoft.AspNetCore.Mvc;

namespace MYA_PortfolioProject.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
