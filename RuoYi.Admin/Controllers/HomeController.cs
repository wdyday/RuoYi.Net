namespace RuoYi.Admin.Controllers
{
    [SuppressMonitor]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly SystemService _systemService;

        public HomeController(SystemService systemService)
        {
            _systemService = systemService;
        }

        public IActionResult Index()
        {
            ViewBag.Description = _systemService.GetDescription();

            return View();
        }
    }
}