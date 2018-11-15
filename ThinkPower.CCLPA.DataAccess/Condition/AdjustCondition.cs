﻿namespace ThinkPower.CCLPA.DataAccess.Condition
{
    /// <summary>
    /// 專案臨調資料查詢條件類別
    /// </summary>
    public class AdjustCondition
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
            ProcessDateByDescendingAndProcessTimeByDescending = 1,
        }

        #endregion



        #region QueryCondition

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 轉授信主管註記
        /// </summary>
        public string ChiefFlag { get; set; }
        /// <summary>
        /// 轉Pending註記
        /// </summary>
        public string PendingFlag { get; set; }
        /// <summary>
        /// 專案進件
        /// </summary>
        public string ProjectStatus { get; set; }

        #endregion
    }
}