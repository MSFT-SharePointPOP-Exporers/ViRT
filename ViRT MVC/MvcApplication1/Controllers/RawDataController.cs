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
    public class RawDataController : Controller
    {
        //
        // GET: /RawData/

        public ActionResult RawData(DateTime start, DateTime end, string pipeline, string datacen, int network, int farm)
        {
            Reliability rawData = new Reliability(datacen, network, farm, pipeline, start, end);

            rawData.ChangeDate((new DateTime(2014, 06, 18)), new DateTime(2014, 06, 19));
            String[] components = rawData.getComponents(pipeline);
            List<DataTable> allComponentsRawData = new List<DataTable>();

            foreach (var compName in components)
            {
                DataTable rawDataTable = rawData.RawDataTable(compName);
                allComponentsRawData.Add(rawDataTable);
            }
            var table = JsonConvert.SerializeObject(allComponentsRawData, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //ViewData["RawData"] = data;
            ViewBag.RawData = table;
            ViewBag.RawTitles = JsonConvert.SerializeObject(components);

            return View();
        }

    }
}
