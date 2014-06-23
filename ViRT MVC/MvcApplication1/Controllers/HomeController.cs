﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Data;
using MvcApplication1.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;



namespace MvcApplication1.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			Reliability world = new Reliability();

			DataTable worldLocs = world.GetDataCenterLatLong();
			world.ChangeDate((new DateTime(2014, 06, 18)), new DateTime(2014, 06, 19));
			world.ChangePipeline("DynamicClaims");
			var json = JsonConvert.SerializeObject(worldLocs);
			String[] dcs = world.GetAllDataCentersArray();

			DataTable dcPipeAverage = new DataTable();
			dcPipeAverage.Columns.Add("DataCenter", typeof(string));
			dcPipeAverage.Columns.Add("Percent", typeof(decimal));

			DataRow temp = dcPipeAverage.NewRow();

			for (int i = 0; i < dcs.Length; i++)
			{
				world.ChangeDataCenter(dcs[i]);
				temp["DataCenter"] = dcs[i];
				temp["Percent"] = world.CalculatePipeOverview();
				dcPipeAverage.Rows.Add(temp);
				temp = dcPipeAverage.NewRow();
			}

			var percentages = JsonConvert.SerializeObject(dcPipeAverage);

			ViewBag.AverageDCPercent = percentages;
			ViewBag.WorldMap = json;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult DCHM()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PercentData()
        {
            Reliability paramsPercent = new Reliability();

            paramsPercent.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));

            DataTable percentTable = paramsPercent.PipelineCalculate(Request.QueryString["pipeline"]);

            var json = JsonConvert.SerializeObject(percentTable, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            ViewBag.PercentData = json;

            return View();
        }

        public ActionResult RawData()
        {
            Reliability rawData = new Reliability();

            rawData.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            String[] components = rawData.getComponents(Request.QueryString["pipeline"]);
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
