using System;

namespace ThinkPower.CCLPA.Web.Models
{
    /// <summary>
    /// 臨調處理歸戶資料類別
    /// </summary>
    public class AdjustProcessCustomer
    {
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
        /// 最近付款日期
        /// </summary>
        public DateTime? RecentPaymentDate { get; set; }
        /// <summary>
        /// 最近付款金額
        /// </summary>
        public decimal? RecentPaymentAmount { get; set; }
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
    }
}