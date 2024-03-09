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
            this._logger = Logger.GetInstance(this._configuration);
        }

        // ---------------------------------------------------------------------
        // Index
        // ---------------------------------------------------------------------
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                const string ERRORCODE = "E-00000001";
                this._logger.Error("ERRORCODE: " + ERRORCODE);
                this._logger.Error(ex.Message);
                this._logger.Error(ex.Source ?? "");
                this._logger.Error(ex.StackTrace ?? "");
                return RedirectToAction("Error", "Home", new { code = ERRORCODE, msg = ex.Message });
            }
        }

        // ---------------------------------------------------------------------
        // Privacy Policy
        // ---------------------------------------------------------------------
        [HttpGet]
        [Route("/Privacy")]
        [Route("/Home/Privacy")]
        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                const string ERRORCODE = "E-00000002";
                this._logger.Error("ERRORCODE: " + ERRORCODE);
                this._logger.Error(ex.Message);
                this._logger.Error(ex.Source ?? "");
                this._logger.Error(ex.StackTrace ?? "");
                return RedirectToAction("Error", "Home", new { code = ERRORCODE, msg = ex.Message });
            }
        }

        // ---------------------------------------------------------------------
        // Error
        // ---------------------------------------------------------------------
        [HttpGet]
        [Route("/Error")]
        [Route("/Home/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string code, string msg)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorCode = code,
                ErrorMessage = msg
            });
        }
    }
}
