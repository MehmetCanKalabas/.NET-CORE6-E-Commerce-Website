using Proje.Models.MVVM;

namespace Proje.Models
{
	public class cls_Setting
	{
        iakademi45Context context = new iakademi45Context();
        public Setting SettingDetails()
        {
            Setting setting = context.Settings.FirstOrDefault(s=>s.SettingID == 1);
            return setting;
        }

        public static bool SettingUpdate(Setting setting)
        {
            try
            {
                using (iakademi45Context context = new iakademi45Context())
                {
                    context.Update(setting);
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
