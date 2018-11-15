using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.VO
{
    /// <summary>
    /// 專案臨調條件檢核結果類別
    /// </summary>
    public class AdjustValidateResult
    {
        /// <summary>
        /// 評分結果
        /// </summary>
        public string EstimateResult { get; set; }
        /// <summary>
        /// 拒絕原因代碼
        /// </summary>
        public string RejectReason { get; set; }
        /// <summary>
        /// 專案臨調結果
        /// </summary>
        public string ProjectResult { get; set; }
        /// <summary>
        /// 專案臨調拒絕原因代碼
        /// </summary>
        public string ProjectRejectReason { get; set; }
        /// <summary>
        /// 處理代碼
        /// </summary>
        public string ResponseCode { get; set; }
    }
}