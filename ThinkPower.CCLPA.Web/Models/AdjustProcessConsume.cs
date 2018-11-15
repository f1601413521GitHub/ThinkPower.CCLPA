namespace ThinkPower.CCLPA.Web.Models
{
    /// <summary>
    /// 消費金額類別
    /// </summary>
    public class AdjustProcessConsume
    {
        /// <summary>
        /// 消費月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 消費金額
        /// </summary>
        public decimal? Amount { get; set; }
    }
}