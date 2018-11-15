using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.DataAccess.Condition
{
    /// <summary>
    /// 資料查詢基底類別
    /// </summary>
    public class BaseCondition
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
    }
}
