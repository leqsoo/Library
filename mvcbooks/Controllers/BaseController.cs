using log4net;
using System.Web.Mvc;

namespace mvcbooks.Controllers
{
    public class BaseController : Controller, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                string loggerName = string.Format("{0}Controller.{1}", controller, action);

                LogManager.GetLogger(loggerName).Error(string.Empty, filterContext.Exception);
            }
        }
    }
}