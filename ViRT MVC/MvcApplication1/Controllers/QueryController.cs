using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;
using MvcApplication1.Models;


namespace MvcApplication1.Controllers
{
    public class QueryController : Controller
    {
        Reliability querydata = new Reliability();
        public string getPipelines()
        {
            return JsonConvert.SerializeObject(querydata.GetAllPipelines());
        }

        public string getOverview()
        {
            return JsonConvert.SerializeObject(querydata.OverviewCalculate(Request.QueryString["pipeline"]));
        }

        public string getDatacenters()
        {
            return JsonConvert.SerializeObject(querydata.GetDataCenterLatLong());
        }

        public string getNetworks()
        {
            return JsonConvert.SerializeObject(querydata.GetAllNetworks());
        }

        public string getFarms()
        {
            return JsonConvert.SerializeObject(querydata.GetAllFarms());
        }

        public string getNetworkFarm()
        {
            Reliability test = new Reliability();

			test.ChangeDataCenter(Request.QueryString["datacen"]);

			DataTable table = test.CalculateDataCenterHeatMap();

            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }
    }
}
