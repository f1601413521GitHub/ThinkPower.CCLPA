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
                AdjustReason = GetAdjustReasonCodeSelectItemList(),
                UseLocation = CreateUseLocationSelectItemList(),
                ManualAuthorization = CreateManualAuthorizationSelectItemList(),
            };

            viewModel = SetTestData(viewModel);

            return View(_adjustProcessPage, viewModel);
        }

        /// <summary>
        /// 設定測試資料
        /// </summary>
        /// <param name="viewModel">臨調檢視模型</param>
        /// <returns></returns>
        private AdjustProcessViewModel SetTestData(AdjustProcessViewModel viewModel)
        {
            viewModel.DataDate = "2016/03/10";
            viewModel.MonthStarLevel = 3;
            viewModel.AccountId = "H142700334";
            viewModel.LiveCardCount = "Y";
            viewModel.Status = "N";
            viewModel.ChineseName = "王小明";
            viewModel.BirthDay = new DateTime(1972, 5, 19);
            viewModel.AboutDataStatus = "Y";
            viewModel.RiskLevel = "3";
            viewModel.RiskRating = "002";
            viewModel.IssueDate = new DateTime(2011, 4, 1);
            viewModel.ClosingDay = "20";
            viewModel.PayDeadline = "05";
            viewModel.Vocation = "M";
            viewModel.TelHome = "03-12345678";
            viewModel.TelOffice = "03-12345678";
            viewModel.MobileTel = "09123456789";
            viewModel.BillAddr = "114台北市內湖區內湖路一段360巷15號6號";
            viewModel.NearlyYearMaxConsumptionAmount = 1201;
            viewModel.SystemAdjustRevFlag = "N";
            viewModel.CreditLimit = 40000;
            viewModel.ClosingAmount = 9000;
            viewModel.MinimumAmountPayable = 900;
            viewModel.ShowAfterAdjustAmount = 40000;
            viewModel.RecentPaymentDate = new DateTime(2016, 3, 29);
            viewModel.RecentPaymentAmount = 10000;
            viewModel.OfferAmount = 77;
            viewModel.UnpaidTotal = 0;
            viewModel.AuthorizedAmountNotAccount = 4000;
            viewModel.CcasUnderpaidAmount = 20000;
            viewModel.CcasUsabilityAmount = 20000;
            viewModel.CcasUnderpaidRate = 33.50M;


            viewModel.Latest1Mnth = "1";
            viewModel.Latest2Mnth = "2";
            viewModel.Latest3Mnth = "3";
            viewModel.Latest4Mnth = "4";
            viewModel.Latest5Mnth = "1";
            viewModel.Latest6Mnth = "2";
            viewModel.Latest7Mnth = "3";
            viewModel.Latest8Mnth = "4";
            viewModel.Latest9Mnth = "1";
            viewModel.Latest10Mnth = "2";
            viewModel.Latest11Mnth = "3";
            viewModel.Latest12Mnth = "4";

            viewModel.Consume1 = 101;
            viewModel.Consume2 = 201;
            viewModel.Consume3 = 301;
            viewModel.Consume4 = 401;
            viewModel.Consume5 = 501;
            viewModel.Consume6 = 601;
            viewModel.Consume7 = 701;
            viewModel.Consume8 = 801;
            viewModel.Consume9 = 901;
            viewModel.Consume10 = 1001;
            viewModel.Consume11 = 1101;
            viewModel.Consume12 = 1201;

            viewModel.PreCash1 = 0;
            viewModel.PreCash2 = 0;
            viewModel.PreCash3 = 0;
            viewModel.PreCash4 = 0;
            viewModel.PreCash5 = 0;
            viewModel.PreCash6 = 0;
            viewModel.PreCash7 = 0;
            viewModel.PreCash8 = 0;
            viewModel.PreCash9 = 0;
            viewModel.PreCash10 = 0;
            viewModel.PreCash11 = 0;
            viewModel.PreCash12 = 0;

            viewModel.CreditRating1 = "1";
            viewModel.CreditRating2 = "1";
            viewModel.CreditRating3 = "1";
            viewModel.CreditRating4 = "2";
            viewModel.CreditRating5 = "2";
            viewModel.CreditRating6 = "2";
            viewModel.CreditRating7 = "3";
            viewModel.CreditRating8 = "3";
            viewModel.CreditRating9 = "3";
            viewModel.CreditRating10 = "4";
            viewModel.CreditRating11 = "4";
            viewModel.CreditRating12 = "4";


            viewModel.AdjustReasonRemark = "原因";
            viewModel.JcicQueryDate = "2018/01/01";
            viewModel.AdjustmentAmountCeiling = "99,999";
            viewModel.SwipeAmount = "12,345";
            viewModel.AfterAdjustAmount = "45,678";
            viewModel.ValidDateStart = "2018/01/01";
            viewModel.ValidDateEnd = "2019/01/01";
            viewModel.PlaceOfGoingAbroad = "日本";


            // TODO Call SP_ELGB_PAD02
            viewModel.ProjectAdjustResult = "預審臨調拒絕";
            viewModel.ProjectRejectReason = "RI_01、RI_02、RI_03、RI_04、RI_05";
            viewModel.GeneralAdjustResult = "預審臨調拒絕";
            viewModel.GeneralRejectReason = "RI_01、RI_03、RI_05";
            viewModel.TransferSupervisorReason = "TEST";

            viewModel.LatestAdjustReason = "原因A";
            viewModel.LatestAdjustArea = "區域B";
            viewModel.LatestAdjustStartDate = "2018/01/02";
            viewModel.LatestAdjustEndDate = "2019/01/02";
            viewModel.LatestAdjustEffectAmount = 12345;
            viewModel.LatestAdjustInfo = new List<AdjustEntity>()
            {
                new AdjustEntity() { ApplyDate = "2018/10/10", ApplyTime = "14:13:12", Reason = "(08)加油", UseSite = "1.國外", Place = "韓國", AdjustDateStart = "2018/01/01", AdjustDateEnd = "2018/02/02", ApproveAmount = 12345, ApproveResult = "申請中", ProcessDate = "2018/03/01", ProcessTime = "12:13:14", RejectReason = "拒絕原因", CcasCode = "01", CcasStatus = "CCAS異常狀態", CcasDateTime = "2018/01/12" },
                new AdjustEntity() { ApplyDate = "2018/10/10", ApplyTime = "14:13:12", Reason = "(08)加油", UseSite = "1.國外", Place = "韓國", AdjustDateStart = "2018/01/01", AdjustDateEnd = "2018/02/02", ApproveAmount = 12345, ApproveResult = "申請中", ProcessDate = "2018/03/01", ProcessTime = "12:13:14", RejectReason = "拒絕原因", CcasCode = "01", CcasStatus = "CCAS異常狀態", CcasDateTime = "2018/01/12" },
                new AdjustEntity() { ApplyDate = "2018/10/10", ApplyTime = "14:13:12", Reason = "(08)加油", UseSite = "1.國外", Place = "韓國", AdjustDateStart = "2018/01/01", AdjustDateEnd = "2018/02/02", ApproveAmount = 12345, ApproveResult = "申請中", ProcessDate = "2018/03/01", ProcessTime = "12:13:14", RejectReason = "拒絕原因", CcasCode = "01", CcasStatus = "CCAS異常狀態", CcasDateTime = "2018/01/12" },
                new AdjustEntity() { ApplyDate = "2018/10/10", ApplyTime = "14:13:12", Reason = "(08)加油", UseSite = "1.國外", Place = "韓國", AdjustDateStart = "2018/01/01", AdjustDateEnd = "2018/02/02", ApproveAmount = 12345, ApproveResult = "申請中", ProcessDate = "2018/03/01", ProcessTime = "12:13:14", RejectReason = "拒絕原因", CcasCode = "01", CcasStatus = "CCAS異常狀態", CcasDateTime = "2018/01/12" },
                new AdjustEntity() { ApplyDate = "2018/10/10", ApplyTime = "14:13:12", Reason = "(08)加油", UseSite = "1.國外", Place = "韓國", AdjustDateStart = "2018/01/01", AdjustDateEnd = "2018/02/02", ApproveAmount = 12345, ApproveResult = "申請中", ProcessDate = "2018/03/01", ProcessTime = "12:13:14", RejectReason = "拒絕原因", CcasCode = "01", CcasStatus = "CCAS異常狀態", CcasDateTime = "2018/01/12" },
            };

            return viewModel;
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
            AdjustProcessViewModel viewModel = null;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException(nameof(actionModel));
                }

                AdjustApplicationInfo adjustApplicationData = AdjService.
                    GetApplicationData(actionModel.CustomerId);

                CustomerInfo customer = adjustApplicationData.Customer;

                viewModel = new AdjustProcessViewModel()
                {
                    AccountId = customer.AccountId,
                    LiveCardCount = ((customer.LiveCardCount == null) ||
                        (customer.LiveCardCount <= 0)) ? "N" : "Y",
                    Status = customer.Status,
                    DataDate = customer.DataDate,
                    MonthStarLevel = adjustApplicationData.Vip.MonthStarLevel,
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
                    NearlyYearMaxConsumptionAmount = new List<decimal?>() {
                        customer.Consume1, customer.Consume2, customer.Consume3, customer.Consume4,
                        customer.Consume5, customer.Consume6, customer.Consume7, customer.Consume8,
                        customer.Consume9, customer.Consume10, customer.Consume11, customer.Consume12,
                    }.Max(),
                    SystemAdjustRevFlag = customer.SystemAdjustRevFlag,
                    CreditLimit = customer.CreditLimit,
                    ClosingAmount = customer.ClosingAmount,
                    MinimumAmountPayable = customer.MinimumAmountPayable,
                    Latest1Mnth = customer.Latest1Mnth,
                    Latest2Mnth = customer.Latest2Mnth,
                    Latest3Mnth = customer.Latest3Mnth,
                    Latest4Mnth = customer.Latest4Mnth,
                    Latest5Mnth = customer.Latest5Mnth,
                    Latest6Mnth = customer.Latest6Mnth,
                    Latest7Mnth = customer.Latest7Mnth,
                    Latest8Mnth = customer.Latest8Mnth,
                    Latest9Mnth = customer.Latest9Mnth,
                    Latest10Mnth = customer.Latest10Mnth,
                    Latest11Mnth = customer.Latest11Mnth,
                    Latest12Mnth = customer.Latest12Mnth,
                    ShowAfterAdjustAmount = ((customer.CcasUnderpaidRate ?? 0) + (customer.CcasUsabilityAmount ?? 0)),
                    RecentPaymentDate = customer.RecentPaymentDate,
                    RecentPaymentAmount = customer.RecentPaymentAmount,
                    Consume1 = customer.Consume1,
                    Consume2 = customer.Consume2,
                    Consume3 = customer.Consume3,
                    Consume4 = customer.Consume4,
                    Consume5 = customer.Consume5,
                    Consume6 = customer.Consume6,
                    Consume7 = customer.Consume7,
                    Consume8 = customer.Consume8,
                    Consume9 = customer.Consume9,
                    Consume10 = customer.Consume10,
                    Consume11 = customer.Consume11,
                    Consume12 = customer.Consume12,
                    OfferAmount = customer.OfferAmount,
                    UnpaidTotal = customer.UnpaidTotal,
                    AuthorizedAmountNotAccount = customer.AuthorizedAmountNotAccount,
                    PreCash1 = customer.PreCash1,
                    PreCash2 = customer.PreCash2,
                    PreCash3 = customer.PreCash3,
                    PreCash4 = customer.PreCash4,
                    PreCash5 = customer.PreCash5,
                    PreCash6 = customer.PreCash6,
                    PreCash7 = customer.PreCash7,
                    PreCash8 = customer.PreCash8,
                    PreCash9 = customer.PreCash9,
                    PreCash10 = customer.PreCash10,
                    PreCash11 = customer.PreCash11,
                    PreCash12 = customer.PreCash12,
                    CcasUnderpaidAmount = customer.CcasUnderpaidAmount,
                    CcasUsabilityAmount = customer.CcasUsabilityAmount,
                    CcasUnderpaidRate = customer.CcasUnderpaidRate,
                    CreditRating1 = customer.CreditRating1,
                    CreditRating2 = customer.CreditRating2,
                    CreditRating3 = customer.CreditRating3,
                    CreditRating4 = customer.CreditRating4,
                    CreditRating5 = customer.CreditRating5,
                    CreditRating6 = customer.CreditRating6,
                    CreditRating7 = customer.CreditRating7,
                    CreditRating8 = customer.CreditRating8,
                    CreditRating9 = customer.CreditRating9,
                    CreditRating10 = customer.CreditRating10,
                    CreditRating11 = customer.CreditRating11,
                    CreditRating12 = customer.CreditRating12,
                    JcicQueryDate = adjustApplicationData.JcicDate.JcicQueryDate,
                    AdjustReason = GetAdjustReasonCodeSelectItemList(),

                    // TODO RG_PARA_M_ACTIVE
                    AdjustmentAmountCeiling = null,

                    UseLocation = CreateUseLocationSelectItemList(),
                    ManualAuthorization = CreateManualAuthorizationSelectItemList(),

                    // TODO Call SP_ELGB_PAD02
                    ProjectAdjustResult = null,
                    ProjectRejectReason = null,
                    GeneralAdjustResult = null,
                    GeneralRejectReason = null,

                    LatestAdjustReason = customer.AdjustReason,
                    LatestAdjustArea = customer.AdjustArea,
                    LatestAdjustStartDate = customer.AdjustStartDate,
                    LatestAdjustEndDate = customer.AdjustEndDate,
                    LatestAdjustEffectAmount = customer.AdjustEffectAmount,
                    LatestAdjustInfo = adjustApplicationData.AdjustLogList,
                };

            }
            catch (Exception e)
            {
                logger.Error(e);
            }


            return View(_adjustProcessPage, viewModel ?? new AdjustProcessViewModel()
            {
                AdjustReason = GetAdjustReasonCodeSelectItemList(),
                UseLocation = CreateUseLocationSelectItemList(),
                ManualAuthorization = CreateManualAuthorizationSelectItemList(),
            });
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