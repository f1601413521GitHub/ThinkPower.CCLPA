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
            AdjustProcessViewModel viewModel = InitialViewModel();
            viewModel = TestData(viewModel);

            return View(_adjustProcessPage, viewModel);
        }

        private AdjustProcessViewModel TestData(AdjustProcessViewModel viewModel)
        {
            viewModel.DataDate = "2016/03/10";
            viewModel.MonthStarLevel = "3";
            viewModel.AccountId = "H142700334";
            viewModel.LiveCardCount = "Y";
            viewModel.Status = "N";
            viewModel.ChineseName = "王小明";
            viewModel.BirthDay = "1972/05/19";
            viewModel.AboutDataStatus = "Y";
            viewModel.RiskLevel = "3";
            viewModel.RiskRating = "002";
            viewModel.IssueDate = "2011/04/01";
            viewModel.ClosingDay = "20";
            viewModel.PayDeadline = "05";
            viewModel.Vocation = "M";
            viewModel.TelHome = "03-12345678";
            viewModel.TelOffice = "03-12345678";
            viewModel.MobileTel = "09123456789";
            viewModel.BillAddr = "114台北市內湖區內湖路一段360巷15號6號";
            viewModel.NearlyYearMaxConsumptionAmount = "1,201";
            viewModel.SystemAdjustRevFlag = "N";
            viewModel.CreditLimit = "40,000";
            viewModel.ClosingAmount = "9,000";
            viewModel.MinimumAmountPayable = "900";
            viewModel.ShowAfterAdjustAmount = "40,000";
            viewModel.RecentPaymentDate = "2016/03/29";
            viewModel.RecentPaymentAmount = "10,000";
            viewModel.OfferAmount = "77";
            viewModel.UnpaidTotal = "0";
            viewModel.AuthorizedAmountNotAccount = "4,000";
            viewModel.CcasUnderpaidAmount = "20,000";
            viewModel.CcasUsabilityAmount = "20,000";
            viewModel.CcasUnderpaidRate = "33.50";

            viewModel.AdjustReasonRemark = "原因";
            viewModel.JcicQueryDate = "2018/01/01";
            viewModel.AdjustmentAmountCeiling = "99,999";
            viewModel.SwipeAmount = "12,345";
            viewModel.AfterAdjustAmount = "45,678";
            viewModel.ValidDateStart = "2018/01/01";
            viewModel.ValidDateEnd = "2019/01/01";
            viewModel.PlaceOfGoingAbroad = "日本";

            viewModel.ProjectAdjustResult = "預審臨調拒絕";
            viewModel.ProjectRejectReason = "RI_01、RI_02、RI_03、RI_04、RI_05";
            viewModel.GeneralAdjustResult = "預審臨調拒絕";
            viewModel.GeneralRejectReason = "RI_01、RI_03、RI_05";
            viewModel.TransferSupervisorReason = "TEST";

            viewModel.LatestAdjustReason = "原因A";
            viewModel.LatestAdjustArea = "區域B";
            viewModel.LatestAdjustStartDate = "2018/01/02";
            viewModel.LatestAdjustEndDate = "2019/01/02";
            viewModel.LatestAdjustEffectAmount = "12,345";
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

        #endregion



        #region Private Method

        /// <summary>
        /// 初始化臨調檢視模型
        /// </summary>
        /// <returns></returns>
        private AdjustProcessViewModel InitialViewModel()
        {
            return new AdjustProcessViewModel()
            {
                CustomerId = "",
                AccountId = "",
                LiveCardCount = "",
                Status = "",
                DataDate = "",
                MonthStarLevel = "",
                ChineseName = "",
                BirthDay = "",
                AboutDataStatus = "",
                RiskLevel = "",
                RiskRating = "",
                IssueDate = "",
                ClosingDay = "",
                PayDeadline = "",
                Vocation = "",
                TelHome = "",
                TelOffice = "",
                MobileTel = "",
                BillAddr = "",
                NearlyYearMaxConsumptionAmount = "",
                SystemAdjustRevFlag = "",
                CreditLimit = "",
                ClosingAmount = "",
                MinimumAmountPayable = "",
                Latest1Mnth = "",
                ShowAfterAdjustAmount = "",
                RecentPaymentAmount = "",
                Consume1 = "",
                OfferAmount = "",
                UnpaidTotal = "",
                AuthorizedAmountNotAccount = "",
                PreCash1 = "",
                CcasUnderpaidAmount = "",
                CcasUsabilityAmount = "",
                CcasUnderpaidRate = "",
                CreditRating1 = "",
                JcicQueryDate = "",
                AdjustReason = GetAdjustReasonCodeSelectItemList(),
                AdjustReasonRemark = "",
                AdjustmentAmountCeiling = "",
                SwipeAmount = "",
                AfterAdjustAmount = "",
                ValidDateStart = "",
                ValidDateEnd = "",
                UseLocation = CreateUseLocationSelectItemList(),
                PlaceOfGoingAbroad = "",
                ManualAuthorization = CreateManualAuthorizationSelectItemList(),
                ProjectAdjustResult = "",
                ProjectRejectReason = "",
                GeneralAdjustResult = "",
                GeneralRejectReason = "",
                TransferSupervisorReason = "",
                LatestAdjustReason = "",
                LatestAdjustArea = "",
                LatestAdjustStartDate = "",
                LatestAdjustEndDate = "",
                LatestAdjustEffectAmount = "",
                LatestAdjustInfo = null,
            };
        }

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