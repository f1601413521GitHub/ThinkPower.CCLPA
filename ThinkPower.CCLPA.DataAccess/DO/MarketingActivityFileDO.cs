using System;

namespace ThinkPower.CCLPA.DataAccess.DO
{
    /// <summary>
    /// 行銷活動檔資料物件類別
    /// </summary>
    public class MarketingActivityFileDO
    {
        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CMPN_ID { get; set; }

        /// <summary>
        /// 行銷活動名稱
        /// </summary>
        public string CMPN_NM { get; set; }

        /// <summary>
        /// 行銷專案描述
        /// </summary>
        public string CMPN_DSC { get; set; }

        /// <summary>
        /// 行銷活動類型
        /// </summary>
        public Nullable<decimal> CMPN_TP_ID { get; set; }

        /// <summary>
        /// 提案單位
        /// </summary>
        public Nullable<decimal> INL_OU_ID { get; set; }

        /// <summary>
        /// 提案人
        /// </summary>
        public string INL_EMPE_NO { get; set; }

        /// <summary>
        /// 行銷產品
        /// </summary>
        public string CMPN_PROM_PRD_ID { get; set; }

        /// <summary>
        /// 排序原則
        /// </summary>
        public Nullable<decimal> CMPN_FILTER_TYPID { get; set; }

        /// <summary>
        /// 預估開始執行時間
        /// </summary>
        public string CMPN_EXPC_STRT_DT { get; set; }

        /// <summary>
        /// 預估執行完成時間
        /// </summary>
        public string CMPN_EXPC_END_DT { get; set; }

        /// <summary>
        /// 預估結案日期
        /// </summary>
        public string CMPN_EXPC_CLOSE_DT { get; set; }

        /// <summary>
        /// 行銷活動內容
        /// </summary>
        public string CMPN_DETAIL_DSC { get; set; }

        /// <summary>
        /// 預估執行通路
        /// </summary>
        public Nullable<decimal> CMPN_EXPC_CNL_ID { get; set; }

        /// <summary>
        /// 活動頻率
        /// </summary>
        public Nullable<decimal> CMPN_FRQ_TP_ID { get; set; }

        /// <summary>
        /// 母體資料說明
        /// </summary>
        public string CMPN_BASE_DSC { get; set; }

        /// <summary>
        /// 覆核狀態
        /// </summary>
        public Nullable<decimal> CMPN_APPROVE_STS { get; set; }

        /// <summary>
        /// mis處理人員
        /// </summary>
        public string CMPN_ASSIGN_MIS { get; set; }

        /// <summary>
        /// 初始提案日期
        /// </summary>
        public string CREATED_DT { get; set; }

        /// <summary>
        /// 最後維護日期
        /// </summary>
        public Nullable<System.DateTime> LST_MTN_DT { get; set; }

        /// <summary>
        /// 交叉銷售提案註記
        /// </summary>
        public string CRSS_FLG { get; set; }
    }
}