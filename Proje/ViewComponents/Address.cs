using Microsoft.AspNetCore.Mvc;
using Proje.Models.MVVM;
using Proje.Models;

namespace Proje.ViewComponents
{
    public class Address : ViewComponent
    {
        iakademi45Context context = new iakademi45Context();
        public string Invoke()
        {
            string address = context.Settings.FirstOrDefault(s => s.SettingID == 3).Address;
            return $"{address}";
        }


    }
}
