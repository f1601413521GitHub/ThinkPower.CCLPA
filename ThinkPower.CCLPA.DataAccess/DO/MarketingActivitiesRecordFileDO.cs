using System;

namespace ThinkPower.CCLPA.DataAccess.DO
{
    /// <summary>
    /// 行銷活動匯入紀錄檔資料物件類別
    /// </summary>
    public class MarketingActivitiesRecordFileDO
    {
        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CMPN_ID { get; set; }

        /// <summary>
        /// 預估開始執行日期
        /// </summary>
        public string CMPN_EXPC_STRT_DT { get; set; }

        /// <summary>
        /// 預估執行完成日期
        /// </summary>
        public string CMPN_EXPC_END_DT { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public Nullable<decimal> CNT { get; set; }

        /// <summary>
        /// 匯入人員代碼
        /// </summary>
        public string IMPORT_USERID { get; set; }

        /// <summary>
        /// 匯入人員姓名
        /// </summary>
        public string IMPORT_USERNAME { get; set; }

        /// <summary>
        /// 匯入日期
        /// </summary>
        public string IMPORT_DT { get; set; }
    }
}