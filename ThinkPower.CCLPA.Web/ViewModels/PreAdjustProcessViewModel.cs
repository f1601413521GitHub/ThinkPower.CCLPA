using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Web.ViewModels
{
    /// <summary>
    /// 處理臨調預審名單檢視模型類別
    /// </summary>
    public class PreAdjustProcessViewModel
    {
        /// <summary>
        /// 預審名單
        /// </summary>
        public IEnumerable<PreAdjustEntity> PreAdjustList { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 提示訊息
        /// </summary>
        public string TipMessage { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }
    }
}