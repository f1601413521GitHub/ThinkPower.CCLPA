using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThinkPower.CCLPA.Web.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            _logger.Debug("Debug");
            _logger.Info("Info");
            _logger.Warn("Warn");
            _logger.Error("Error");
            _logger.Fatal("Fatal");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}