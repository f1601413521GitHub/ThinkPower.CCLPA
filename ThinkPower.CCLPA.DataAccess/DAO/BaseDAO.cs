using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.Helper;

namespace ThinkPower.CCLPA.DataAccess.DAO
{
    /// <summary>
    /// 資料存取物件基底類別
    /// </summary>
    public abstract class BaseDAO
    {
        private SqlConnection _dbConnection;

        /// <summary>
        /// SQL資料庫連線鍵值
        /// </summary>
        protected enum Connection { ICRS, CMPN, CDRM, }

        /// <summary>
        /// SQL資料庫連線物件
        /// </summary>
        /// <param name="db">資料庫鍵值</param>
        protected SqlConnection DbConnection(Connection db)
        {
            if ((_dbConnection == null) ||
                String.IsNullOrEmpty(_dbConnection.ConnectionString))
            {
                _dbConnection = GetConnection(db);
            }

            return _dbConnection;
        }

        /// <summary>
        /// 呼叫DbHelper取得資料庫連線物件。
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private SqlConnection GetConnection(Connection db)
        {
            return DbHelper.GetConnection(ConfigurationManager.
                ConnectionStrings[$"LabCCLPA.{Enum.GetName(typeof(Connection), db)}"].ConnectionString);
        }
    }
}
