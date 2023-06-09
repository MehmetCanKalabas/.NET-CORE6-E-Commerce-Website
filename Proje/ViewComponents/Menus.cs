﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje.Models;
using Proje.Models.MVVM;

namespace Proje.ViewComponents
{
    public class Menus : ViewComponent
    {
        iakademi45Context context = new iakademi45Context();
        public IViewComponentResult Invoke()
        {
            List<Category> categories = context.Categories.ToList();
            return View(categories);
        }

    }
}
