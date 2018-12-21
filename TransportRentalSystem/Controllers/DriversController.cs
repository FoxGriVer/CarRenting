using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Models.DataBaseModels;
using DAL.Repository;
using Ext.Net;
using Ext.Net.MVC;

namespace TransportRentalSystem.Controllers {
    public class DriversController : Controller
    {
        DriverRepository driver_repository = new DriverRepository();

        // GET: Drivers
        public ActionResult Index()
        {
           return View(driver_repository.GetManyObjects());
        }

        public ActionResult HandleChanges( StoreDataHandler handler ) {
            List<Driver> drivers = handler.ObjectData<Driver>();
            string errorMessage = null;

            if( handler.Action == StoreAction.Create ) {
                foreach( Driver created in drivers ) {
                    driver_repository.InsertObject( created );
                }
            } else if( handler.Action == StoreAction.Destroy ) {
                foreach( Driver deleted in drivers ) {
                    driver_repository.DeleteObject( deleted.DRIVER_ID );
                }
            } else if( handler.Action == StoreAction.Update ) {
                foreach( Driver updated in drivers ) {
                    try {
                        driver_repository.UpdateObject( updated );
                    } catch( Exception e ) {
                        errorMessage = e.Message;
                    }
                }
            }

            if( errorMessage != null ) {
                return this.Store( errorMessage );
            }

            return handler.Action != StoreAction.Destroy ? ( ActionResult ) this.Store( drivers ) : ( ActionResult ) this.Content( "" );
        }
    }
}
