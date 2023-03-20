using Microsoft.AspNetCore.Mvc;
using Proje.Models;
using Proje.Models.MVVM;

namespace Proje.Controllers
{
    public class HomeController : Controller
    {
        MainPageModel mpm = new MainPageModel();
        iakademi45Context context = new iakademi45Context();
        cls_Product cp = new cls_Product();
        int mainpageCount = 0;


        public HomeController()
        {
            this.mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainPageCount;
        }
        public IActionResult Index()
        {
            mpm.SliderProducts = cp.ProductSelect("Slider", mainpageCount,"");
            mpm.ProductOfDay = cp.ProductDetails("ProductOfDay");//günün ürünü
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount,"");//yeni
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, ""); //özel
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "");//indirim
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, ""); // öne çıkan
            mpm.TopsellerProducts = cp.ProductSelect("Topseller", mainpageCount, ""); // cok satan
            mpm.StarProducts = cp.ProductSelect("Star", mainpageCount, ""); // yıldız
            mpm.FeaturedProducts = cp.ProductSelect("Featured", mainpageCount, ""); // fırsat
            mpm.NotableProducts = cp.ProductSelect("Notable", mainpageCount, ""); // dikkat çeken
            
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

        public IActionResult CategoryPage(int id)
        {
            List<Product> products = cp.ProductSelectWithCategoryID(id);
            return View(products);
        }
        public IActionResult SupplierPage(int id)
        {
            List<Product> products = cp.ProductSelectWithSupplierID(id);
            return View(products);
        }

        public IActionResult NewProducts()
        {
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount,"New");
            return View(mpm);
        }

        public PartialViewResult _PartialNewProducts(string pagenumber)
        {
            return PartialView();
        }
    }
}
