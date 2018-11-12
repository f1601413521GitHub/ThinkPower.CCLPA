using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Service;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Logger logger = LogManager.GetCurrentClassLogger();
        protected PreAdjustService _preAdjustService;

        /// <summary>
        /// 臨調預審服務
        /// </summary>
        protected PreAdjustService PreAdjService
        {
            get
            {
                if (_preAdjustService == null)
                {
                    _preAdjustService = new PreAdjustService()
                    {
                        UserInfo = new UserInfo()
                        {
                            Id = Session["UserId"] as string,
                            Name = Session["UserName"] as string,
                        }
                    };
                }

                return _preAdjustService;
            }
        }

        /// <summary>
        /// 系統錯誤提示訊息
        /// </summary>
        protected readonly string _systemErrorMsg = "系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。";

        /// <summary>
        /// 預審名單匯入，檢視畫面名稱。
        /// </summary>
        protected readonly string _preAdjustImportPage = "PreAdjustImport";

        /// <summary>
        /// 預審名單處理，檢視畫面名稱。
        /// </summary>
        protected readonly string _preAdjustProcessPage = "PreAdjustProcess";

        /// <summary>
        /// 資料分頁每頁筆數
        /// </summary>
        protected readonly int _pageSize = 5;

        /// <summary>
        /// 專案臨調處理，檢視畫面名稱。
        /// </summary>
        protected readonly string _adjustProcessPage = "AdjustProcess";



        /// <summary>
        /// 檢核使用者操作權限
        /// </summary>
        /// <returns></returns>
        protected bool CheckUserPermission()
        {
            bool canExecuteOperation = false;

            var serviece = new UserService()
            {
                UserInfo = new UserInfo()
                {
                    Id = Session["UserId"] as string,
                    Name = Session["UserName"] as string,
                }
            };

            AdjustPermission permission = serviece.GetUserPermission();

            if (!String.IsNullOrEmpty(permission.AdjustExecute) && permission.AdjustExecute == "Y")
            {
                canExecuteOperation = true;
            }

            return canExecuteOperation;
        }
    }
}