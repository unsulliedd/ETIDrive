using Microsoft.AspNetCore.Mvc;

namespace ETIDrive_WebUI.ViewComponents
{
    public class FilterPanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("FilterPanel");
        }   
    }
}
