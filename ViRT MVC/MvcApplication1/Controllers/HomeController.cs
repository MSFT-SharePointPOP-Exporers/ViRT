using System;
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


    }
}
