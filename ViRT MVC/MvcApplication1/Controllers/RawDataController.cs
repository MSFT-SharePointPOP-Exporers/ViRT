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

        public ActionResult RawData()
        {
            Reliability percent = new Reliability();

            percent.ChangeDate((new DateTime(2014, 06, 18)), new DateTime(2014, 06, 19));
            String[] components = percent.getComponents("DynamicClaims");
            List<DataTable> allComponentsRawData = new List<DataTable>();

            foreach (var compName in components)
            {
                DataTable rawDataTable = percent.RawDataTable(compName);
                allComponentsRawData.Add(rawDataTable);
            }
            var table = JsonConvert.SerializeObject(allComponentsRawData);
            //ViewData["RawData"] = data;
            ViewBag.RawData = table;
            return View();
        }

    }
}
