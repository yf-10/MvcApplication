using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcApplication.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public BaseController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._logger = Logger.GetInstance(configuration);
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



    }
}
