using Microsoft.AspNetCore.Mvc;

namespace HotelWebApi.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
