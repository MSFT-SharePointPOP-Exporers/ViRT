using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Data;
using MvcApplication1.Models;
using Newtonsoft.Json;


namespace MvcApplication1.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
			
			Reliability world = new Reliability();

			DataTable worldLocs = world.GetDataCenterLatLong();

			var json = JsonConvert.SerializeObject(worldLocs);

			ViewBag.WorldMap = json;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}
