using Microsoft.AspNetCore.Mvc;
using MvcApplication.Models;
using System.Diagnostics;

namespace MvcApplication.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public HomeController(IConfiguration configuration) : base(configuration)
        {
            this._configuration = configuration;
            this._logger = Logger.GetInstance(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
