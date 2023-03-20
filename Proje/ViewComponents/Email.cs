using Microsoft.AspNetCore.Mvc;
using Proje.Models;

namespace Proje.ViewComponents
{
	public class Email : ViewComponent
	{
        iakademi45Context context = new iakademi45Context();
        public string Invoke()
        {
            string email = context.Settings.FirstOrDefault(s => s.SettingID == 1).Email;
            return $"{email}";
        }

    }
}
