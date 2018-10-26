namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 強制同意失敗結果類別
    /// </summary>
    public class ForcedConsentFailResult
    {
        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 拒絕原因代碼
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 處理代碼
        /// </summary>
        public string ResponseCode { get; set; }
    }
}