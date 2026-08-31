using Microsoft.AspNetCore.Mvc;

namespace AITouristTransport.Web.Controllers
{
    public class DestinationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}