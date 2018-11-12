namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 臨調資料類別
    /// </summary>
    public class AdjustEntity : BaseEntity
    {
        /// <summary>
        /// 身分證字號
        /// </summary> 
        public string Id { get; set; }
        /// <summary>
        /// 申請日期
        /// </summary> 
        public string ApplyDate { get; set; }
        /// <summary>
        /// 申請時間
        /// </summary> 
        public string ApplyTime { get; set; }
        /// <summary>
        /// 歸戶ID
        /// </summary> 
        public string CustomerId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary> 
        public string CustomerName { get; set; }
        /// <summary>
        /// 信用額度
        /// </summary> 
        public decimal? CreditLimit { get; set; }
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
        /// 臨調原因1
        /// </summary> 
        public string Reason1 { get; set; }
        /// <summary>
        /// 臨調原因2
        /// </summary> 
        public string Reason2 { get; set; }
        /// <summary>
        /// 臨調原因3
        /// </summary> 
        public string Reason3 { get; set; }
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
        /// 預審臨調額度上限
        /// </summary> 
        public decimal? ApproveAmountMax { get; set; }
        /// <summary>
        /// 預審額度核定後可用餘額
        /// </summary> 
        public decimal? UsabilityAmount { get; set; }
        /// <summary>
        /// 建議溢繳金額
        /// </summary> 
        public decimal? OverpayAmountPro { get; set; }
        /// <summary>
        /// 核准生效金額
        /// </summary> 
        public decimal? ApproveAmount { get; set; }
        /// <summary>
        /// 溢繳金額
        /// </summary> 
        public decimal? OverpayAmount { get; set; }
        /// <summary>
        /// 評分結果
        /// </summary> 
        public string EstimateResult { get; set; }
        /// <summary>
        /// 拒絕原因代碼
        /// </summary> 
        public string RejectReason { get; set; }
        /// <summary>
        /// 申請結果
        /// </summary> 
        public string ApproveResult { get; set; }
        /// <summary>
        /// 轉授信主管註記
        /// </summary> 
        public string ChiefFlag { get; set; }
        /// <summary>
        /// 轉授信主管原因說明
        /// </summary> 
        public string ChiefRemark { get; set; }
        /// <summary>
        /// 轉Pending註記
        /// </summary> 
        public string PendingFlag { get; set; }
        /// <summary>
        /// 經辦人員ID
        /// </summary> 
        public string UserId { get; set; }
        /// <summary>
        /// 經辦姓名
        /// </summary> 
        public string UserName { get; set; }
        /// <summary>
        /// 主管人員ID
        /// </summary> 
        public string ChiefId { get; set; }
        /// <summary>
        /// 主管姓名
        /// </summary> 
        public string ChiefName { get; set; }
        /// <summary>
        /// JCIC查詢日期
        /// </summary> 
        public string JcicDate { get; set; }
        /// <summary>
        /// 1.預審臨調 2.臨調 3.專案臨調
        /// </summary> 
        public string Type { get; set; }
        /// <summary>
        /// CCAS回覆碼
        /// </summary> 
        public string CcasCode { get; set; }
        /// <summary>
        /// CCAS回覆結果
        /// </summary> 
        public string CcasStatus { get; set; }
        /// <summary>
        /// CCAS傳送回覆時間
        /// </summary> 
        public string CcasDateTime { get; set; }
        /// <summary>
        /// 處理日期
        /// </summary> 
        public string ProcessDate { get; set; }
        /// <summary>
        /// 處理時間
        /// </summary> 
        public string ProcessTime { get; set; }
        /// <summary>
        /// ICARE進件
        /// </summary> 
        public string IcareStatus { get; set; }
        /// <summary>
        /// 專案進件
        /// </summary> 
        public string ProjectStatus { get; set; }
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