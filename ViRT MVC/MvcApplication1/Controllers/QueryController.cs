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

        public string Test()
        {
            Reliability test = new Reliability();
            return JsonConvert.SerializeObject(test.OverviewCalculate("UserLogin"));
        }

    }
}
