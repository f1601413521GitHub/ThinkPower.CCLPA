namespace ThinkPower.CCLPA.DataAccess.DO.CDRM
{
    /// <summary>
    /// 預審生效條件檢核結果資料物件類別
    /// </summary>
    public class PreAdjustEffectDO
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