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
        public string sjson;

        public DataTable Test()
        {
            ReliabilityModels calc = new ReliabilityModels();
            DataTable test = calc.OverviewCalculate();
            return null;
        }

    }
}
