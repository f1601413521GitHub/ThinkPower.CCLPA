using System;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 貴賓資訊類別
    /// </summary>
    public class VipInfo
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary> 
        ///  資料年月 
        /// </summary> 
        public string DataDate { get; set; }
        /// <summary> 
        ///  資料異動日期
        /// </summary> 
        public string DataChangeDate { get; set; }
        /// <summary> 
        ///  適用VIP星等 
        /// </summary> 
        public Nullable<decimal> ApplicableStarLevel { get; set; }
        /// <summary> 
        ///  適用VIP星等效期
        /// </summary> 
        public string ApplicableStarValidityPeriod { get; set; }
        /// <summary> 
        ///  本月VIP星等 
        /// </summary> 
        public Nullable<decimal> MonthStarLevel { get; set; }
        /// <summary> 
        ///  本月VIP星等效期
        /// </summary> 
        public string MonthStarValidityPeriod { get; set; }
        /// <summary> 
        ///  往來業務餘額
        /// </summary> 
        public Nullable<decimal> BusinessBalance { get; set; }
        /// <summary> 
        ///  存款平均餘額
        /// </summary> 
        public Nullable<decimal> AverageBalance { get; set; }
        /// <summary> 
        ///  庫存餘額+贖回在途 
        /// </summary> 
        public Nullable<decimal> InventotyBalance { get; set; }
        /// <summary> 
        ///  保險回流型已繳保費
        /// </summary> 
        public Nullable<decimal> PremiunsPaid { get; set; }
        /// <summary> 
        ///  AUM餘額  
        /// </summary> 
        public Nullable<decimal> AUM { get; set; }
        /// <summary> 
        ///  最近一年信用卡刷卡總金額
        /// </summary> 
        public Nullable<decimal> NearlyYearSwipeAmount { get; set; }
        /// <summary> 
        ///  房貸餘額 
        /// </summary> 
        public Nullable<decimal> MortgageBalance { get; set; }
        /// <summary> 
        ///  外匯保證金  
        /// </summary> 
        public Nullable<decimal> ForexMargin { get; set; }
        /// <summary> 
        ///  可轉債名目本金 
        /// </summary> 
        public Nullable<decimal> ConvertiblePrincipal { get; set; }
        /// <summary> 
        ///  遠智複委託  
        /// </summary> 
        public Nullable<decimal> ReDelegate { get; set; }
        /// <summary> 
        ///  信用卡VIP To Be
        /// </summary> 
        public Nullable<decimal> CardVipFlag { get; set; }
        /// <summary> 
        ///  個金Household例外星等
        /// </summary> 
        public Nullable<decimal> HouseholdExceptionStars { get; set; }
        /// <summary> 
        ///  法金例外星等
        /// </summary> 
        public Nullable<decimal> LegalExceptionStars { get; set; }
    }
}