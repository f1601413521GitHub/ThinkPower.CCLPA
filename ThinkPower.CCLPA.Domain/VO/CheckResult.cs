namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 檢核結果類別
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// 驗證結果
        /// </summary>
        public bool ValidateResult { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}