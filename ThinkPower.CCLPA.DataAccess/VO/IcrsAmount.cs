namespace ThinkPower.CCLPA.DataAccess.VO
{
    /// <summary>
    /// ICRS掛帳金額資料類別
    /// </summary>
    public class IcrsAmount
    {
        /// <summary>
        /// 掛帳金額 (含已授權未清算)
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 可用額度
        /// </summary>
        public decimal? AvailableCredit { get; set; }
        /// <summary>
        /// 卡戶等級
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 所有有效卡均被下特殊指示戶D控時，回傳Y;否則回傳N（D控不判斷是否控至效期）
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 回覆碼
        /// </summary>
        public string ResponseCode { get; set; }
    }
}