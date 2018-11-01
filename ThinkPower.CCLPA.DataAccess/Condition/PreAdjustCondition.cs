﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.DataAccess.Condition
{
    /// <summary>
    /// 臨調預審名單資料查詢條件類別
    /// </summary>
    public class PreAdjustCondition
    {
        #region PagingCondition

        /// <summary>
        /// 資料分頁頁碼
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 資料分頁每頁筆數
        /// </summary>
        public int? PagingSize { get; set; }

        #endregion




        #region SortCondition

        /// <summary>
        /// 排序方式
        /// </summary>
        public OrderByKind OrderBy { get; set; }

        /// <summary>
        /// 資料排序方式列舉
        /// </summary>
        public enum OrderByKind
        {
            None = 0,
            CustomerId,
            ImportDate,
            ImportDateByDescending,
            CustomerIdAndImportDate
        }

        #endregion




        #region QueryCondition

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// CCAS回覆碼
        /// </summary>
        public string CcasReplyCode { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 行銷活動代碼
        /// </summary>
        public string CampaignId { get; set; }

        #endregion
    }
}
