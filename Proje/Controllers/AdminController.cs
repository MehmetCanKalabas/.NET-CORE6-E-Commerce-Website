using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proje.Models;

namespace Proje.Controllers
{
    public class AdminController : Controller
    {
        cls_User u = new cls_User();
        cls_Product p = new cls_Product();
        cls_Category c = new cls_Category();
        cls_Supplier s = new cls_Supplier();
        cls_Status st = new cls_Status();
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email","Password","NameSurname")] User user)
        {
            if (ModelState.IsValid)
            {
               User? usr = await u.loginControl(user);
                if (usr != null)
                {
                    return RedirectToAction("Index");
                }
            }           
            else
            {
                ViewBag.error = "Login ve/veya şifre yanlış";
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
     
        public async Task<IActionResult> CategoryIndex()
        {
            List<Category> categories = await c.CategorySelect();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {
            CategoryFill();
            return View();
        }

        void CategoryFill()
        {
            List<Category> categories = c.CategorySelectMain();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = 
                c.CategoryID.ToString() });
        }

        //async void CategoryFillAll()
        //{
        //    List<Category> categories = await c.CategorySelect();
        //    ViewData["categoryList"] = categories.Select(c => new SelectListItem
        //    {
        //        Text = c.CategoryName,
        //        Value =
        //        c.CategoryID.ToString()
        //    });
        //}

        //async void SupplierFill()
        //{
        //    List<Supplier> suppliers = await s.SupplierSelect();
        //    ViewData["supplierList"] = suppliers.Select(s => new SelectListItem
        //    {
        //        Text = s.BrandName,
        //        Value =
        //        s.SupplierID.ToString()
        //    });
        //}

        //async void StatuFill()
        //{
        //    List<Status> statuses = await st.StatusSelect();
        //    ViewData["statusList"] = statuses.Select(s => new SelectListItem
        //    {
        //        Text = s.StatusName,
        //        Value =
        //        s.StatusID.ToString()
        //    });
        //}

        iakademi45Context context = new iakademi45Context();

        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            bool answer = cls_Category.CategoryInsert(category);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";                
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(CategoryCreate));
        }

        public async Task<IActionResult> CategoryEdit(int? id)
        {
            CategoryFill();
            if (id==null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await c.CategoryDetails(id);

            return View(category);
        }

        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            bool answer = cls_Category.CategoryUpdate(category);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(CategoryEdit));
            }           
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost,ActionName("CategoryDelete")]
        public async Task<IActionResult> CategoryDeleteConfirm(int id)
        {
            /*
            if (context.Categories == null)
            {
                return Problem("Kategori 'context.Categories' is null ");
            }

            var category = await context.Categories.FindAsync(id);

            if (category !=null)
            {
                context.Categories.Remove(category);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(CategoryIndex));
            */

            bool answer = cls_Category.CategoryDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(CategoryDelete));
            }
        }

        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            ViewBag.categoryname = category?.CategoryName;
            
            return View(category);
        }

        public async Task<IActionResult> SupplierIndex()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult SupplierCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SupplierCreate(Supplier supplier)
        {
            bool answer = cls_Supplier.SupplierInsert(supplier);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(SupplierCreate));
        }

        public async Task<IActionResult> SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await s.SupplierDetails(id);

            return View(supplier);
        }

        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {
            if (supplier.PhotoPath == null)
            {
                string PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath;
                supplier.PhotoPath = PhotoPath;
            }
            bool answer = cls_Supplier.SupplierUpdate(supplier);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierEdit));
            }
        }

        public async Task<IActionResult> SupplierDetails(int id)
        {
            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
            //ViewBag.brandname = supplier?.BrandName.ToUpper();

            return View(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("SupplierDelete")]
        public async Task<IActionResult> SupplierDeleteConfirm(int id)
        {           
            bool answer = cls_Supplier.SupplierDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierDelete));
            }
        }

        public async Task<IActionResult> StatusIndex()
        {
            List<Status> statuses = await st.StatusSelect();
            return View(statuses);
        }

        [HttpGet]
        public IActionResult StatusCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatusCreate(Status status)
        {
            bool answer = cls_Status.StatusInsert(status);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(StatusCreate));
        }


        public async Task<IActionResult> StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var status = await st.StatusDetails(id);

            return View(status);
        }

        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {           
            bool answer = cls_Status.StatusUpdate(status);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("StatusIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> StatusDelete(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        [HttpPost, ActionName("StatusDelete")]
        public async Task<IActionResult> StatusDeleteConfirm(int id)
        {
            bool answer = cls_Status.StatusDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("StatusIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusDelete));
            }
        }

        public async Task<IActionResult> StatusDetails(int id)
        {
            var status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);
            ViewBag.statusname = status?.StatusName;

            return View(status);
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<Product> products = await p.ProductSelect();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            List<Category> categories = await c.CategorySelect();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value =
                c.CategoryID.ToString()
            });
        
            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem
            {
                Text = s.BrandName,
                Value =
                s.SupplierID.ToString()
            });

            List<Status> statuses = await st.StatusSelect();
            ViewData["statusList"] = statuses.Select(s => new SelectListItem
            {
                Text = s.StatusName,
                Value =
                s.StatusID.ToString()
            });

            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            bool answer = cls_Product.ProductInsert(product);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(ProductCreate));
    
        }

        public async Task<IActionResult> ProductEdit(int? id)
        {
            CategoryFill();
            //SupplierFill();
            //StatusFill();

            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem
            {
                Text = s.BrandName,
                Value =
                s.SupplierID.ToString()
            });

            List<Status> statuses = await st.StatusSelect();
            ViewData["statusList"] = statuses.Select(s => new SelectListItem
            {
                Text = s.StatusName,
                Value =
                s.StatusID.ToString()
            });

            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await p.ProductDetails(id);

            return View(product);
        }

        [HttpPost]
        public IActionResult ProductEdit(Product product)
        {
            //veritabanından kayıt getirildi.
            Product prd = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID);
            //formdan gelmeyen, bazı kolonları null yerine eski bilgileri basıldı.
            product.AddDate = prd.AddDate;
            product.HighLighted = prd.HighLighted;
            product.TopSeller = prd.TopSeller;

            if (product.PhotoPath == null)
            {
                string PhotoPath = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
                product.PhotoPath = PhotoPath;
            }

            bool answer = cls_Product.ProductUpdate(product);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductEdit));
            }
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            ViewBag.productname = product?.ProductName;

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("ProductDelete")]
        public async Task<IActionResult> ProductDeleteConfirm(int id)
        {
            bool answer = cls_Product.ProductDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductDelete));
            }
        }
    }             
}
