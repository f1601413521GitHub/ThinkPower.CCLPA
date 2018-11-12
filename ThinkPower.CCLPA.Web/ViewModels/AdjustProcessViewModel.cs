using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Web.ViewModels
{
    /// <summary>
    /// 專案臨調檢視模型類別
    /// </summary>
    public class AdjustProcessViewModel
    {
        /// <summary>
        /// 歸戶ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 身分證字號
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// 正卡有效卡
        /// </summary>
        public string LiveCardCount { get; set; }
        /// <summary>
        /// 停卡狀態中是否有3-77
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }
        /// <summary>
        /// 本月VIP星等 
        /// </summary>
        public string MonthStarLevel { get; set; }
        /// <summary>
        /// 中文姓名
        /// </summary>
        public string ChineseName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string BirthDay { get; set; }
        /// <summary>
        /// 歸戶狀態
        /// </summary>
        public string AboutDataStatus { get; set; }
        /// <summary>
        /// 卡戶等級
        /// </summary>
        public string RiskLevel { get; set; }
        /// <summary>
        /// 風險評等
        /// </summary>
        public string RiskRating { get; set; }
        /// <summary>
        /// 最初發卡日
        /// </summary>
        public string IssueDate { get; set; }
        /// <summary>
        /// 關帳日
        /// </summary>
        public string ClosingDay { get; set; }
        /// <summary>
        /// 繳款期限
        /// </summary>
        public string PayDeadline { get; set; }
        /// <summary>
        /// 職業／職級
        /// </summary>
        public string Vocation { get; set; }
        /// <summary>
        /// 住家電話
        /// </summary>
        public string TelHome { get; set; }
        /// <summary>
        /// 公司電話
        /// </summary>
        public string TelOffice { get; set; }
        /// <summary>
        /// 行動電話
        /// </summary>
        public string MobileTel { get; set; }
        /// <summary>
        /// 帳單地址
        /// </summary>
        public string BillAddr { get; set; }
        /// <summary>
        /// 近一年最高消費金額
        /// </summary>
        public string NearlyYearMaxConsumptionAmount { get; set; }
        /// <summary>
        /// 註記
        /// </summary>
        public string SystemAdjustRevFlag { get; set; }
        /// <summary>
        /// 信用額度
        /// </summary>
        public string CreditLimit { get; set; }
        /// <summary>
        /// 近期關帳
        /// </summary>
        public string ClosingAmount { get; set; }
        /// <summary>
        /// 最低應繳
        /// </summary>
        public string MinimumAmountPayable { get; set; }
        /// <summary>
        /// 繳款評等(1~12)
        /// </summary>
        public string Latest1Mnth { get; set; }
        /// <summary>
        /// 臨調後額度
        /// </summary>
        public string ShowAfterAdjustAmount { get; set; }
        /// <summary>
        /// 最近付款日期
        /// </summary>
        public string RecentPaymentDate { get; set; }
        /// <summary>
        /// 最近付款金額
        /// </summary>
        public string RecentPaymentAmount { get; set; }
        /// <summary>
        /// 消費金額(1~12)
        /// </summary>
        public string Consume1 { get; set; }
        /// <summary>
        /// 優利餘額
        /// </summary>
        public string OfferAmount { get; set; }
        /// <summary>
        /// 總未付
        /// </summary>
        public string UnpaidTotal { get; set; }
        /// <summary>
        /// 未入帳金額
        /// </summary>
        public string AuthorizedAmountNotAccount { get; set; }
        /// <summary>
        /// 預借金額(1~12)
        /// </summary>
        public string PreCash1 { get; set; }
        /// <summary>
        /// CCAS掛帳
        /// </summary>
        public string CcasUnderpaidAmount { get; set; }
        /// <summary>
        /// 可用餘額
        /// </summary>
        public string CcasUsabilityAmount { get; set; }
        /// <summary>
        /// 掛帳比率
        /// </summary>
        public string CcasUnderpaidRate { get; set; }
        /// <summary>
        /// 信貸/AIG評等(1~12)
        /// </summary>
        public string CreditRating1 { get; set; }
        /// <summary>
        /// JCIC日期
        /// </summary>
        public string JcicQueryDate { get; set; }
        /// <summary>
        /// 調高原因
        /// </summary>
        public IEnumerable<SelectListItem> AdjustReason { get; set; }
        /// <summary>
        /// 調高原因備註
        /// </summary>
        public string AdjustReasonRemark { get; set; }
        /// <summary>
        /// 調額上限
        /// </summary>
        public string AdjustmentAmountCeiling { get; set; }
        /// <summary>
        /// 刷卡金額(不含額度)
        /// </summary>
        public string SwipeAmount { get; set; }
        /// <summary>
        /// 臨調後額度
        /// </summary>
        public string AfterAdjustAmount { get; set; }
        /// <summary>
        /// 有效日期(起)
        /// </summary>
        public string ValidDateStart { get; set; }
        /// <summary>
        /// 有效日期(迄)
        /// </summary>
        public string ValidDateEnd { get; set; }
        /// <summary>
        /// 使用地點
        /// </summary>
        public IEnumerable<SelectListItem> UseLocation { get; set; }
        /// <summary>
        /// 出國地點
        /// </summary>
        public string PlaceOfGoingAbroad { get; set; }
        /// <summary>
        /// 是否可人工授權
        /// </summary>
        public IEnumerable<SelectListItem> ManualAuthorization { get; set; }
        /// <summary>
        /// 專案臨調結果
        /// </summary>
        public string ProjectAdjustResult { get; set; }
        /// <summary>
        /// 拒絕原因(專案)
        /// </summary>
        public string ProjectRejectReason { get; set; }
        /// <summary>
        /// 一般臨調結果
        /// </summary>
        public string GeneralAdjustResult { get; set; }
        /// <summary>
        /// 拒絕原因(一般)
        /// </summary>
        public string GeneralRejectReason { get; set; }
        /// <summary>
        /// 轉授信主管原因
        /// </summary>
        public string TransferSupervisorReason { get; set; }
        /// <summary>
        /// 最近一次臨調(原因)
        /// </summary>
        public string LatestAdjustReason { get; set; }
        /// <summary>
        /// 最近一次臨調(區域)
        /// </summary>
        public string LatestAdjustArea { get; set; }
        /// <summary>
        /// 最近一次臨調(起日)
        /// </summary>
        public string LatestAdjustStartDate { get; set; }
        /// <summary>
        /// 最近一次臨調(迄日)
        /// </summary>
        public string LatestAdjustEndDate { get; set; }
        /// <summary>
        /// 最近一次臨調(生效金額)
        /// </summary>
        public string LatestAdjustEffectAmount { get; set; }
        /// <summary>
        /// 最近一次臨調(清單)
        /// </summary>
        public IEnumerable<AdjustEntity> LatestAdjustInfo { get; set; }
    }
}