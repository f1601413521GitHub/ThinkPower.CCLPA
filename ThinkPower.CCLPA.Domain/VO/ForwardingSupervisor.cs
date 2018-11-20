namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 轉授信主管資料類別
    /// </summary>
    public class ForwardingSupervisor
    {
        /// <summary>
        /// 歸戶ID
        /// </summary> 
        public string CustomerId { get; set; }
        /// <summary>
        /// 申請金額
        /// </summary> 
        public decimal? ApplyAmount { get; set; }
        /// <summary>
        /// 使用地點
        /// </summary> 
        public string UseSite { get; set; }
        /// <summary>
        /// 出國地點
        /// </summary> 
        public string Place { get; set; }
        /// <summary>
        /// 有效日期(起)
        /// </summary> 
        public string AdjustDateStart { get; set; }
        /// <summary>
        /// 有效日期(迄)
        /// </summary> 
        public string AdjustDateEnd { get; set; }
        /// <summary>
        /// 臨調原因彙總
        /// </summary> 
        public string Reason { get; set; }
        /// <summary>
        /// 調高原因備註
        /// </summary> 
        public string Remark { get; set; }
        /// <summary>
        /// 是否可人工授權
        /// </summary> 
        public string ForceAuthenticate { get; set; }
        /// <summary>
        /// 評分結果
        /// </summary> 
        public string EstimateResult { get; set; }
        /// <summary>
        /// 拒絕原因代碼
        /// </summary> 
        public string RejectReason { get; set; }
        /// <summary>
        /// 轉授信主管原因說明
        /// </summary> 
        public string ChiefRemark { get; set; }
        /// <summary>
        /// JCIC查詢日期
        /// </summary> 
        public string JcicDate { get; set; }
        /// <summary>
        /// 專案臨調結果
        /// </summary> 
        public string ProjectAdjustResult { get; set; }
        /// <summary>
        /// 專案臨調拒絕原因代碼
        /// </summary> 
        public string ProjectAdjustRejectReason { get; set; }
        /// <summary>
        /// 刷卡金額(不含額度)
        /// </summary> 
        public decimal? CreditAmount { get; set; }
    }
}