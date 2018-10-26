namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 預審名單驗證結果類別
    /// </summary>
    public class PreAdjustValidateResult
    {
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 行銷活動名單數量
        /// </summary>
        public int CampaignDetailCount { get; set; }
    }
}