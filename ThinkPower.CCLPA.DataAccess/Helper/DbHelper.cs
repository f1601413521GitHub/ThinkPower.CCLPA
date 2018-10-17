using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.DataAccess.Helper
{
    /// <summary>
    /// 提供SQL資料庫連線處理
    /// </summary>
    public class DbHelper
    {
        /// <summary>
        /// 產生及回傳資料庫連線物件
        /// </summary>
        /// <param name="connKey">指定連線資料庫鍵值</param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string connKey)
        {
            return new SqlConnection(connKey);
        }
    }
}
