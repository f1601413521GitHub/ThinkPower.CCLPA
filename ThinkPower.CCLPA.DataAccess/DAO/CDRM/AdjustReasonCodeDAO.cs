using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 調整原因代碼檔資料存取類別
    /// </summary>
    public class AdjustReasonCodeDAO : BaseDAO
    {
        /// <summary>
        /// 取出所有調整原因代碼
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdjustReasonCodeDO> GetAll()
        {
            List<AdjustReasonCodeDO> result = null;

            string query = @"
SELECT 
    [CODE],[NAME],[USE_YN]
FROM [COD_RG_REASON];";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<AdjustReasonCodeDO>();
                    AdjustReasonCodeDO item = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        item = ConvertIncreaseReasonCodeDO(dr);

                        result.Add(item);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result ?? new List<AdjustReasonCodeDO>();
        }

        /// <summary>
        /// 轉換調整原因代碼
        /// </summary>
        /// <param name="reasonCode">調整原因代碼</param>
        /// <returns></returns>
        private AdjustReasonCodeDO ConvertIncreaseReasonCodeDO(DataRow reasonCode)
        {
            return new AdjustReasonCodeDO()
            {
                Code = reasonCode.Field<string>("CODE"),
                Name = reasonCode.Field<string>("NAME"),
                UseFlag = reasonCode.Field<string>("USE_YN"),
            };
        }
    }
}