namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 調整原因類別
    /// </summary>
    public class AdjustReason
    {
        /// <summary>
        /// 調整原因代碼
        /// </summary>
        public AdjustReasonCode ReasonCode { get; set; }
        /// <summary>
        /// 調整原因生效資訊
        /// </summary>
        public ParamCurrentlyEffect ReasonEffectInfo { get; set; }
    }
}