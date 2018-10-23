using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 臨調預審服務公開介面
    /// </summary>
    interface IPreAdjust
    {
        /// <summary>
        /// 匯入臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executeImport">是否執行匯入</param>
        /// <returns>行銷活動名單數量</returns>
        int? ImportPreAdjust(string campaignId, bool executeImport = false);

        /// <summary>
        /// 處理臨調預審名單
        /// </summary>
        void PreAdjustProcessing();
    }
}
