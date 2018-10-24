using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.Entity;

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
        int? Import(string campaignId, bool executeImport = false);

        /// <summary>
        /// 查詢臨調預審名單
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>臨調預審名單</returns>
        PreAdjustInfoEntity Search(string id);

        /// <summary>
        /// 刪除臨調預審名單
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="isWaitZone">是否為等待區</param>
        /// <returns></returns>
        int? Delete(PreAdjustInfoEntity data, bool isWaitZone);

        /// <summary>
        /// 同意執行臨調預審名單
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="forceConsent">是否強制同意</param>
        /// <returns></returns>
        object Agree(object data, bool forceConsent = false);
    }
}
