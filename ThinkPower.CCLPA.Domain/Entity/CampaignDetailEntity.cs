using System;

namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 行銷活動名單類別
    /// </summary>
    public class CampaignDetailEntity : BaseEntity
    {

        /// <summary>
        /// 行銷專案代碼
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 活動序號
        /// </summary>
        public decimal Sequence { get; set; }

        /// <summary>
        /// 行動方案代碼
        /// </summary>
        public Nullable<decimal> SchemeId { get; set; }

        /// <summary>
        /// 通路代碼
        /// </summary>
        public Nullable<decimal> PathwayId { get; set; }

        /// <summary>
        /// 客群代碼
        /// </summary>
        public Nullable<decimal> GroupId { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 正式執行開始日期
        /// </summary>
        public string PerformStartDate { get; set; }

        /// <summary>
        /// 正式執行結束日期
        /// </summary>
        public string PerformEndDate { get; set; }

        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }
        public string Col11 { get; set; }
        public string Col12 { get; set; }
        public string Col13 { get; set; }
        public string Col14 { get; set; }
        public string Col15 { get; set; }
        public string Col16 { get; set; }
        public string Col17 { get; set; }
        public string Col18 { get; set; }
        public string Col19 { get; set; }
        public string Col20 { get; set; }

        /// <summary>
        /// 資料上傳日期
        /// </summary>
        public string UpdateDate { get; set; }

        /// <summary>
        /// 上傳檔名
        /// </summary>
        public string UpdateFileName { get; set; }

        public string MtnDt { get; set; }
    }
}