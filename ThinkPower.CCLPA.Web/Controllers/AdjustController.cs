using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Service;
using ThinkPower.CCLPA.Domain.VO;
using ThinkPower.CCLPA.Web.ActionModels;
using ThinkPower.CCLPA.Web.ViewModels;

namespace ThinkPower.CCLPA.Web.Controllers
{
    /// <summary>
    /// 專案臨調
    /// </summary>
    public class AdjustController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private PreAdjustService _preAdjustService;

        /// <summary>
        /// 臨調預審服務
        /// </summary>
        public PreAdjustService PreAdjService
        {
            get
            {
                if (_preAdjustService == null)
                {
                    _preAdjustService = new PreAdjustService();
                }

                return _preAdjustService;
            }
        }

        /// <summary>
        /// 系統錯誤提示訊息
        /// </summary>
        private readonly string _systemErrorMsg = "系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。";





        /// <summary>
        /// 預設首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 顯示預審名單匯入畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult PreAdjustImportPage()
        {
            return View("PreAdjustImport");
        }

        /// <summary>
        /// 進行預審名單驗證動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreAdjustValidate(PreAdjustValidateActionModel actionModel)
        {
            PreAdjustImportViewModel viewModel = null;
            string campaignId = null;
            string errorMessage = null;
            int? campaignDetailCount = null;
            bool executeImport = false;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                PreAdjService.UserInfo = new UserInfo()
                {
                    Id = Session["UserId"] as string,
                    Name = Session["UserName"] as string,
                };

                PreAdjustValidateResult result = PreAdjService.Validate(actionModel.CampaignId);

                campaignId = actionModel.CampaignId;
                errorMessage = result.ErrorMessage;
                campaignDetailCount = result.CampaignDetailCount;
            }
            catch (Exception e)
            {
                logger.Error(e);
                errorMessage = _systemErrorMsg;
            }

            viewModel = new PreAdjustImportViewModel()
            {
                CampaignId = campaignId,
                ErrorMessage = errorMessage,
                CampaignDetailCount = campaignDetailCount,
                ExecuteImport = executeImport,
            };

            return View("PreAdjustImport", viewModel);
        }

        /// <summary>
        /// 進行預審名單匯入動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreAdjustImport(PreAdjustImportActionModel actionModel)
        {
            PreAdjustImportViewModel viewModel = null;
            string errorMessage = null;
            bool executeImport = false;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                PreAdjService.UserInfo = new UserInfo()
                {
                    Id = Session["UserId"] as string,
                    Name = Session["UserName"] as string,
                };

                PreAdjService.Import(actionModel.CampaignId);

                executeImport = true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                errorMessage = _systemErrorMsg;
            }

            viewModel = new PreAdjustImportViewModel()
            {
                ErrorMessage = errorMessage,
                ExecuteImport = executeImport,
            };

            return View(viewModel);
        }
    }
}