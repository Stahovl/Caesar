using Microsoft.AspNetCore.Mvc;

namespace Caesar.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
