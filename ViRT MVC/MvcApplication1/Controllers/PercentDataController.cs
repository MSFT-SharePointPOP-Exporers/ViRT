﻿using System;
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

        public ActionResult PercentData(DateTime start, DateTime end, string pipeline, string datacen, int network, int farm)
        {
            Reliability paramsPercent = new Reliability(datacen, network, farm, pipeline, start, end);

            paramsPercent.ChangeDate((new DateTime(2014, 06, 18)), new DateTime(2014, 06, 19));

            DataTable percentTable = paramsPercent.PipelineCalculate(pipeline);

            var json = JsonConvert.SerializeObject(percentTable, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            
            ViewBag.PercentData = json;

            return View();
        }

    }
}
