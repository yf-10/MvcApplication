using Microsoft.AspNetCore.Mvc;

namespace MvcApplication.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public LoginController(IConfiguration configuration) : base(configuration)
        {
            this._configuration = configuration;
            this._logger = Logger.GetInstance(this._configuration);
        }

        // ---------------------------------------------------------------------
        // Auth
        // ---------------------------------------------------------------------
        [HttpGet]
        public IActionResult Auth()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                const string ERRORCODE = "E-90000001";
                _logger.Error("ERRORCODE: " + ERRORCODE);
                _logger.Error(ex.Message);
                _logger.Error(ex.Source ?? "");
                _logger.Error(ex.StackTrace ?? "");
                return RedirectToAction("Error", "Home", new { code = ERRORCODE, msg = ex.Message });
            }
        }

    }
}
