using System.Collections.Generic;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Web.ActionModels
{
    /// <summary>
    /// 強制同意臨調預審名單資料模型類別
    /// </summary>
    public class PreAdjustForceAgreeActionModel
    {
        /// <summary>
        /// 預審名單
        /// </summary>
        public IEnumerable<PreAdjustShortData> PreAdjustList { get; set; }

        /// <summary>
        /// 是否需要驗證
        /// </summary>
        public bool NeedValidate { get; set; }
    }
}