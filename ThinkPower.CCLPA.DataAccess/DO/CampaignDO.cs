﻿using System;

namespace ThinkPower.CCLPA.DataAccess.DO
{
    /// <summary>
    /// 行銷活動檔資料物件類別
    /// </summary>
    public class CampaignDO
    {
        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 行銷活動名稱
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        /// 行銷專案描述
        /// </summary>
        public string CampaignDescript { get; set; }

        /// <summary>
        /// 行銷活動類型
        /// </summary>
        public Nullable<decimal> CampaignTypeId { get; set; }

        /// <summary>
        /// 提案單位
        /// </summary>
        public Nullable<decimal> ProposalUnitNo { get; set; }

        /// <summary>
        /// 提案人
        /// </summary>
        public string ProposalEmployeeNo { get; set; }

        /// <summary>
        /// 行銷產品
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 排序原則
        /// </summary>
        public Nullable<decimal> SortPrinciple { get; set; }

        /// <summary>
        /// 預估開始執行時間
        /// </summary>
        public string ExpectedStartDateTime { get; set; }

        /// <summary>
        /// 預估執行完成時間
        /// </summary>
        public string ExpectedEndDateTime { get; set; }

        /// <summary>
        /// 預估結案日期
        /// </summary>
        public string ExpectedCloseDate { get; set; }

        /// <summary>
        /// 行銷活動內容
        /// </summary>
        public string DetailDescript { get; set; }

        /// <summary>
        /// 預估執行通路
        /// </summary>
        public Nullable<decimal> ExecutionPathway { get; set; }

        /// <summary>
        /// 活動頻率
        /// </summary>
        public Nullable<decimal> ActivityFrequency { get; set; }

        /// <summary>
        /// 母體資料說明
        /// </summary>
        public string BaseDescript { get; set; }

        /// <summary>
        /// 覆核狀態
        /// </summary>
        public Nullable<decimal> ApproveState { get; set; }

        /// <summary>
        /// mis處理人員
        /// </summary>
        public string AssignMIS { get; set; }

        /// <summary>
        /// 初始提案日期
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// 最後維護日期
        /// </summary>
        public Nullable<System.DateTime> LastMaintenanceDate { get; set; }

        /// <summary>
        /// 交叉銷售提案註記
        /// </summary>
        public string CrossSellProposalNotes { get; set; }
    }
}