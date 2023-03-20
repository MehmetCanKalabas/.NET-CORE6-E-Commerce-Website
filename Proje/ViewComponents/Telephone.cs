using Microsoft.AspNetCore.Mvc;
using Proje.Models;

namespace Proje.ViewComponents
{
    public class Telephone : ViewComponent
    {
        iakademi45Context context = new iakademi45Context();
        public string Invoke()
        {
            string telephone = context.Settings.FirstOrDefault(s => s.SettingID == 1).Telephone;
            return $"{telephone}";
        }

    }
}
