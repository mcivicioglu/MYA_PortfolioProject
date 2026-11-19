using Microsoft.AspNetCore.Mvc;
using MYA_PortfolioProject.Context;

namespace MYA_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultContactComponentPartial : ViewComponent
    {
        private readonly PortfolioContext context;

        public _DefaultContactComponentPartial(PortfolioContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = context.Contacts.FirstOrDefault();
            return View(values);
        }
    }
}
