using Microsoft.AspNetCore.Mvc;

namespace MvcApplication.Controllers
{
    [Route("[controller]/[action]")]
    public class BudgetController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public BudgetController(IConfiguration configuration) : base(configuration)
        {
            this._configuration = configuration;
            this._logger = Logger.GetInstance(this._configuration);
        }

        // ---------------------------------------------------------------------
        // Graph
        // ---------------------------------------------------------------------
        [HttpGet]
        public IActionResult Graph()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                const string ERRORCODE = "E-10000001";
                this._logger.Error("ERRORCODE: " + ERRORCODE);
                this._logger.Error(ex.Message);
                this._logger.Error(ex.Source ?? "");
                this._logger.Error(ex.StackTrace ?? "");
                return RedirectToAction("Error", "Home", new { code = ERRORCODE, msg = ex.Message });
            }
        }

    }
}
