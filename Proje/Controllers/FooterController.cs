using Microsoft.AspNetCore.Mvc;

namespace Proje.Controllers
{
	public class FooterController : Controller
	{		
        public IActionResult InformationalSecurityPolicy()
        {
            return View();
        }
        public IActionResult WorkHealthSecurityandEnvironmentPolicy()
        {
            return View();
        }
    }
}
