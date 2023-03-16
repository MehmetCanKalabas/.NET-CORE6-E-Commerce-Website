using Microsoft.EntityFrameworkCore;

namespace Proje.Models
{
    public class cls_Category
    {
        iakademi45Context context = new iakademi45Context();

        public async Task<List<Category>> CategorySelect()
        {
            List<Category> categories = await context.Categories.ToListAsync();
            return categories;
        }

        public List<Category> CategorySelectMain()
        {
            List<Category> categories = context.Categories.Where(c=>c.ParentID == 0).ToList();
            return categories;
        }

        public static bool CategoryInsert(Category category)
        {       
             try
             {
                 using (iakademi45Context context = new iakademi45Context())
                 {
                     context.Add(category);
                     context.SaveChanges();
                     return true;
                 }               
             }
             catch (Exception)
             {
                 return false;
             }
        }

        public async Task<Category> CategoryDetails(int? id)
        {
            Category? categories = await context.Categories.FindAsync(id);
            return categories;
        }

        public static bool CategoryUpdate(Category category)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Update(category);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool CategoryDelete(int id)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    Category? category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
                    category.Active = false;


                    List<Category> categoryList = context.Categories.Where(c => c.ParentID == id).ToList();
                    foreach (var item in categoryList)
                    {
                        item.Active = false;
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
