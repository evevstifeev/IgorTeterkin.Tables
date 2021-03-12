using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace IgorTeterkin.Tables.WebUI.Controllers
{
    /// <summary>
    /// Not a pure MVC approach since this controller has logic and data access, 
    /// .js contains some logic too, so this is only for an example
    /// </summary>
    public class DataController : Controller
    {
        // GET: Data
        /// <summary>
        /// Displays the page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Reads the .json file and return the data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetData()
        {
            // Cannot apply ChildActionOnly attribute since this will prevent the access from .js script
            // , so the address "/Data/GetData" will be accessible for the client and will return the result json.
            // The best approach in this case would be to parse json to the models of the tables, but since it is
            // as js, html and css skills demo I am doing this intentionally using JS.
            // Probebly should get path as parameter but for test example this will do 
            var path = HostingEnvironment.MapPath("~/App_Data/test.json");

            string data = string.Empty;
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    data = r.ReadToEnd();
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                // Return json with only success = false 
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, generalData = data, currencyRatio = GetCurrencyRatio().Result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns currency ratio from www.cbr.ru for current date
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [HttpGet]
        public async Task<string> GetCurrencyRatio()
        {
            HtmlWeb web = new HtmlWeb();

            // Pass current date to the required format (current locale format may vary)
            var currentDate = DateTime.Now.Date.ToString("dd.MM.yyyy");

            try
            {
                // Download page with current date passed
                var htmlDoc = web.Load($"https://www.cbr.ru/currency_base/daily/?UniDbQuery.Posted=True&UniDbQuery.To={DateTime.Now}");

                // Find the required value (assuming that the page structure will mostly not change)
                var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='data']");
                var targetTr = table.ChildNodes[1].ChildNodes.First(tr => tr.ChildNodes.Any(td => td.InnerHtml == "USD"));
                var targetValue = targetTr.ChildNodes[targetTr.ChildNodes.Count - 2].InnerText;

                // Return result
                return await Task.FromResult(targetValue);
            }
            catch (HtmlWebException)
            {
                // The remote address was incorrect. Return empty result
                return await Task.FromResult(string.Empty);
            }
        }
    }
}