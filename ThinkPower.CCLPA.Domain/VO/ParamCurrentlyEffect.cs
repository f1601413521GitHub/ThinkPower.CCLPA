using System;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 專案參數目前生效主檔資料類別
    /// </summary>
    public class ParamCurrentlyEffect
    {
        /// <summary> 
        /// 臨調原因 
        /// </summary>
        public string Reason { get; set; }
        /// <summary> 
        /// 生效日期 
        /// </summary>
        public string EffectDate { get; set; }
        /// <summary> 
        /// 有效日期(起) 
        /// </summary>
        public string AdjustDateStart { get; set; }
        /// <summary> 
        /// 有效日期(迄) 
        /// </summary>
        public string AdjustDateEnd { get; set; }
        /// <summary> 
        /// 金額上限 
        /// </summary>
        public Nullable<decimal> ApproveAmountMax { get; set; }
        /// <summary> 
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary> 
        /// 檢核條件 
        /// </summary>
        public string VerifiyCondition { get; set; }
        /// <summary> 
        /// 額度上限% 
        /// </summary>
        public Nullable<decimal> ApproveScaleMax { get; set; }
    }
}