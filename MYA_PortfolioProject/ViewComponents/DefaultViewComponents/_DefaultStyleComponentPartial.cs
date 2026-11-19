using Microsoft.AspNetCore.Mvc;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultStyleComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
