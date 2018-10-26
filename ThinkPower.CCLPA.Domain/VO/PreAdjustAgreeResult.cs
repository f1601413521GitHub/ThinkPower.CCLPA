namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 同意執行預審名單處理結果類別
    /// </summary>
    public class PreAdjustAgreeResult
    {
        /// <summary>
        /// 生效筆數
        /// </summary>
        public int EffectCount { get; set; }

        /// <summary>
        /// 失敗筆數
        /// </summary>
        public int FailCount { get; set; }
    }
}