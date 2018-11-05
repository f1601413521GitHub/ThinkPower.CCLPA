﻿using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Condition;
using ThinkPower.CCLPA.Domain.Entity;
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
        #region Private PropertyOrField

        private Logger logger = LogManager.GetCurrentClassLogger();
        private PreAdjustService _preAdjustService;

        /// <summary>
        /// 臨調預審服務
        /// </summary>
        private PreAdjustService PreAdjService
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
        private readonly string _systemErrorMsg = "系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。";

        /// <summary>
        /// 預審名單匯入，檢視畫面名稱。
        /// </summary>
        private readonly string _preAdjustImportPage = "PreAdjustImport";

        /// <summary>
        /// 預審名單處理，檢視畫面名稱。
        /// </summary>
        private readonly string _preAdjustProcessPage = "PreAdjustProcess";

        #endregion


        #region Action Method

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
            return View(_preAdjustImportPage);
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

            return View(_preAdjustImportPage, viewModel);
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











        /// <summary>
        /// 顯示預審名單處理畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult PreAdjustProcessPage()
        {
            return View(_preAdjustProcessPage);
        }

        /// <summary>
        /// 進行預審名單查詢動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreAdjustQuery(PreAdjustQueryActionModel actionModel)
        {
            PreAdjustProcessViewModel viewModel = null;
            List<PreAdjustEntity> queryResult = null;
            string errorMessage = null;
            bool canExecuteOperation = false;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                queryResult = QueryPreAdjust(new PreAdjustCondition()
                {
                    PageIndex = null,
                    PagingSize = null,
                    CloseDate = DateTime.Now,
                    CcasReplyCode = null,
                    CustomerId = actionModel.CustomerId,
                    CampaignId = null,
                });


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
            }
            catch (Exception e)
            {
                logger.Error(e);
                errorMessage = _systemErrorMsg;
            }


            viewModel = new PreAdjustProcessViewModel()
            {
                CustomerId = (actionModel == null) ? null : actionModel.CustomerId,
                PreAdjustList = queryResult,
                ErrorMessage = errorMessage,
                CanExecuteOperation = canExecuteOperation,
            };

            return View(_preAdjustProcessPage, viewModel);
        }

        /// <summary>
        /// 進行預審名單刪除動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreAdjustDeleteNotEffect(PreAdjustDeleteActionModel actionModel)
        {
            PreAdjustResult result = null;

            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("Not ajax request");
                }
                else if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                var preAdjustInfo = new PreAdjustInfo()
                {
                    PreAdjustList = actionModel.PreAdjustList,
                    Remark = actionModel.Remark,
                };

                result = PreAdjService.DeleteNotEffect(preAdjustInfo);
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            return Json(result, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 進行預審名單刪除動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreAdjustDeleteEffect(PreAdjustDeleteActionModel actionModel)
        {
            PreAdjustResult result = null;

            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("Not ajax request");
                }
                else if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                var preAdjustInfo = new PreAdjustInfo()
                {
                    PreAdjustList = actionModel.PreAdjustList,
                    Remark = actionModel.Remark,
                };

                result = PreAdjService.DeleteEffect(preAdjustInfo);
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            return Json(result, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 進行預審名單同意動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreAdjustAgree(PreAdjustAgreeActionModel actionModel)
        {

            PreAdjustResult result = null;

            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("Not ajax request");
                }
                else if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                var preAdjustInfo = new PreAdjustInfo()
                {
                    PreAdjustList = actionModel.PreAdjustList,
                };

                result = PreAdjService.Agree(preAdjustInfo);
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            return Json(result, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 進行預審名單強制同意動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreAdjustForceAgree(PreAdjustForceAgreeActionModel actionModel)
        {
            PreAdjustResult result = null;

            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("Not ajax request");
                }
                if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }



                var preAdjustInfo = new PreAdjustInfo()
                {
                    PreAdjustList = actionModel.PreAdjustList,
                };

                result = PreAdjService.ForceAgree(preAdjustInfo, actionModel.NeedValidate);
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            return Json(result, "application/json", Encoding.UTF8);
        }


        #endregion





        #region Private Method

        /// <summary>
        /// 查詢預審名單
        /// </summary>
        /// <param name="condition">資料查詢條件</param>
        /// <returns></returns>
        private List<PreAdjustEntity> QueryPreAdjust(PreAdjustCondition condition)
        {
            List<PreAdjustEntity> queryResult = null;

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            IEnumerable<PreAdjustEntity> notEffect = PreAdjService.Query(condition);

            condition.CcasReplyCode = "00";
            IEnumerable<PreAdjustEntity> effect = PreAdjService.Query(condition);

            queryResult = new List<PreAdjustEntity>();
            queryResult.AddRange(notEffect);
            queryResult.AddRange(effect);

            return queryResult;
        }

        #endregion
    }
}