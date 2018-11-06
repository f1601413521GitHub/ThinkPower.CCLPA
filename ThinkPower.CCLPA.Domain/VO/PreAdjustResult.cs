using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 臨調預審名單回傳結果類別
    /// </summary>
    public class PreAdjustResult
    {
        /// <summary>
        /// 臨調預審名單
        /// </summary>
        public IEnumerable<PreAdjustResultInfo> PreAdjustList { get; set; }
    }
}