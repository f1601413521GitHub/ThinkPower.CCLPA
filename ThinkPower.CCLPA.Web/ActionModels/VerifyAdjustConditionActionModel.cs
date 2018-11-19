namespace ThinkPower.CCLPA.Web.ActionModels
{
    /// <summary>
    /// 專案臨調條件檢核資料模型類別
    /// </summary>
    public class VerifyAdjustConditionActionModel
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// JCIC送查日期
        /// </summary>
        public string JcicQueryDate { get; set; }
        /// <summary>
        /// 臨調原因代碼
        /// </summary>
        public string AdjustReasonCode { get; set; }
    }
}