using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.Domain.Condition;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 臨調預審名單資訊類別
    /// </summary>
    public class PreAdjustInfo
    {
        /// <summary>
        /// 臨調預審名單
        /// </summary>
        public IEnumerable<PreAdjustResultInfo> PreAdjustList { get; set; }

        /// <summary>
        /// 刪除備註說明
        /// </summary>
        public string Remark { get; set; }
    }
}