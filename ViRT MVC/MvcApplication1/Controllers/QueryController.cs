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

        public string getNetworkFarm()
        {
            Reliability test = new Reliability();
            Random random = new Random();
            DataTable table = new DataTable();
            DataTable networkTable = test.getNetworks("CH1");
            table.Columns.Add(new DataColumn("NetworkID", typeof(int)));
            table.Columns.Add(new DataColumn("Percentage", typeof(double)));
            table.Columns.Add(new DataColumn("Farms", typeof(DataTable)));
            foreach (DataRow row in networkTable.Rows)
            {
                DataRow network = table.NewRow();
                network["NetworkID"] = row["NetworkID"].ToString();
                network["Percentage"] = Convert.ToDouble(String.Format("{0:0.0000}", random.NextDouble() * 100));
                network["Farms"] = test.getFarms(Convert.ToInt32(row["NetworkID"]));
                table.Rows.Add(network);
            }
            //Console.WriteLine(JsonConvert.SerializeObject(test.getFarms(20)));
            table.AcceptChanges();
            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }


    }
}
