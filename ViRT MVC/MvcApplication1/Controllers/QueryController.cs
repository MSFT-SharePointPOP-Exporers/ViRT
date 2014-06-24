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
        Reliability test = new Reliability();
        public string getPipelines()
        {
            return JsonConvert.SerializeObject(test.GetAllPipelines());
        }

        public string getOverview()
        {
            return JsonConvert.SerializeObject(test.OverviewCalculate(Request.QueryString["pipeline"]));
        }

        public string getDatacenters()
        {
            return JsonConvert.SerializeObject(test.GetDataCenterLatLong());
        }

        public string getNetworks()
        {
            return JsonConvert.SerializeObject(test.GetAllNetworks());
        }

        public string getFarms()
        {
            return JsonConvert.SerializeObject(test.GetAllFarms());
        }

        public string getNetworkFarm()
        {
            Reliability test = new Reliability();
            Random rand = new Random();
            DataTable table = new DataTable();
            DataTable networkTable = test.getNetworks("CH1");
            table.Columns.Add(new DataColumn("NetworkID", typeof(int)));
            table.Columns.Add(new DataColumn("Percentage", typeof(double)));
            table.Columns.Add(new DataColumn("Farms", typeof(DataTable)));
            DataRow network;
            foreach (DataRow row in networkTable.Rows)
            {
                network = table.NewRow();
                network["NetworkID"] = row["NetworkID"];
                network["Percentage"] = rand.NextDouble() * 100;
                network["Farms"] = test.getFarms((int)row["NetworkID"]);
                table.Rows.Add(network);
            }
            table.AcceptChanges();
            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }


    }
}
