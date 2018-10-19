namespace ThinkPower.CCLPA.Domain.DTO
{
    /// <summary>
    /// 預審名單檢核結果類別
    /// </summary>
    public class ValidatePreAdjustResultDTO
    {
        /// <summary>
        /// 檢核失敗錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 預審名單數量
        /// </summary>
        public int? CampaignListCount { get; set; }
    }
}