using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;


namespace MvcApplication1.Controllers
{
    public class QueryController : Controller
    {
        public string sjson;

        public string Test()
        {
            return sjson;
        }

    }
}
