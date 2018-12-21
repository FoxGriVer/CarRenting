using System.Web.Mvc;
using DAL.Repository;
using Ext.Net.MVC;

namespace TransportRentalSystem.Controllers
{
    public class JournalReportController : Controller
    {
        JournReportRepository journal_repository = new JournReportRepository();

        // GET: VJOURNAL
        public ActionResult Index()
        {
            return View(journal_repository.GetManyObjects());
        }

        public ActionResult GetData()
        {
            return this.Store(journal_repository.GetManyObjects());
        }
    }
}
