namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 臨調紀錄類別
    /// </summary>
    public class LogProgress
    {
        /// <summary> 
        /// 申請書編號
        /// </summary>
        public string ApplicationNo { get; set; }
        /// <summary> 
        /// 申請書種類 
        /// </summary>
        public string ApplicationKind { get; set; }
        /// <summary> 
        /// 處理代碼 
        /// </summary>
        public string ProgressCode { get; set; }
        /// <summary> 
        /// 處理人員代號 
        /// </summary>
        public string ProgressId { get; set; }
        /// <summary> 
        /// 處理人員姓名 
        /// </summary>
        public string ProgressName { get; set; }
        /// <summary> 
        /// 處理日期 
        /// </summary>
        public string ProgressDate { get; set; }
        /// <summary> 
        /// 處理時間 
        /// </summary>
        public string ProgressTime { get; set; }
        /// <summary> 
        /// 序號 
        /// </summary>
        public decimal? SerialNo { get; set; }
        /// <summary> 
        /// 備註 
        /// </summary>
        public string Memo { get; set; }
    }
}