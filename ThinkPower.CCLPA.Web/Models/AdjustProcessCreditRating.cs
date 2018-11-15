namespace ThinkPower.CCLPA.Web.Models
{
    /// <summary>
    /// 信貸/AIG評等類別
    /// </summary>
    public class AdjustProcessCreditRating
    {
        /// <summary>
        /// 評等月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 信貸/AIG評等
        /// </summary>
        public string Rating { get; set; }
    }
}