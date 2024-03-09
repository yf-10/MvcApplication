using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace MvcApplication.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        protected const string ERR401URL = "html/401.html";
        protected const string ERR404URL = "html/404.html";
        protected const string ERR500URL = "html/500.html";

        public BaseController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._logger = Logger.GetInstance(this._configuration);
        }

        // ---------------------------------------------------------------------
        // FILTER
        // ---------------------------------------------------------------------

        // Before Action
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RouteValueDictionary routeValue = filterContext.RouteData.Values;

            // Login
            if (routeValue["controller"]?.ToString() == "Login")
            {
                return;
            } else
            {
                // Login Check
                // ...
                this._logger.Debug("Login Check");
            }

            // Index
            if (routeValue["action"]?.ToString() == "Index")
            {
                this._logger.Debug("OnActionExecuting Filter");
            }

            base.OnActionExecuting(filterContext);
        }

        // After Action
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RouteValueDictionary routeValue = filterContext.RouteData.Values;

            // Login
            if (routeValue["controller"]?.ToString() == "Login")
            {
                return;
            }

            // Index
            if (routeValue["action"]?.ToString() == "Index")
            {
                this._logger.Debug("OnActionExecuted Filter");
            }

            base.OnActionExecuted(filterContext);
        }

        // ---------------------------------------------------------------------
        // CHECK
        // ---------------------------------------------------------------------
        private bool SessionCheck()
        {
            return true;
        }

        private bool TokenCheck()
        {
            return true;
        }

        private bool RoleCheck()
        {
            return true;
        }


    }
}
