using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IgorTeterkin.Tables.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFound()
        {
            // returns the response that page is not found
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}