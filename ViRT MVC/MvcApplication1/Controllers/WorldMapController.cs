using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MvcApplication1.Models;
using Newtonsoft.Json;

namespace MvcApplication1.Controllers
{
    public class WorldMapController : Controller
    {
        //
        // GET: /WorldMap/

        public ActionResult WorldMap()
        {
			Reliability world = new Reliability();

			world.ChangeDate((new DateTime(2014, 06, 18)), new DateTime(2014, 06, 19));

			DataTable worldLocs = world.GetDataCenterLatLong();

			var json = JsonConvert.SerializeObject(worldLocs);

			
			ViewBag.Index = json;

            return View();
        }

    }
}
