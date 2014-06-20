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
            return "hey";
        }


    }
}
