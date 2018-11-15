using NLog;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service;
using ThinkPower.CCLPA.Domain.VO;
using ThinkPower.CCLPA.Web.ActionModels;
using ThinkPower.CCLPA.Web.Models;
using ThinkPower.CCLPA.Web.ViewModels;

namespace ThinkPower.CCLPA.Web.Controllers
{
    /// <summary>
    /// 專案臨調
    /// </summary>
    public class AdjustController : BaseController
    {
        #region Private PropertyOrField

        #endregion



        #region Action Method

        /// <summary>
        /// 預設首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AdjustProcessViewModel viewModel = new AdjustProcessViewModel()
            {
                AdjustReasonSelectListItem = GetAdjustReasonCodeSelectItemList(),
                UseLocationSelectListItem = CreateUseLocationSelectItemList(),
                ManualAuthorizationSelectListItem = CreateManualAuthorizationSelectItemList(),
            };

            return View(_adjustProcessPage, viewModel);
        }

        /// <summary>
        /// 查詢歸戶ID
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Query(AdjustQueryActionModel actionModel)
        {
            AdjustVerifyResult adjustVerifyResult = null;

            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("Not AjaxRequest");
                }
                else if (actionModel == null)
                {
                    throw new ArgumentNullException(nameof(actionModel));
                }

                adjustVerifyResult = AdjService.Verify(actionModel.CustomerId);
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            return Json(adjustVerifyResult, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 取得臨調申請資料
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetApplicationData(AdjustApplicationActionModel actionModel)
        {
            AdjustProcessViewModel viewModel = new AdjustProcessViewModel()
            {
                AdjustReasonSelectListItem = GetAdjustReasonCodeSelectItemList(),
                UseLocationSelectListItem = CreateUseLocationSelectItemList(),
                ManualAuthorizationSelectListItem = CreateManualAuthorizationSelectItemList(),

                CustomerId = null,
                AdjustReasonRemark = null,
                SwipeAmount = null,
                AfterAdjustAmount = null,
                ValidDateStart = null,
                ValidDateEnd = null,
                PlaceOfGoingAbroad = null,
                TransferSupervisorReason = null,
            };

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException(nameof(actionModel));
                }

                AdjustApplication application = AdjService.GetApplicationData(actionModel.CustomerId);

                Customer customer = application.Customer;
                VipInfo vip = application.Vip;
                AdjustConditionValidateResult adjValidateResult = application.AdjustValidateResult;

                viewModel.JcicQueryDate = application.JcicSendQuery.JcicQueryDate;

                viewModel.ProjectAdjustResult = adjValidateResult?.ProjectResult;
                viewModel.ProjectRejectReason = adjValidateResult?.ProjectRejectReason;
                viewModel.GeneralAdjustResult = adjValidateResult?.EstimateResult;
                viewModel.GeneralRejectReason = adjValidateResult?.RejectReason;

                // TODO RG_PARA_M_ACTIVE
                //AdjustmentAmountCeiling = null,

                viewModel.Vip = new AdjustProcessVip() { MonthStarLevel = vip?.MonthStarLevel };

                viewModel.Customer = new AdjustProcessCustomer
                {
                    AccountId = customer.AccountId,
                    LiveCardCount = ((customer.LiveCardCount == null) || (customer.LiveCardCount <= 0)) ? "N" : "Y",
                    Status = customer.Status,
                    DataDate = customer.DataDate,
                    ChineseName = customer.ChineseName,
                    BirthDay = customer.BirthDay,
                    AboutDataStatus = customer.AboutDataStatus,
                    RiskLevel = customer.RiskLevel,
                    RiskRating = customer.RiskRating,
                    IssueDate = customer.IssueDate,
                    ClosingDay = customer.ClosingDay,
                    PayDeadline = customer.PayDeadline,
                    Vocation = customer.Vocation,
                    TelHome = customer.TelHome,
                    TelOffice = customer.TelOffice,
                    MobileTel = customer.MobileTel,
                    BillAddr = customer.BillAddr,
                    SystemAdjustRevFlag = customer.SystemAdjustRevFlag,
                    CreditLimit = customer.CreditLimit,
                    ClosingAmount = customer.ClosingAmount,
                    MinimumAmountPayable = customer.MinimumAmountPayable,
                    RecentPaymentDate = customer.RecentPaymentDate,
                    RecentPaymentAmount = customer.RecentPaymentAmount,
                    OfferAmount = customer.OfferAmount,
                    UnpaidTotal = customer.UnpaidTotal,
                    AuthorizedAmountNotAccount = customer.AuthorizedAmountNotAccount,
                    CcasUnderpaidAmount = customer.CcasUnderpaidAmount,
                    CcasUsabilityAmount = customer.CcasUsabilityAmount,
                    CcasUnderpaidRate = customer.CcasUnderpaidRate,
                };

                viewModel.AdjustLog = new AdjustLogModel
                {

                    AdjustReason = customer.AdjustReason,
                    AdjustArea = customer.AdjustArea,
                    AdjustStartDate = customer.AdjustStartDate,
                    AdjustEndDate = customer.AdjustEndDate,
                    AdjustEffectAmount = customer.AdjustEffectAmount,
                    AdjustLogList = application.AdjustLogList,
                };

                viewModel.NearlyYearRating = new NearlyYearRatingModel()
                {
                    LatestMnthList = new AdjustProcessLatestMnth[] {
                            new AdjustProcessLatestMnth(){ Month =1  , Rating = customer.Latest1Mnth },
                            new AdjustProcessLatestMnth(){ Month =2  , Rating = customer.Latest2Mnth },
                            new AdjustProcessLatestMnth(){ Month =3  , Rating = customer.Latest3Mnth },
                            new AdjustProcessLatestMnth(){ Month =4  , Rating = customer.Latest4Mnth },
                            new AdjustProcessLatestMnth(){ Month =5  , Rating = customer.Latest5Mnth },
                            new AdjustProcessLatestMnth(){ Month =6  , Rating = customer.Latest6Mnth },
                            new AdjustProcessLatestMnth(){ Month =7  , Rating = customer.Latest7Mnth },
                            new AdjustProcessLatestMnth(){ Month =8  , Rating = customer.Latest8Mnth },
                            new AdjustProcessLatestMnth(){ Month =9  , Rating = customer.Latest9Mnth },
                            new AdjustProcessLatestMnth(){ Month =10 , Rating = customer.Latest10Mnth },
                            new AdjustProcessLatestMnth(){ Month =11 , Rating = customer.Latest11Mnth },
                            new AdjustProcessLatestMnth(){ Month =12 , Rating = customer.Latest12Mnth },
                        },
                    ConsumeList = new AdjustProcessConsume[] {
                            new AdjustProcessConsume(){ Month =1  , Amount = customer.Consume1  },
                            new AdjustProcessConsume(){ Month =2  , Amount = customer.Consume2  },
                            new AdjustProcessConsume(){ Month =3  , Amount = customer.Consume3  },
                            new AdjustProcessConsume(){ Month =4  , Amount = customer.Consume4  },
                            new AdjustProcessConsume(){ Month =5  , Amount = customer.Consume5  },
                            new AdjustProcessConsume(){ Month =6  , Amount = customer.Consume6  },
                            new AdjustProcessConsume(){ Month =7  , Amount = customer.Consume7  },
                            new AdjustProcessConsume(){ Month =8  , Amount = customer.Consume8  },
                            new AdjustProcessConsume(){ Month =9  , Amount = customer.Consume9  },
                            new AdjustProcessConsume(){ Month =10 , Amount = customer.Consume10 },
                            new AdjustProcessConsume(){ Month =11 , Amount = customer.Consume11 },
                            new AdjustProcessConsume(){ Month =12 , Amount = customer.Consume12 },
                        },
                    PreCashList = new AdjustProcessPreCash[] {
                            new AdjustProcessPreCash(){ Month =1  , Amount = customer.PreCash1  },
                            new AdjustProcessPreCash(){ Month =2  , Amount = customer.PreCash2  },
                            new AdjustProcessPreCash(){ Month =3  , Amount = customer.PreCash3  },
                            new AdjustProcessPreCash(){ Month =4  , Amount = customer.PreCash4  },
                            new AdjustProcessPreCash(){ Month =5  , Amount = customer.PreCash5  },
                            new AdjustProcessPreCash(){ Month =6  , Amount = customer.PreCash6  },
                            new AdjustProcessPreCash(){ Month =7  , Amount = customer.PreCash7  },
                            new AdjustProcessPreCash(){ Month =8  , Amount = customer.PreCash8  },
                            new AdjustProcessPreCash(){ Month =9  , Amount = customer.PreCash9  },
                            new AdjustProcessPreCash(){ Month =10 , Amount = customer.PreCash10 },
                            new AdjustProcessPreCash(){ Month =11 , Amount = customer.PreCash11 },
                            new AdjustProcessPreCash(){ Month =12 , Amount = customer.PreCash12 },
                        },
                    CreditRatingList = new AdjustProcessCreditRating[] {
                            new AdjustProcessCreditRating(){ Month =1  , Rating = customer.CreditRating1  },
                            new AdjustProcessCreditRating(){ Month =2  , Rating = customer.CreditRating2  },
                            new AdjustProcessCreditRating(){ Month =3  , Rating = customer.CreditRating3  },
                            new AdjustProcessCreditRating(){ Month =4  , Rating = customer.CreditRating4  },
                            new AdjustProcessCreditRating(){ Month =5  , Rating = customer.CreditRating5  },
                            new AdjustProcessCreditRating(){ Month =6  , Rating = customer.CreditRating6  },
                            new AdjustProcessCreditRating(){ Month =7  , Rating = customer.CreditRating7  },
                            new AdjustProcessCreditRating(){ Month =8  , Rating = customer.CreditRating8  },
                            new AdjustProcessCreditRating(){ Month =9  , Rating = customer.CreditRating9  },
                            new AdjustProcessCreditRating(){ Month =10 , Rating = customer.CreditRating10 },
                            new AdjustProcessCreditRating(){ Month =11 , Rating = customer.CreditRating11 },
                            new AdjustProcessCreditRating(){ Month =12 , Rating = customer.CreditRating12 },
                        },
                };
            }
            catch (Exception e)
            {
                logger.Error(e);
            }


            return View(_adjustProcessPage, viewModel);
        }

        #endregion



        #region Private Method

        /// <summary>
        /// 取得調整原因代碼選單資料
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetAdjustReasonCodeSelectItemList()
        {
            IEnumerable<AdjustReasonCodeInfo> adjustReasonCodeList = new ParamterService().
                GetActiveAdjustReasonCode();

            if (adjustReasonCodeList == null || !adjustReasonCodeList.Any())
            {
                throw new InvalidOperationException($"{nameof(adjustReasonCodeList)} not found");
            }

            List<SelectListItem> selectItemList = new List<SelectListItem>();

            foreach (AdjustReasonCodeInfo adjustReasonCode in adjustReasonCodeList)
            {
                selectItemList.Add(new SelectListItem()
                {
                    Text = adjustReasonCode.Name,
                    Value = adjustReasonCode.Code
                });
            }

            return selectItemList;
        }

        /// <summary>
        /// 產生使用地點選單資料
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SelectListItem> CreateUseLocationSelectItemList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){ Value = "1", Text = "國外" },
                new SelectListItem(){ Value = "2", Text = "國內" },
                new SelectListItem(){ Value = "3", Text = "國內外" },
            };
        }

        /// <summary>
        /// 產生人工授權選單資料
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SelectListItem> CreateManualAuthorizationSelectItemList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){ Value = "Y", Text = "Y" },
                new SelectListItem(){ Value = "N", Text = "N" },
            };
        }

        #endregion
    }
}