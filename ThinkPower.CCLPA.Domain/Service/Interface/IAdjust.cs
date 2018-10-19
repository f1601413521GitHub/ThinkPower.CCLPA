using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.DTO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 專案臨調服務公開介面
    /// </summary>
    interface IAdjust
    {
        /// <summary>
        /// 檢核預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        ValidatePreAdjustResultDTO ValidatePreAdjust(string campaignId);

        /// <summary>
        /// 匯入預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        object ImportPreAdjust(string campaignId);

        /// <summary>
        /// 處理預審名單
        /// </summary>
        void PreAdjustProcessing();

        /// <summary>
        /// 臨調處理
        /// </summary>
        void AdjustProcessing();
    }
}
