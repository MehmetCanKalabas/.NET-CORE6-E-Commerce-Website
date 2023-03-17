using Microsoft.AspNetCore.Mvc;
using Proje.Models.MVVM;
using Proje.Models;

namespace Proje.ViewComponents
{
	public class Footers : ViewComponent
	{
        iakademi45Context context = new iakademi45Context();
        public IViewComponentResult Invoke()
        {
            List<Supplier> suppliers = context.Suppliers.ToList();
            return View(suppliers);
        }
    }
}
