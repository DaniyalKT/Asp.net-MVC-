using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using DataLayer;

namespace JanaFand.Areas.Admin.Controllers
{
    public class PrintFactorController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();   
        // GET: Admin/PrintFactor
        public ActionResult Print()
        {
            return View();
        }

        public ActionResult report()
        {
            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/Report.mrt"));
            report.Compile();
            report.RegBusinessObject("dt", db.FactorDetails.ToList());
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        public ActionResult viewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(HttpContext);
        }
    }
}