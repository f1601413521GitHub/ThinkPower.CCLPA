using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 行銷活動匯入紀錄檔資料存取類別
    /// </summary>
    public class MarketingActivitiesRecordFileDAO : BaseDAO
    {
        /// <summary>
        /// 取得行銷活動匯入紀錄檔
        /// </summary>
        /// <param name="cmpnId">行銷活動編號</param>
        /// <returns>行銷活動匯入紀錄檔</returns>
        public MarketingActivitiesRecordFileDO Get(string cmpnId)
        {
            MarketingActivitiesRecordFileDO result = null;

            if (String.IsNullOrEmpty(cmpnId))
            {
                throw new ArgumentNullException("cmpnId");
            }

            string query = @"
SELECT
    [CMPN_ID],[CMPN_EXPC_STRT_DT],[CMPN_EXPC_END_DT],[CNT],
    [IMPORT_USERID],[IMPORT_USERNAME],[IMPORT_DT]
FROM [LOG_RG_ILRC]
WHERE CMPN_ID = @CMPN_ID;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CMPN_ID", SqlDbType.VarChar) { Value = cmpnId });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertDataObject(dt.Rows[0]);
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換行銷活動匯入紀錄資訊
        /// </summary>
        /// <param name="recordInfo">行銷活動匯入紀錄資訊</param>
        /// <returns>行銷活動匯入紀錄資訊</returns>
        private MarketingActivitiesRecordFileDO ConvertDataObject(DataRow recordInfo)
        {
            return new MarketingActivitiesRecordFileDO()
            {
                CMPN_ID = recordInfo.Field<string>("CMPN_ID"),
                CMPN_EXPC_STRT_DT = recordInfo.Field<string>("CMPN_EXPC_STRT_DT"),
                CMPN_EXPC_END_DT = recordInfo.Field<string>("CMPN_EXPC_END_DT"),
                CNT = recordInfo.Field<decimal?>("CNT"),
                IMPORT_USERID = recordInfo.Field<string>("IMPORT_USERID"),
                IMPORT_USERNAME = recordInfo.Field<string>("IMPORT_USERNAME"),
                IMPORT_DT = recordInfo.Field<string>("IMPORT_DT"),
            };
        }
    }
}
