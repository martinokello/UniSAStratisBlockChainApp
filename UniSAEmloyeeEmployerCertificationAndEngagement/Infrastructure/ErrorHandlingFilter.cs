using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using log4net;
using log4net.Repository.Hierarchy;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Infrastructure
{
    public class ErrorHandlingFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var log = LogManager.GetLogger(typeof(ErrorHandlingFilter)) as Logger;
            log.Log(log4net.Core.Level.All, filterContext.Exception.Message, filterContext.Exception);

            filterContext.ExceptionHandled = true;

            filterContext.HttpContext.Response.Redirect("~/Error/Error");
        }
    }
}