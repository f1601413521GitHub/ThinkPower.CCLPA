using PagedList;
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

        /// <summary>
        /// 判斷是否可執行操作功能
        /// </summary>
        public bool CanExecuteOperation { get; set; }

        /// <summary>
        /// 等待區資料分頁頁碼
        /// </summary>
        public int NotEffectPageIndex { get; set; }

        /// <summary>
        /// 生效區資料分頁頁碼
        /// </summary>
        public int EffectPageIndex { get; set; }

        /// <summary>
        /// 資料分頁每頁筆數
        /// </summary>
        public int PagingSize { get; set; }


        /// <summary>
        /// 待生效預審名單
        /// </summary>
        public IPagedList<PreAdjustEntity> NotEffectPreAdjustList { get; set; }

        /// <summary>
        /// 生效中預審名單
        /// </summary>
        public IPagedList<PreAdjustEntity> EffectPreAdjustList { get; set; }
    }
}