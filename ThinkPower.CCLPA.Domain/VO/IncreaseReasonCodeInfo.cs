namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 調高原因代碼檔資料類別
    /// </summary>
    public class IncreaseReasonCodeInfo
    {
        /// <summary>
        /// 調高原因代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 調高原因說明
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 使用註記
        /// </summary>
        public string UseFlag { get; set; }
    }
}