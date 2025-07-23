using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Form_Builder.Views.ViewComponents
{
    public class DropdownFieldViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int index)
        {
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            ViewBag.Index = index;
            return View(options);
        }
    }

}
