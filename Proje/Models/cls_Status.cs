using Microsoft.EntityFrameworkCore;
using Proje.Models.MVVM;

namespace Proje.Models
{
    public class cls_Status
    {
        iakademi45Context context = new iakademi45Context();

        public async Task<List<Status>> StatusSelect()
        {
            List<Status> statuses = await context.Statuses.ToListAsync();
            return statuses;
        }



        public static bool StatusInsert(Status status)
        {
            //metod static oldugu için, context burada tanımlanmak zorunda
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Add(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Status>StatusDetails(int? id)
        {
            Status? status = await context.Statuses.FindAsync(id);
            return status;
        }

        public static bool StatusUpdate(Status status)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Update(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool StatusDelete(int? id)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    Status? status = context.Statuses.FirstOrDefault(c => c.StatusID == id);
                    status.Active = false;
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
