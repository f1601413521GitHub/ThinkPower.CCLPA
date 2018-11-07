using System;

namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 歸戶基本資料類別
    /// </summary>
    public class CustomerEntity : BaseEntity
    {
        /// <summary>
        /// 身份證字號 
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 中文姓名 
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 生日 
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// 卡戶等級 
        /// </summary>
        public string RiskLevel { get; set; }

        /// <summary>
        /// 風險評等 
        /// </summary>
        public string RiskRating { get; set; }

        /// <summary>
        /// 信用額度 
        /// </summary>
        public decimal? CreditLimit { get; set; }

        /// <summary>
        /// 歸戶狀態 
        /// </summary>
        public string AboutDataStatus { get; set; }

        /// <summary>
        /// 最初發卡日 
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// 任一有效活卡(商務卡,政府採購卡,虛擬卡除外) 
        /// </summary>
        public decimal? LiveCardCount { get; set; }

        /// <summary>
        /// 停卡狀態是否有3-77 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 職業/職級
        /// </summary>
        public string Vocation { get; set; }

        /// <summary>
        /// 帳單地址 
        /// </summary>
        public string BillAddr { get; set; }

        /// <summary>
        /// 公司電話 
        /// </summary>
        public string TelOffice { get; set; }

        /// <summary>
        /// 住家電話 
        /// </summary>
        public string TelHome { get; set; }

        /// <summary>
        /// 行動電話 
        /// </summary>
        public string MobileTel { get; set; }

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
        /// 關帳日 
        /// </summary>
        public string ClosingDay { get; set; }

        /// <summary>
        /// 繳款期限 
        /// </summary>
        public string PayDeadline { get; set; }

        /// <summary>
        /// 最近一期關帳金額 
        /// </summary>
        public decimal? ClosingAmount { get; set; }

        /// <summary>
        /// 最近一期最低應繳金額 
        /// </summary>
        public decimal? MinimumAmountPayable { get; set; }

        /// <summary>
        /// 最近付款金額 
        /// </summary>
        public decimal? RecentPaymentAmount { get; set; }

        /// <summary>
        /// 最近付款日期 
        /// </summary>
        public DateTime? RecentPaymentDate { get; set; }

        /// <summary>
        /// 優利餘額 
        /// </summary>
        public decimal? OfferAmount { get; set; }

        /// <summary>
        /// 總未付 
        /// </summary>
        public decimal? UnpaidTotal { get; set; }

        /// <summary>
        /// 已授權未入帳金額 
        /// </summary>
        public decimal? AuthorizedAmountNotAccount { get; set; }

        /// <summary>
        /// 最近一次臨調原因 
        /// </summary>
        public string AdjustReason { get; set; }

        /// <summary>
        /// 最近一次臨調區域 
        /// </summary>
        public string AdjustArea { get; set; }

        /// <summary>
        /// 最近一次臨調起日 
        /// </summary>
        public string AdjustStartDate { get; set; }

        /// <summary>
        /// 最近一次臨調迄日 
        /// </summary>
        public string AdjustEndDate { get; set; }

        /// <summary>
        /// 最近一次臨調生效金額 
        /// </summary>
        public decimal? AdjustEffectAmount { get; set; }

        /// <summary>
        /// 本行持卡時間 
        /// </summary>
        public decimal? VintageMonths { get; set; }

        /// <summary>
        /// 所有有效卡都被下『特殊指示戶』(D控)
        /// </summary>
        public string StatusFlag { get; set; }

        /// <summary>
        /// 有保人之歸戶 
        /// </summary>
        public string GutrFlag { get; set; }

        /// <summary>
        /// 延滯天數 
        /// </summary>
        public decimal? DelayCount { get; set; }

        /// <summary>
        /// CCAS目前掛帳金額(含已授權未入帳) 
        /// </summary>
        public decimal? CcasUnderpaidAmount { get; set; }

        /// <summary>
        /// CCAS目前可用餘額 
        /// </summary>
        public decimal? CcasUsabilityAmount { get; set; }

        /// <summary>
        /// CCAS目前掛帳比率 
        /// </summary>
        public decimal? CcasUnderpaidRate { get; set; }

        /// <summary>
        /// 資料日期 
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 符合循環戶退場資格 
        /// </summary>
        public string EligibilityForWithdrawal { get; set; }

        /// <summary>
        /// 1414不可系統調整額度註記 
        /// </summary>
        public string SystemAdjustRevFlag { get; set; }

        /// <summary>
        /// 自動扣款 
        /// </summary>
        public string AutomaticDebit { get; set; }

        /// <summary>
        /// 扣款行庫代碼 
        /// </summary>
        public string DebitBankCode { get; set; }

        /// <summary>
        /// ACCOUNT LINK代繳 
        /// </summary>
        public string EtalStatus { get; set; }

        /// <summary>
        /// 戶籍電話 
        /// </summary>
        public string TelResident { get; set; }

        /// <summary>
        /// 卡片及帳單寄送地址碼 
        /// </summary>
        public string SendType { get; set; }

        /// <summary>
        /// 電子帳單客戶註記 
        /// </summary>
        public string ElectronicBillingCustomerNote { get; set; }

        /// <summary>
        /// 電子信箱 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 行業別(IC) 
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 職稱(CR) 
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 戶籍地址 
        /// </summary>
        public string ResidentAddr { get; set; }

        /// <summary>
        /// 通訊地址 
        /// </summary>
        public string MailingAddr { get; set; }

        /// <summary>
        /// 公司地址 
        /// </summary>
        public string CompanyAddr { get; set; }

        /// <summary>
        /// 年收入 
        /// </summary>
        public decimal? AnnualIncome { get; set; }

        /// <summary>
        /// IN1
        /// </summary>
        public decimal? In1 { get; set; }

        /// <summary>
        /// IN2
        /// </summary>
        public decimal? In2 { get; set; }

        /// <summary>
        /// IN3
        /// </summary>
        public decimal? In3 { get; set; }

        /// <summary>
        /// 戶籍地址郵遞區號 
        /// </summary>
        public string ResidentAddrPostalCode { get; set; }

        /// <summary>
        /// 通訊地址郵遞區號 
        /// </summary>
        public string MailingAddrPostalCode { get; set; }

        /// <summary>
        /// 公司地址郵遞區號 
        /// </summary>
        public string CompanyAddrPostalCode { get; set; }
    }
}