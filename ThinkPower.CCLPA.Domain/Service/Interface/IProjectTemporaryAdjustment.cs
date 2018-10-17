using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 專案臨調服務公開介面
    /// </summary>
    interface IProjectTemporaryAdjustment
    {
        /// <summary>
        /// 檢核預審名單
        /// </summary>
        /// <param name="cmpnId">行銷活動代號</param>
        /// <returns></returns>
        TemporaryAdjustmentEntity ValidatePreTrialList(string cmpnId);

        /// <summary>
        /// 預審名單匯入
        /// </summary>
        /// <param name="cmpnId">行銷活動代號</param>
        /// <returns></returns>
        TemporaryAdjustmentEntity ImportPreTrialList(string cmpnId);

        /// <summary>
        /// 預審名單處理
        /// </summary>
        void PreTrialListProcessing();

        /// <summary>
        /// 臨調處理
        /// </summary>
        void Processing();
    }
}
