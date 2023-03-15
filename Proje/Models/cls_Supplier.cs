using Microsoft.EntityFrameworkCore;

namespace Proje.Models
{
    public class cls_Supplier
    {
        iakademi45Context context = new iakademi45Context();

        public async Task<List<Supplier>> SupplierSelect()
        {
            List<Supplier> suppliers = await context.Suppliers.ToListAsync();
            return suppliers;
        }
       
        public static bool SupplierInsert(Supplier supplier)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Add(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<Supplier> SupplierDetails(int? id)
        {
            Supplier? supplier = await context.Suppliers.FindAsync(id);
            return supplier;
        }

        public static bool SupplierUpdate(Supplier supplier)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Update(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool SupplierDelete(int id)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    Supplier? supplier = context.Suppliers.FirstOrDefault(c => c.SupplierID == id);
                    supplier.Active = false;
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
