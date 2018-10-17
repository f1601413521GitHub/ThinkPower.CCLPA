using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 專案臨調服務公開介面
    /// </summary>
    interface IProjectTemporaryAdjustment
    {
        /// <summary>
        /// 預審名單匯入
        /// </summary>
        /// <param name="cmpnId">行銷活動代號</param>
        /// <returns></returns>
        object ImportPreTrialList(string cmpnId);

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
