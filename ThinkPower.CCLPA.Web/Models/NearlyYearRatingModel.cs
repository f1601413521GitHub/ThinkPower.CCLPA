using System.Collections.Generic;

namespace ThinkPower.CCLPA.Web.Models
{
    /// <summary>
    /// 近年評等紀錄類別
    /// </summary>
    public class NearlyYearRatingModel
    {
        /// <summary>
        /// 繳款評等
        /// </summary>
        public IEnumerable<AdjustProcessLatestMnth> LatestMnthList { get; set; }
        /// <summary>
        /// 消費金額
        /// </summary>
        public IEnumerable<AdjustProcessConsume> ConsumeList { get; set; }
        /// <summary>
        /// 預借金額
        /// </summary>
        public IEnumerable<AdjustProcessPreCash> PreCashList { get; set; }
        /// <summary>
        /// 信貸/AIG評等
        /// </summary>
        public IEnumerable<AdjustProcessCreditRating> CreditRatingList { get; set; }
    }
}