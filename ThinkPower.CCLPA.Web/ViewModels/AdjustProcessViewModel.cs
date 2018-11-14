using System;
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
        public decimal? MonthStarLevel { get; set; }
        /// <summary>
        /// 中文姓名
        /// </summary>
        public string ChineseName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }
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
        public DateTime? IssueDate { get; set; }
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
        public decimal? NearlyYearMaxConsumptionAmount { get; set; }
        /// <summary>
        /// 註記
        /// </summary>
        public string SystemAdjustRevFlag { get; set; }
        /// <summary>
        /// 信用額度
        /// </summary>
        public decimal? CreditLimit { get; set; }
        /// <summary>
        /// 近期關帳
        /// </summary>
        public decimal? ClosingAmount { get; set; }
        /// <summary>
        /// 最低應繳
        /// </summary>
        public decimal? MinimumAmountPayable { get; set; }
        /// <summary>
        /// 近1個月繳款評等
        /// </summary>
        public string Latest1Mnth { get; set; }

        /// <summary>
        /// 近2個月繳款評等
        /// </summary>
        public string Latest2Mnth { get; set; }

        /// <summary>
        /// 近3個月繳款評等
        /// </summary>
        public string Latest3Mnth { get; set; }

        /// <summary>
        /// 近4個月繳款評等
        /// </summary>
        public string Latest4Mnth { get; set; }

        /// <summary>
        /// 近5個月繳款評等
        /// </summary>
        public string Latest5Mnth { get; set; }

        /// <summary>
        /// 近6個月繳款評等
        /// </summary>
        public string Latest6Mnth { get; set; }

        /// <summary>
        /// 近7個月繳款評等
        /// </summary>
        public string Latest7Mnth { get; set; }

        /// <summary>
        /// 近8個月繳款評等
        /// </summary>
        public string Latest8Mnth { get; set; }

        /// <summary>
        /// 近9個月繳款評等
        /// </summary>
        public string Latest9Mnth { get; set; }

        /// <summary>
        /// 近10個月繳款評等 
        /// </summary>
        public string Latest10Mnth { get; set; }

        /// <summary>
        /// 近11個月繳款評等 
        /// </summary>
        public string Latest11Mnth { get; set; }

        /// <summary>
        /// 近12個月繳款評等 
        /// </summary>
        public string Latest12Mnth { get; set; }

        /// <summary>
        /// 臨調後額度
        /// </summary>
        public decimal? ShowAfterAdjustAmount { get; set; }
        /// <summary>
        /// 最近付款日期
        /// </summary>
        public DateTime? RecentPaymentDate { get; set; }
        /// <summary>
        /// 最近付款金額
        /// </summary>
        public decimal? RecentPaymentAmount { get; set; }
        /// <summary>
        /// 前1月單月消費金額
        /// </summary>
        public decimal? Consume1 { get; set; }

        /// <summary>
        /// 前2月單月消費金額
        /// </summary>
        public decimal? Consume2 { get; set; }

        /// <summary>
        /// 前3月單月消費金額
        /// </summary>
        public decimal? Consume3 { get; set; }

        /// <summary>
        /// 前4月單月消費金額
        /// </summary>
        public decimal? Consume4 { get; set; }

        /// <summary>
        /// 前5月單月消費金額
        /// </summary>
        public decimal? Consume5 { get; set; }

        /// <summary>
        /// 前6月單月消費金額
        /// </summary>
        public decimal? Consume6 { get; set; }

        /// <summary>
        /// 前7月單月消費金額
        /// </summary>
        public decimal? Consume7 { get; set; }

        /// <summary>
        /// 前8月單月消費金額
        /// </summary>
        public decimal? Consume8 { get; set; }

        /// <summary>
        /// 前9月單月消費金額
        /// </summary>
        public decimal? Consume9 { get; set; }

        /// <summary>
        /// 前10月單月消費金額 
        /// </summary>
        public decimal? Consume10 { get; set; }

        /// <summary>
        /// 前11月單月消費金額 
        /// </summary>
        public decimal? Consume11 { get; set; }

        /// <summary>
        /// 前12月單月消費金額 
        /// </summary>
        public decimal? Consume12 { get; set; }
        /// <summary>
        /// 優利餘額
        /// </summary>
        public decimal? OfferAmount { get; set; }
        /// <summary>
        /// 總未付
        /// </summary>
        public decimal? UnpaidTotal { get; set; }
        /// <summary>
        /// 未入帳金額
        /// </summary>
        public decimal? AuthorizedAmountNotAccount { get; set; }
        /// <summary>
        /// 前1月單月預借現金
        /// </summary>
        public decimal? PreCash1 { get; set; }

        /// <summary>
        /// 前2月單月預借現金
        /// </summary>
        public decimal? PreCash2 { get; set; }

        /// <summary>
        /// 前3月單月預借現金
        /// </summary>
        public decimal? PreCash3 { get; set; }

        /// <summary>
        /// 前4月單月預借現金
        /// </summary>
        public decimal? PreCash4 { get; set; }

        /// <summary>
        /// 前5月單月預借現金
        /// </summary>
        public decimal? PreCash5 { get; set; }

        /// <summary>
        /// 前6月單月預借現金
        /// </summary>
        public decimal? PreCash6 { get; set; }

        /// <summary>
        /// 前7月單月預借現金
        /// </summary>
        public decimal? PreCash7 { get; set; }

        /// <summary>
        /// 前8月單月預借現金
        /// </summary>
        public decimal? PreCash8 { get; set; }

        /// <summary>
        /// 前9月單月預借現金
        /// </summary>
        public decimal? PreCash9 { get; set; }

        /// <summary>
        /// 前10月單月預借現金 
        /// </summary>
        public decimal? PreCash10 { get; set; }

        /// <summary>
        /// 前11月單月預借現金 
        /// </summary>
        public decimal? PreCash11 { get; set; }

        /// <summary>
        /// 前12月單月預借現金 
        /// </summary>
        public decimal? PreCash12 { get; set; }

        /// <summary>
        /// CCAS掛帳
        /// </summary>
        public decimal? CcasUnderpaidAmount { get; set; }
        /// <summary>
        /// 可用餘額
        /// </summary>
        public decimal? CcasUsabilityAmount { get; set; }
        /// <summary>
        /// 掛帳比率
        /// </summary>
        public decimal? CcasUnderpaidRate { get; set; }
        /// <summary>
        /// 前1期信貸評等
        /// </summary>
        public string CreditRating1 { get; set; }

        /// <summary>
        /// 前2期信貸評等
        /// </summary>
        public string CreditRating2 { get; set; }

        /// <summary>
        /// 前3期信貸評等
        /// </summary>
        public string CreditRating3 { get; set; }

        /// <summary>
        /// 前4期信貸評等
        /// </summary>
        public string CreditRating4 { get; set; }

        /// <summary>
        /// 前5期信貸評等
        /// </summary>
        public string CreditRating5 { get; set; }

        /// <summary>
        /// 前6期信貸評等
        /// </summary>
        public string CreditRating6 { get; set; }

        /// <summary>
        /// 前7期信貸評等
        /// </summary>
        public string CreditRating7 { get; set; }

        /// <summary>
        /// 前8期信貸評等
        /// </summary>
        public string CreditRating8 { get; set; }

        /// <summary>
        /// 前9期信貸評等
        /// </summary>
        public string CreditRating9 { get; set; }

        /// <summary>
        /// 前10期信貸評等 
        /// </summary>
        public string CreditRating10 { get; set; }

        /// <summary>
        /// 前11期信貸評等 
        /// </summary>
        public string CreditRating11 { get; set; }

        /// <summary>
        /// 前12期信貸評等 
        /// </summary>
        public string CreditRating12 { get; set; }
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
        public decimal? LatestAdjustEffectAmount { get; set; }
        /// <summary>
        /// 最近一次臨調(清單)
        /// </summary>
        public IEnumerable<AdjustEntity> LatestAdjustInfo { get; set; }
    }
}