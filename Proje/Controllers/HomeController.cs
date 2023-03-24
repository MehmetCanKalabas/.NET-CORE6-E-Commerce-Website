using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Proje.Models;
using Proje.Models.MVVM;
using Microsoft.AspNetCore.Http;
namespace Proje.Controllers
{
    public class HomeController : Controller
    {
        MainPageModel mpm = new MainPageModel();
        iakademi45Context context = new iakademi45Context();
        cls_Product cp = new cls_Product();
        cls_Order o = new cls_Order();
        int mainpageCount = 0;


        public HomeController()
        {
            mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainPageCount;
        }
        public IActionResult Index()
        {
            mpm.SliderProducts = cp.ProductSelect("Slider", mainpageCount,"",0);
            mpm.ProductOfDay = cp.ProductDetails("ProductOfDay");//günün ürünü
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount,"", 0);//yeni
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "", 0); //özel
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "", 0);//indirim
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "", 0); // öne çıkan
            mpm.TopsellerProducts = cp.ProductSelect("Topseller", mainpageCount, "", 0); // cok satan
            mpm.StarProducts = cp.ProductSelect("Star", mainpageCount, "", 0); // yıldız
            mpm.FeaturedProducts = cp.ProductSelect("Featured", mainpageCount, "", 0); // fırsat
            mpm.NotableProducts = cp.ProductSelect("Notable", mainpageCount, "", 0); // dikkat çeken
            
            return View(mpm);
        }

        public IActionResult Details(int id)
        {
            cls_Product.HighlightedIncrease(id);
            return View();
        }

        //Sepete ekle tıklanınca buraya gelecek
        //microsoft.aspnetcore.http
        public IActionResult CartProcess(int id)
        {
            cls_Product.HighlightedIncrease(id);
            o.ProductID = id;
            o.Quantity = 1;

            var cookieOptions = new CookieOptions();
            //read
            var cookie = Request.Cookies["sepetim"]; // Tarayıcıdan aldım
            if (cookie == null)
            {
                cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük çerez
                cookieOptions.Path = "/";
                o.MyCart = "";
                o.AddToCart(id.ToString());
                Response.Cookies.Append("sepetim",o.MyCart, cookieOptions); // Tarayıcıya gönderdim
                HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi.");
                TempData["Message"] = "Ürün Sepetinize Eklendi.";
            }
            else
            {
                o.MyCart = cookie; // tarayıcıda ki sepet bilgisini propertye gönderdim
                if (o.AddToMyCart(id.ToString()) == false)
                {
                    //ürün sepete daha önce eklenmemiş,ekle
                    HttpContext.Response.Cookies.Append("sepetim", o.MyCart, cookieOptions);
                    cookieOptions.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi.");
                    TempData["Message"] = "Ürün Sepetinize Eklendi.";
                }
                else
                {
                    //Bu ürün sepette var
                    HttpContext.Session.SetString("Message", "Bu Ürün Zaten Sepetinizde Var.");
                    HttpContext.Session.GetString("Message");
                    TempData["Message"] = "Bu Ürün Zaten Sepetinizde Var.";
                }
            }
            return RedirectToAction("Index");
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
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount,"New",0);
            return View(mpm);
        }

        public PartialViewResult _PartialNewProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "New", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "Special", 0);
            return View(mpm);
        }
        public PartialViewResult _PartialSpecialProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "Special", pagenumber);
            return PartialView(mpm);
        }
        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "Discounted", 0);
            return View(mpm);
        }
        public PartialViewResult _PartialDiscountedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "Discounted", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult HighlightedProducts()
        {
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "Highlighted", 0);
            return View(mpm);
        }
        public PartialViewResult _PartialHighlightedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "Highlighted", pagenumber);
            return PartialView(mpm);
        }
        public IActionResult TopSellerProducts(int page = 1, int pageSize = 4)
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller),page,pageSize);

            return View("TopSellerProducts",model);
        }
        

    }
}
