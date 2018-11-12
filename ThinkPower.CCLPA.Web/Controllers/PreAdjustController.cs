using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Condition;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.VO;
using ThinkPower.CCLPA.Web.ActionModels;
using ThinkPower.CCLPA.Web.ViewModels;

namespace ThinkPower.CCLPA.Web.Controllers
{
    public class PreAdjustController : BaseController
    {
        /// <summary>
        /// 顯示預審名單匯入畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(_preAdjustImportPage);
        }

        /// <summary>
        /// 進行預審名單驗證動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Validate(PreAdjustValidateActionModel actionModel)
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
                    throw new ArgumentNullException(nameof(actionModel));
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
        public ActionResult Import(PreAdjustImportActionModel actionModel)
        {
            PreAdjustImportViewModel viewModel = null;
            string errorMessage = null;
            bool executeImport = false;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException(nameof(actionModel));
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

            return View(_preAdjustImportPage, viewModel);
        }











        /// <summary>
        /// 進行預審名單載入動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Load(PreAdjustLoadActionModel actionModel)
        {
            PreAdjustProcessViewModel viewModel = null;
            string errorMessage = null;
            bool canExecuteOperation = false;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException(nameof(actionModel));
                }
                else if (actionModel.NotEffectPageIndex == 0)
                {
                    throw new ArgumentNullException(nameof(actionModel.NotEffectPageIndex));
                }
                else if (actionModel.EffectPageIndex == 0)
                {
                    throw new ArgumentNullException(nameof(actionModel.EffectPageIndex));
                }



                var currentTime = DateTime.Now;

                var notEffectCondition = new PreAdjustCondition()
                {
                    PageIndex = null,
                    PagingSize = null,
                    CloseDate = currentTime,
                    CcasReplyCode = null,
                    CustomerId = actionModel.CustomerId,
                    CampaignId = null,
                };

                int notEffectTotalCount = PreAdjService.Count(notEffectCondition);


                notEffectCondition.PageIndex = actionModel.NotEffectPageIndex;
                notEffectCondition.PagingSize = _pageSize;
                IEnumerable<PreAdjustEntity> notEffectPreAdjustList = PreAdjService.
                    Query(notEffectCondition);



                var effectCondition = new PreAdjustCondition()
                {
                    PageIndex = null,
                    PagingSize = null,
                    CloseDate = currentTime,
                    CcasReplyCode = "00",
                    CustomerId = actionModel.CustomerId,
                    CampaignId = null,
                };

                int effectTotalCount = PreAdjService.Count(effectCondition);

                effectCondition.PageIndex = actionModel.EffectPageIndex;
                effectCondition.PagingSize = _pageSize;
                IEnumerable<PreAdjustEntity> effectPreAdjsutList = PreAdjService.Query(effectCondition);

                canExecuteOperation = CheckUserPermission();

                viewModel = new PreAdjustProcessViewModel()
                {
                    ErrorMessage = errorMessage,
                    CanExecuteOperation = canExecuteOperation,

                    CustomerId = String.IsNullOrEmpty(actionModel.CustomerId) ? null : actionModel.CustomerId,
                    NotEffectPageIndex = actionModel.NotEffectPageIndex,
                    EffectPageIndex = actionModel.EffectPageIndex,

                    NotEffectPreAdjustList = new StaticPagedList<PreAdjustEntity>(notEffectPreAdjustList,
                        actionModel.NotEffectPageIndex, _pageSize, notEffectTotalCount),

                    EffectPreAdjustList = new StaticPagedList<PreAdjustEntity>(effectPreAdjsutList,
                        actionModel.EffectPageIndex, _pageSize, effectTotalCount),
                };
            }
            catch (Exception e)
            {
                logger.Error(e);
                errorMessage = _systemErrorMsg;
            }

            if (viewModel == null)
            {
                viewModel = new PreAdjustProcessViewModel()
                {
                    ErrorMessage = errorMessage
                };
            }

            return View(_preAdjustProcessPage, viewModel);
        }


        /// <summary>
        /// 進行預審名單刪除動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteNotEffect(PreAdjustDeleteActionModel actionModel)
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
                    throw new ArgumentNullException(nameof(actionModel));
                }
                else if (!CheckUserPermission())
                {
                    throw new InvalidOperationException("User does not permissions.");
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
        public ActionResult DeleteEffect(PreAdjustDeleteActionModel actionModel)
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
                    throw new ArgumentNullException(nameof(actionModel));
                }
                else if (!CheckUserPermission())
                {
                    throw new InvalidOperationException("User does not permissions.");
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
        public ActionResult Agree(PreAdjustAgreeActionModel actionModel)
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
                    throw new ArgumentNullException(nameof(actionModel));
                }
                else if (!CheckUserPermission())
                {
                    throw new InvalidOperationException("User does not permissions.");
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
        public ActionResult ForceAgree(PreAdjustForceAgreeActionModel actionModel)
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
                    throw new ArgumentNullException(nameof(actionModel));
                }
                else if (!CheckUserPermission())
                {
                    throw new InvalidOperationException("User does not permissions.");
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


    }
}