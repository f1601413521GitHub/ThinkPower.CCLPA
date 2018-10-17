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
    /// 領域物件基底類別
    /// </summary>
    public abstract class BaseDAO
    {
        private readonly string _connectionKeyICRS = "LabCCLPA.ICRS";
        private readonly string _connectionKeyCMPN = "LabCCLPA.CMPN";
        private readonly string _connectionKeyCDRM = "LabCCLPA.CDRM";

        private SqlConnection _dbConnectionICRS;
        private SqlConnection _dbConnectionCMPN;
        private SqlConnection _dbConnectionCDRM;


        /// <summary>
        /// ICRS資料庫連線物件
        /// </summary>
        public SqlConnection DbConnectionICRS
        {
            get
            {
                if ((_dbConnectionICRS == null) ||
                    String.IsNullOrEmpty(_dbConnectionICRS.ConnectionString))
                {
                    _dbConnectionICRS = GetConnection(_connectionKeyICRS);
                }

                return _dbConnectionICRS;
            }
        }

        /// <summary>
        /// CMPN資料庫連線物件
        /// </summary>
        public SqlConnection DbConnectionCMPN
        {
            get
            {
                if ((_dbConnectionCMPN == null) ||
                    String.IsNullOrEmpty(_dbConnectionCMPN.ConnectionString))
                {
                    _dbConnectionCMPN = GetConnection(_connectionKeyCMPN);
                }

                return _dbConnectionCMPN;
            }
        }

        /// <summary>
        /// CDRM資料庫連線物件
        /// </summary>
        public SqlConnection DbConnectionCDRM
        {
            get
            {
                if ((_dbConnectionCDRM == null) ||
                    String.IsNullOrEmpty(_dbConnectionCDRM.ConnectionString))
                {
                    _dbConnectionCDRM = GetConnection(_connectionKeyCDRM);
                }

                return _dbConnectionCDRM;
            }
        }

        /// <summary>
        /// 取得資料筆數
        /// </summary>
        public abstract int Count();

        /// <summary>
        /// 呼叫DbHelper取得資料庫連線物件。
        /// </summary>
        /// <param name="key">資料庫連線鍵值</param>
        /// <returns></returns>
        private SqlConnection GetConnection(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(key);
            }

            return DbHelper.GetConnection(ConfigurationManager.ConnectionStrings[key].ConnectionString);
        }
    }
}
