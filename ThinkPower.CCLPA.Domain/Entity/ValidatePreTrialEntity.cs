namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 專案臨調檢核預審名單結果類別
    /// </summary>
    public class ValidatePreTrialEntity
    {
        /// <summary>
        /// 檢核訊息
        /// </summary>
        public string ValidateMessage { get; set; }

        /// <summary>
        /// 預審名單數量
        /// </summary>
        public int? CampaignListCount { get; set; }
    }
}