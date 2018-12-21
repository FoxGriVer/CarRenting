using System.Web.Mvc;
using DAL.Repository;
using Ext.Net.MVC;

namespace TransportRentalSystem.Controllers
{
    public class JournalsController : Controller
    {
        JournOfAccountingRepository journal_repository = new JournOfAccountingRepository();

        // GET: Journals
        public ActionResult Index()
        {
            return View( journal_repository.GetManyObjects() );
        }

        public ActionResult GetData()
        {
            return this.Store( journal_repository.GetManyObjects() );
        }
    }
}
