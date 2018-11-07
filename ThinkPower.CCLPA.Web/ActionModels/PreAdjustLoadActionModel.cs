namespace ThinkPower.CCLPA.Web.ActionModels
{
    /// <summary>
    /// 載入臨調預審名單資料模型類別
    /// </summary>
    public class PreAdjustLoadActionModel
    {
        /// <summary>
        /// 資料分頁頁碼
        /// </summary>
        public int NotEffectPageIndex { get; set; }

        /// <summary>
        /// 資料分頁頁碼
        /// </summary>
        public int EffectPageIndex { get; set; }
        
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }
    }
}