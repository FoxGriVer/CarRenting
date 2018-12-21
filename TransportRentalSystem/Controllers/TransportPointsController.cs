using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Models.DataBaseModels;
using DAL.Repository;
using Ext.Net;
using Ext.Net.MVC;

namespace TransportRentalSystem.Controllers
{
    public class TransportPointsController : Controller
    {
        DestinationPointRepository object_repository = new DestinationPointRepository();

        // GET: 
        public ActionResult Index()
        {
            return View(object_repository.GetManyObjects());
        }

        public ActionResult HandleChanges(StoreDataHandler handler)
        {
            List<TransportPoint> objectDB = handler.ObjectData<TransportPoint>();
            string errorMessage = null;

            if (handler.Action == StoreAction.Create)
            {
                foreach (TransportPoint created in objectDB)
                {
                    object_repository.InsertObject(created);
                }
            }
            else if (handler.Action == StoreAction.Destroy)
            {
                foreach (TransportPoint deleted in objectDB)
                {
                    object_repository.DeleteObject(deleted.DESTINATION_POINT_ID);
                }
            }
            else if (handler.Action == StoreAction.Update)
            {
                foreach (TransportPoint updated in objectDB)
                {
                    try
                    {
                        object_repository.UpdateObject(updated);
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            if (errorMessage != null)
            {
                return this.Store(errorMessage);
            }

            return handler.Action != StoreAction.Destroy ? (ActionResult)this.Store(objectDB) : (ActionResult)this.Content("");
        }
    }
}
