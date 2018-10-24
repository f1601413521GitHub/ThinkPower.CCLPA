using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.DataAccess
{
    /// <summary>
    /// 資料查詢條件類別
    /// </summary>
    public class PagingCondition
    {
        /// <summary>
        /// 資料分頁頁碼
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 資料分頁每頁筆數
        /// </summary>
        public int? PagingSize { get; set; }

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
            OrderId,
            OrderDate,
            OrderDateByDescending,
            CustomerIdAndOrderDate
        }
    }
}
