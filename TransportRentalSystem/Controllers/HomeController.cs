using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Repository;
using Models.DataBaseModels;

namespace TransportRentalSystem.Controllers
{
    public class HomeController : Controller
    {
        ActiveRepository active_repository = new ActiveRepository();
        DriverRepository driver_repository = new DriverRepository();
        CarRepository car_repository = new CarRepository();
        UserRepository user_repository = new UserRepository();
        List<Active> active_list = new List<Active>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Actives() {
            List<Active> dataFromActiveRepository = active_repository.GetManyObjects().ToList();
            List<Car> dataFromCarRepository = car_repository.GetManyObjects().ToList();
            List<int> usedCarsIds = new List<int>();
            foreach( var item in dataFromActiveRepository ) {
                if( !usedCarsIds.Contains( item.CAR_ID) ) {
                    usedCarsIds.Add( item.CAR_ID );
                }
                active_list.Add( item );
            }

            foreach( var item in dataFromCarRepository ) {
                if( !usedCarsIds.Contains( item.CAR_ID ) ) {
                    usedCarsIds.Add( item.CAR_ID );
                    active_list.Add( new Active
                    {
                        ACTIVE_ID = -1,
                        CAR_ID = item.CAR_ID,
                        DRIVER_ID = -1,
                        STATUS_ID = 2
                    } );
                }
            }
            return View( active_list );
        }

        [HttpPost]
        public JsonResult GetCars()
        {
            List<object> data = new List<object>();
            List<Car> dataFromCarRepository = car_repository.GetManyObjects().ToList();
            foreach( var item in dataFromCarRepository ) {
                data.Add( new { id = item.CAR_ID, value = item.MODEL } );
            }
            return Json( data, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public JsonResult GetDrivers()
        {
            List<object> data = new List<object>();
            List<Driver> dataFromDriverRepository = driver_repository.GetManyObjects().ToList();
            foreach( var item in dataFromDriverRepository ) {
                data.Add( new { id = item.DRIVER_ID, value = item.FIO } );
            }
            return Json( data, JsonRequestBehavior.AllowGet ); 
        }

        [HttpPost]
        public ActionResult CheckUser( User user )
        {
            var users = user_repository.GetManyObjects().ToList();
            foreach( var item in users )
            {
                if( user.LOGIN == item.LOGIN )
                {
                    if( user.PASSWORD == item.PASSWORD )
                    {
                        return RedirectToAction( "Actives" );
                    }
                    else
                    {
                        return RedirectToAction( "AccessDenied" );
                    }
                }
            }
            return RedirectToAction( "AccessDenied" );
        }
    }
}
