using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThinkPower.CCLPA.Web.Controllers
{
    /// <summary>
    /// 專案臨調
    /// </summary>
    public class TemporaryAdjustmentController : Controller
    {
        /// <summary>
        /// 預設首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}