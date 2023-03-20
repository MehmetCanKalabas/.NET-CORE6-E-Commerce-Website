using Microsoft.EntityFrameworkCore;
using Proje.Models.MVVM;

namespace Proje.Models
{
    public class cls_Product
    {

        iakademi45Context context = new iakademi45Context();

        public async Task<List<Product>> ProductSelect()
        {
            List<Product>? products = await context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> ProductDetails(int? id)
        {
            Product? product = await context.Products.FindAsync(id);
            return product;
        }

        public static bool ProductInsert(Product product)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    product.AddDate = DateTime.Now;
                    context.Add(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ProductUpdate(Product product)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Update(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool ProductDelete(int id)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    Product? product = context.Products.FirstOrDefault(c => c.ProductID == id);
                    product.Active = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Product> ProductSelect(string productName, int mainpageCount,string subPageName)
        {
            List<Product> products;

            if (productName == "New")
            {
                if (subPageName == "")
                {
                    //Home/Index
                    products = context.Products.OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
                else
                {
                    //En yeni ürünler
                    products = context.Products.OrderByDescending(p => p.AddDate).Take(4).ToList();
                }             
            }



            else if(productName == "Special")
            {
                products = context.Products.Where(p => p.StatusID == 2).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
            }
            else if (productName == "Discounted")
            {
                products = context.Products.OrderByDescending(p => p.Discount).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
            }
            else if (productName == "Highlighted")
            {
                products = context.Products.OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
            }
            else if (productName == "Topseller")
            {
                products = context.Products.OrderByDescending(p => p.TopSeller).Take(mainpageCount).ToList();
            }
            else if (productName == "Slider")
            {
                products = context.Products.Where(p => p.StatusID == 1).Take(mainpageCount).ToList();
            }
            else if (productName == "Star")
            {
                products = context.Products.Where(p => p.StatusID == 3).Take(mainpageCount).ToList();
            }
            else if (productName == "Featured")
            {
                products = context.Products.Where(p => p.StatusID == 4).Take(mainpageCount).ToList();
            }
            else if (productName == "Notable")
            {
                products = context.Products.Where(p => p.StatusID == 5).Take(mainpageCount).ToList();
            }
            else
            {
                products = context.Products.Where(p => p.StatusID == 000).Take(mainpageCount).ToList();
            }
            return products;
        }

        public Product ProductDetails(string productName)
        {
            Product product = context.Products.FirstOrDefault(p => p.StatusID == 6);
            return product;
        }

        public List<Product> ProductSelectWithCategoryID(int id)
        {
            List<Product>? products = context.Products.Where(p => p.CategoryID == id).ToList();
            return products;
        }
        public List<Product> ProductSelectWithSupplierID(int id)
        {
            List<Product>? products = context.Products.Where(p => p.SupplierID == id).ToList();
            return products;
        }
    }
}
