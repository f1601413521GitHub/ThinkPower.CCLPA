using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Web.ActionModels
{
    /// <summary>
    /// 同意臨調預審名單資料模型類別
    /// </summary>
    public class PreAdjustAgreeActionModel
    {
        /// <summary>
        /// 預審名單
        /// </summary>
        public IEnumerable<PreAdjustShortData> PreAdjustList { get; set; }
    }
}