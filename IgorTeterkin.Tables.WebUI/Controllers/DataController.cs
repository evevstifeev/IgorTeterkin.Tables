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
        [ChildActionOnly]
        //[HttpGet]
        public JsonResult GetData()
        {
            var path = HostingEnvironment.MapPath("~/App_Data/test.json");

            string data = string.Empty;
            dynamic DynamicData = data;
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    data = r.ReadToEnd();
                    DynamicData = JsonConvert.DeserializeObject(data);
                    
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                // Handle expeption here
            }
            return Json(DynamicData, JsonRequestBehavior.AllowGet);
        } 

        [ChildActionOnly]
        [HttpGet]
        public async Task<string> GetHtml()
        {
            HtmlWeb web = new HtmlWeb();

            var currentDate = DateTime.Now.Date.ToString("dd.MM.yyyy");

            var htmlDoc = web.Load($"https://www.cbr.ru/currency_base/daily/?UniDbQuery.Posted=True&UniDbQuery.To={DateTime.Now}");

            var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='data']");
            var targetTr = table.ChildNodes[1].ChildNodes.First(tr => tr.ChildNodes.Any(td => td.InnerHtml == "USD"));
            var targetValue = targetTr.ChildNodes[targetTr.ChildNodes.Count-2].InnerText;
           
            return await Task.FromResult(targetValue);

        }
    }
}