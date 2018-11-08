using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 臨調處理檢核結果類別
    /// </summary>
    public class VerifiedResult
    {
        /// <summary>
        /// 錯誤資訊(題號與訊息)
        /// </summary>
        public Dictionary<string, string> ErrorInfo { get; set; }
        /// <summary>
        /// 預審名單資訊
        /// </summary>
        public PreAdjustEntity PreAdjustInfo { get; set; }
        /// <summary>
        /// 歸戶基本資料
        /// </summary>
        public CustomerInfo CustomerInfo { get; set; }
    }
}