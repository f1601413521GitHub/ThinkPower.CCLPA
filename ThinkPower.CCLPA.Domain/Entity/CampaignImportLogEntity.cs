using System;

namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 行銷活動匯入紀錄類別
    /// </summary>
    public class CampaignImportLogEntity
    {

        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 預估開始執行日期
        /// </summary>
        public string ExpectedStartDate { get; set; }

        /// <summary>
        /// 預估執行完成日期
        /// </summary>
        public string ExpectedEndDate { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public Nullable<decimal> Count { get; set; }

        /// <summary>
        /// 匯入人員代碼
        /// </summary>
        public string ImportUserId { get; set; }

        /// <summary>
        /// 匯入人員姓名
        /// </summary>
        public string ImportUserName { get; set; }

        /// <summary>
        /// 匯入日期
        /// </summary>
        public string ImportDate { get; set; }
    }
}