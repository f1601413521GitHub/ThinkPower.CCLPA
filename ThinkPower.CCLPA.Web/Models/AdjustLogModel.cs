using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Web.Models
{
    /// <summary>
    /// 臨調紀錄類別
    /// </summary>
    public class AdjustLogModel
    {
        /// <summary>
        /// 最近一次臨調(原因)
        /// </summary>
        public string AdjustReason { get; set; }
        /// <summary>
        /// 最近一次臨調(區域)
        /// </summary>
        public string AdjustArea { get; set; }
        /// <summary>
        /// 最近一次臨調(起日)
        /// </summary>
        public string AdjustStartDate { get; set; }
        /// <summary>
        /// 最近一次臨調(迄日)
        /// </summary>
        public string AdjustEndDate { get; set; }
        /// <summary>
        /// 最近一次臨調(生效金額)
        /// </summary>
        public decimal? AdjustEffectAmount { get; set; }
        /// <summary>
        /// 最近一次臨調(清單)
        /// </summary>
        public IEnumerable<AdjustEntity> AdjustLogList { get; set; }
    }
}