using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MPA.Controllers
{
    [Authorize]
    public class SwarfMoistureController : Controller
    {
        public SwarfMoistureController()
        {

        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
