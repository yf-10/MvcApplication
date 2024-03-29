﻿using Microsoft.AspNetCore.Mvc;
using Npgsql;

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
                const string CONNECTION_STR = "Server=localhost;Port=5432;User ID=postgres;Database=postgres;Password=postgres;Enlist=true";
                using (NpgsqlConnection con = new NpgsqlConnection(CONNECTION_STR))
                {
                    con.Open();
                    _logger.Info("Connect to database.");
                }
                return View();
            }
            catch (Exception ex)
            {
                const string ERRORCODE = "E-10000001";
                _logger.Error("ERRORCODE: " + ERRORCODE);
                _logger.Error(ex.Message);
                _logger.Error(ex.Source ?? "");
                _logger.Error(ex.StackTrace ?? "");
                return RedirectToAction("Error", "Home", new { code = ERRORCODE, msg = ex.Message });
            }
        }

    }
}
