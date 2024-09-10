using Microsoft.AspNetCore.Mvc;

namespace RabbitMQWeb.Controllers
{
    public class MonitoringController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
