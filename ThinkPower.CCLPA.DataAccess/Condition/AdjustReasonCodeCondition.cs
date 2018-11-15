namespace ThinkPower.CCLPA.DataAccess.Condition
{
    /// <summary>
    /// 調整原因代碼資料查詢條件類別
    /// </summary>
    public class AdjustReasonCodeCondition : BaseCondition
    {
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
        }

        #endregion



        #region QueryCondition

        /// <summary>
        /// 使用註記
        /// </summary>
        public string UseFlag { get; set; }

        #endregion
    }
}