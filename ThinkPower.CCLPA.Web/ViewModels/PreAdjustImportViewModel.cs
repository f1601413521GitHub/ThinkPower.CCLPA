namespace ThinkPower.CCLPA.Web.ViewModels
{
    /// <summary>
    /// 匯入臨調預審名單檢視模型類別
    /// </summary>
    public class PreAdjustImportViewModel
    {
        /// <summary>
        /// 行銷活動代碼
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 是否執行匯入
        /// </summary>
        public bool ExecuteImport { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 臨調預審名單數量
        /// </summary>
        public int? CampaignDetailCount { get; set; }

    }
}