using Microsoft.AspNetCore.Mvc;
using Proje.Models;

namespace Proje.Controllers
{
    public class HomeController : Controller
    {
        MainPageModel mpm = new MainPageModel();
        iakademi45Context context = new iakademi45Context();
        cls_Product cp = new cls_Product();
        public IActionResult Index()
        {
            mpm.SliderProducts = cp.ProductSelect("Slider");
            mpm.ProductOfDay = cp.ProductDetails("ProductOfDay");//günün ürünü
            mpm.NewProducts = cp.ProductSelect("New");//yeni
            mpm.SpecialProducts = cp.ProductSelect("Special"); //özel
            mpm.DiscountedProducts = cp.ProductSelect("Discounted");//indirim
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted"); // öne çıkan
            mpm.TopsellerProducts = cp.ProductSelect("Topseller"); // cok satan
            mpm.StarProducts = cp.ProductSelect("Star"); // yıldız
            mpm.FeaturedProducts = cp.ProductSelect("Featured"); // fırsat
            mpm.NotableProducts = cp.ProductSelect("Notable"); // dikkat çeken
            
            return View(mpm);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        //Sepete ekle tıklanınca buraya gelecek

        public IActionResult CartProcess(int id)
        {
            return View();
        }

    }
}
