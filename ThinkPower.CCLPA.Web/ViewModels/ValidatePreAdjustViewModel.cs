using ThinkPower.CCLPA.Domain.DTO;

namespace ThinkPower.CCLPA.Web.ViewModels
{
    /// <summary>
    /// 預審名單檢核檢視模型類別
    /// </summary>
    public class ValidatePreAdjustViewModel
    {
        /// <summary>
        /// 行銷活動代碼
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 預審名單檢核結果
        /// </summary>
        public ValidatePreAdjustResultDTO ValidatePreAdjustResult { get; set; }
    }
}