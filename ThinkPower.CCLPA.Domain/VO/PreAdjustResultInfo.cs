using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 臨調預審名單回傳資料類別
    /// </summary>
    public class PreAdjustResultInfo
    {
        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 拒絕原因代碼
        /// </summary>
        public string RejectReasonCode { get; set; }
    }
}