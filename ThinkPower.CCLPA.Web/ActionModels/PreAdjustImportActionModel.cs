namespace ThinkPower.CCLPA.Web.ActionModels
{
    /// <summary>
    /// 匯入臨調預審名單資料模型類別
    /// </summary>
    public class PreAdjustImportActionModel
    {
        /// <summary>
        /// 行銷活動代碼
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 是否執行匯入
        /// </summary>
        public bool ExecuteImport { get; set; }
    }
}