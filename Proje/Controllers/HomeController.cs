using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Proje.Models;
using Proje.Models.MVVM;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using XAct;
using System.Collections.Specialized;
using System.Text;
using Microsoft.CodeAnalysis.Differencing;
using System.Net;
using Newtonsoft.Json;

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
            //EfCore
            //mpm.ProductDetails = context.Products.FirstOrDefault(p => p.ProductID == id);

            mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();

            //linq
            mpm.CategoryName = (from p in context.Products
                                   join c in context.Categories
                                   on p.CategoryID equals c.CategoryID
                                   where p.ProductID == id
                                   select c.CategoryName).FirstOrDefault();

            mpm.BrandName = (from p in context.Products
                                   join s in context.Suppliers
                                   on p.SupplierID equals s.SupplierID
                                   where p.ProductID == id
                                   select s.BrandName).FirstOrDefault();

            mpm.RelatedProducts = context.Products.Where(p => p.Releted == mpm.ProductDetails.Releted && p.ProductID != id).ToList();

            cls_Product.HighlightedIncrease(id);
            return View(mpm);
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
                o.AddToMyCart(id.ToString());
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
            string url = Request.Headers["Referer"].ToString(); // sepete nerede eklediyse orada kalmaya devam edecek.
            return Redirect(url);
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
        
        public IActionResult Cart()
        {
            cls_Order o = new cls_Order();
            List<cls_Order> sepet;

            if (HttpContext.Request.Query["scid"].ToString() != "")
            {
                //sil butonuyla geldi
                string? scid = HttpContext.Request.Query["scid"];
                o.MyCart = Request.Cookies["sepetim"]; // tarayıcıdan al , propertye yaz
                o.DeleteFromMyCart(scid); //propertyden silindi
                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("sepetim", o.MyCart, cookieOptions); // tarayıcıya silinmiş(güncel) hali gitti
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                TempData["Message"] = "Ürün Sepetten Silindi";
                //sepet içindeki son halini cart.cshtml sayfasına da gönderecek
                sepet = o.SelectMyCart();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                //Sepetim tıklanınca
                var cookie = Request.Cookies["sepetim"];
                if (cookie == null)
                {
                    o.MyCart = "";
                    sepet = o.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else
                {
                    var cookieOptions = new CookieOptions();
                    o.MyCart = Request.Cookies["sepetim"];
                    sepet = o.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            if (sepet.Count == 0)
            {
                ViewBag.Sepetim = null;
            }
            return View();
        }

        public IActionResult Order()
        {
            //HttpContext.Session.SetString("Email", "deneme");
            //HttpContext.Session.GetString("Email");
            if (HttpContext.Session.GetString("Email") != null)
            {
                //kullanıcı Login.cshtml den giriş yapıp, Session alıp geldi
                User? usr =  cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email"));
                return View(usr);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            string txt_individual = Request.Form["txt_individual"];
            string txt_corporate = Request.Form["txt_corporate"];

            if (txt_individual != null)
            {
                //bireysel fatura
                //bu kullanım  webservicecontroller içine metotları yazarsan
                WebServiceController.tckimlik_vergi_no = txt_individual; 
                //property olarak kullanırsan da cls_order'da prop ve metotlar
                o.tckimlik_vergi_no = txt_individual;
                o.EfaturaCreate();
            }
            else
            {
                //kurumsal fatura
                WebServiceController.tckimlik_vergi_no = txt_corporate;
                o.tckimlik_vergi_no = txt_corporate;
                o.EfaturaCreate();
            }
            string kredikartno = Request.Form["kredikartno"]; //IFormCollection olmadan
            string kredikartay = frm["kredikartay"];
            string kredikartyil = frm["kredikartyil"];
            string kredikartcvv = frm["kredikartcvv"];

            return RedirectToAction("backgref");

            //buradan sonraki kodlar, payu,iyzico

            //payu dan gelen örnek kodlar

            /*
            NameValueCollection data = new NameValueCollection();
            string url = "https://mcan.com/backref";

            data.Add("BACK_REF",url);
            data.Add("CC_CVV", kredikartcvv);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR","20" + kredikartyil);

            var deger = "";
            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "Size verilen SECRET_KEY buraya yazılacak";

            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH",hash);

            var x = POSTFromPAYU("https://secure.payu.com.tr/order/....",data);

            //sanal kart
            if (x.Contains("<STATUS>SUCCESS</STATUS>")&& x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart(debit kart) ile alışveriş yaptı,bankadan onay aldı
            }
            else
            {
                //gerçek kart ile alışveriş yaptı,bankadan onay aldı
            }
            */
        }

        public IActionResult backgref()
        {
            ConfirmOrder();
            return RedirectToAction("ConfirmPage");
        }

        public static string OrderGroupGUID = "";
        public IActionResult ConfirmOrder()
        {
            //sipariş tablosuna kaydet
            //sepetim cookiesinden sepet temizlenecek
            //e fatura oluştur metodunu cagır

            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie != null)
            {
                o.MyCart = cookie;
                OrderGroupGUID = o.OrderCreate(HttpContext.Session.GetString("Email").ToString());

                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("sepetim"); // tarayıcıdan sepeti sil
               // cls_User.Send_Sms(OrderGroupGUID);
               // cls_User.Send_Email(OrderGroupGUID);
            }
            return RedirectToAction("ConfirmPage");
        }

        public IActionResult ConfirmPage()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }

        public static string HashWithSignature(string deger , string signatureKey)
        {
            return "";
        }

        public static string POSTFromPAYU(string url, NameValueCollection data)
        {
            return "";
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            string answer = cls_User.MemberControl(user);
            if (answer == "error")
            {
                HttpContext.Session.SetString("Mesaj","Email/Şifre yanlış girildi.");
                TempData["Message"] = "Email/Şifre yanlış girildi.";
                return View();
            }
            else if (answer == "admin")
            {
                HttpContext.Session.SetString("Email",answer);
                HttpContext.Session.SetString("Admin",answer);
                return RedirectToAction("Login","Admin");
            }
            else
            {
                HttpContext.Session.SetString("Email",answer);
                return RedirectToAction("Index");
            }      
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (cls_User.loginEmailControl(user) == false)
            {
                bool answer = cls_User.AddUser(user);

                if (answer)
                {
                    TempData["Message"] = "Üyelik Oluşturuldu/Giriş yapabilirsiniz.";
                    return RedirectToAction("Login");
                }          
            }
            else
            {
                TempData["Message"] = "Bu Email zaten kayıtlı.";
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");
        }

        public IActionResult MyOrders(int id)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<vw_MyOrders> orders = o.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //detaylı arama
        public IActionResult DetailedSearch(string search)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers = context.Suppliers.ToList();
            return View();
        }

        public IActionResult DProducts(int CategoryID,string[] SupplierID,string price,string IsInStock)
        {
            price = price.Replace(" ", "");
            string[] PriceArray = price.Split('-');
            string startPrice = PriceArray[0];
            string endPrice = PriceArray[1];

            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }

            int count = 0;
            string suppliervalue = ""; //1,2,4
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count == 0)
                {
                    suppliervalue = "SupplierID = " + SupplierID[i];
                    count++;
                }
                else
                {
                    suppliervalue += " or SupplierID =" + SupplierID[i];

                }
            }
            string query = "select * from Products where CategoryID = "+CategoryID+" and ("+suppliervalue+") and (UnitPrice > "+startPrice+" and UnitPrice < "+endPrice+ ") and Stock "+sign+" 0 order by ProductName";

            ViewBag.Products = cp.SelectProductsByDetails(query);

            return View();
        }

        public IActionResult PharmacyOnDuty()
        {
            //https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler

            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");
            var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);

            return View(pharmacy);
        }
        public IActionResult ArtAndCulture()
        {
            //https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler

            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");
            var activite = JsonConvert.DeserializeObject<List<Activite>>(json);

            return View(activite);
        }

        public PartialViewResult gettingProducts(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<sp_arama> ulist = cls_Product.gettingSearchProducts(id);
            string json = JsonConvert.SerializeObject(ulist);
            var response = JsonConvert.DeserializeObject<List<Search>>(json);
            return PartialView(response);
        }
    }
}
