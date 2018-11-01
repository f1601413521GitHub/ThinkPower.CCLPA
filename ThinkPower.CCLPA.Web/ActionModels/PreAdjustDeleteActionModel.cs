using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Web.ActionModels
{
    /// <summary>
    /// 刪除臨調預審名單資料模型類別
    /// </summary>
    public class PreAdjustDeleteActionModel
    {
        /// <summary>
        /// 預審名單
        /// </summary>
        public IEnumerable<PreAdjustShortData> PreAdjustList { get; set; }

        /// <summary>
        /// 刪除備註說明
        /// </summary>
        public string Remark { get; set; }
    }
}