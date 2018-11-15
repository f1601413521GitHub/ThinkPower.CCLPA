using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 專案臨調檢核結果類別
    /// </summary>
    public class AdjustVerifyResult
    {
        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public IEnumerable<string> ErrorCodeList { get; set; }
        /// <summary>
        /// 預審名單資訊
        /// </summary>
        public PreAdjustEntity PreAdjustInfo { get; set; }
        /// <summary>
        /// 歸戶基本資料
        /// </summary>
        public Customer CustomerInfo { get; set; }
    }
}