namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 預審生效條件檢核結果類別
    /// </summary>
    public class PreAdjustEffectEntity
    {
        /// <summary>
        /// 拒絕原因代碼
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 處理代碼
        /// </summary>
        public string ResponseCode { get; set; }
    }
}