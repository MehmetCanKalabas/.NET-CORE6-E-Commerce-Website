using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje.Models;

namespace Proje.ViewComponents
{
    public class Menus : ViewComponent
    {
        iakademi45Context context = new iakademi45Context();
        public IViewComponentResult Invoke()
        {
            List<Category> kategoriListesi = context.Categories.ToList();
            return View(kategoriListesi);
        }

    }
}
