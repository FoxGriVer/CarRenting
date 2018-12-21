using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Repository;

namespace TransportRentalSystem.Controllers
{
    public class AboutCarsController : Controller
    {
        CarRepository repository = new CarRepository();
        // GET: AboutCars
        public ActionResult Index()
        {
            return View();
        }
    }
}