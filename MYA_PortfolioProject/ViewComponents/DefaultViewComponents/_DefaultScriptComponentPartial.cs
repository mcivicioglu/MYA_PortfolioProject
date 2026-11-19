using Microsoft.AspNetCore.Mvc;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultScriptComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
