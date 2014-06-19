using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MvcApplication1.Models;
using Newtonsoft.Json;

namespace MvcApplication1.Controllers
{
    public class PercentDataController : Controller
    {
        //
        // GET: /PercentData/

        public ActionResult PercentData()
        {
            Reliability percent = new Reliability();
            percent.ChangeDate((new DateTime(2014, 06, 18)), new DateTime(2014, 06, 19));
            DataTable percentTable = percent.PipelineCalculate("DynamicClaims");
            var json = JsonConvert.SerializeObject(percentTable);
            ViewBag.PercentData = json;

            return View();
        }

    }
}
