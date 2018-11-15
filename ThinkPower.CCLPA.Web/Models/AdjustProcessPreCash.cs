namespace ThinkPower.CCLPA.Web.Models
{
    /// <summary>
    /// 預借金額類別
    /// </summary>
    public class AdjustProcessPreCash
    {
        /// <summary>
        /// 借款月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 借款金額
        /// </summary>
        public decimal? Amount { get; set; }
    }
}