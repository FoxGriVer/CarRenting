using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Models;

namespace TransportRentalSystem.Controllers
{
    [DirectController(IDMode = DirectMethodProxyIDMode.None)]
    public class BasicController : Controller
    {
        public ActionResult Index()
        {
            return View(BasicModel.Events);
        }

        public ActionResult SubmitData(string data)
        {
            List<EventModel> events = JSON.Deserialize<List<EventModel>>(data);

            return new System.Web.Mvc.PartialViewResult
            {
                ViewName = "EventsViewer",
                ViewBag =
                {
                    Events = events
                }
            };
        }

        //[DirectMethod(Namespace = "CompanyX")]
        [DirectMethod]
        public ActionResult ShowMsg(string msg)
        {
            X.Msg.Notify("Message", msg).Show();

            return this.Direct();
        }

        [DirectMethod]
        public ActionResult Delete()
        {
            //X.Msg.Notify("Message", msg).Show();

            return this.Direct();
        }

        //[DirectMethod(Namespace = "ExtNetExample")]
        [DirectMethod(Namespace = "Comp")]
        public ActionResult GetDateTime(int timeDiff)
        {
            //X.Msg.Notify("Message", msg).Show();
            int temp = timeDiff;

            return this.Direct();
        }
    }
}
