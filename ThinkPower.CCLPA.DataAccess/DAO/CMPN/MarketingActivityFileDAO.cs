using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO;

namespace ThinkPower.CCLPA.DataAccess.DAO.CMPN
{
    /// <summary>
    /// 行銷活動檔資料存取類別
    /// </summary>
    public class MarketingActivityFileDAO : BaseDAO
    {
        /// <summary>
        /// 取得資料筆數
        /// </summary>
        /// <returns>資料筆數</returns>
        public override int Count()
        {
            int count = 0;

            using (SqlConnection connection = DbConnectionCMPN)
            {
                SqlCommand command = new SqlCommand("SELECT Count(1) FROM CMPN", connection);

                connection.Open();

                count = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return count;
        }

        /// <summary>
        /// 取得行銷活動資訊
        /// </summary>
        /// <param name="cmpnId">行銷活動編號</param>
        /// <returns>行銷活動資訊</returns>
        public MarketingActivityFileDO Get(string cmpnId)
        {
            MarketingActivityFileDO result = null;

            if (String.IsNullOrEmpty(cmpnId))
            {
                throw new ArgumentNullException("cmpnId");
            }

            string query = @"
SELECT 
    [CMPN_ID],[CMPN_NM],[CMPN_DSC],[CMPN_TP_ID],[INL_OU_ID],[INL_EMPE_NO],[CMPN_PROM_PRD_ID],
    [CMPN_FILTER_TYPID],[CMPN_EXPC_STRT_DT],[CMPN_EXPC_END_DT],[CMPN_EXPC_CLOSE_DT],[CMPN_DETAIL_DSC],
    [CMPN_EXPC_CNL_ID],[CMPN_FRQ_TP_ID],[CMPN_BASE_DSC],[CMPN_APPROVE_STS],[CMPN_ASSIGN_MIS],
    [CREATED_DT],[LST_MTN_DT],[CRSS_FLG]
FROM [CMPN]
WHERE CMPN_ID = @CMPN_ID;";

            using (SqlConnection connection = DbConnectionCMPN)
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
        /// 轉換行銷活動資訊
        /// </summary>
        /// <param name="activityInfo">行銷活動資訊</param>
        /// <returns>行銷活動資訊</returns>
        private MarketingActivityFileDO ConvertDataObject(DataRow activityInfo)
        {
            return new MarketingActivityFileDO()
            {
                CMPN_ID = activityInfo.Field<string>("CMPN_ID"),
                CMPN_NM = activityInfo.Field<string>("CMPN_NM"),
                CMPN_DSC = activityInfo.Field<string>("CMPN_DSC"),
                CMPN_TP_ID = activityInfo.Field<decimal?>("CMPN_TP_ID"),
                INL_OU_ID = activityInfo.Field<decimal?>("INL_OU_ID"),
                INL_EMPE_NO = activityInfo.Field<string>("INL_EMPE_NO"),
                CMPN_PROM_PRD_ID = activityInfo.Field<string>("CMPN_PROM_PRD_ID"),
                CMPN_FILTER_TYPID = activityInfo.Field<decimal?>("CMPN_FILTER_TYPID"),
                CMPN_EXPC_STRT_DT = activityInfo.Field<string>("CMPN_EXPC_STRT_DT"),
                CMPN_EXPC_END_DT = activityInfo.Field<string>("CMPN_EXPC_END_DT"),
                CMPN_EXPC_CLOSE_DT = activityInfo.Field<string>("CMPN_EXPC_CLOSE_DT"),
                CMPN_DETAIL_DSC = activityInfo.Field<string>("CMPN_DETAIL_DSC"),
                CMPN_EXPC_CNL_ID = activityInfo.Field<decimal?>("CMPN_EXPC_CNL_ID"),
                CMPN_FRQ_TP_ID = activityInfo.Field<decimal?>("CMPN_FRQ_TP_ID"),
                CMPN_BASE_DSC = activityInfo.Field<string>("CMPN_BASE_DSC"),
                CMPN_APPROVE_STS = activityInfo.Field<decimal?>("CMPN_APPROVE_STS"),
                CMPN_ASSIGN_MIS = activityInfo.Field<string>("CMPN_ASSIGN_MIS"),
                CREATED_DT = activityInfo.Field<string>("CREATED_DT"),
                LST_MTN_DT = activityInfo.Field<DateTime?>("LST_MTN_DT"),
                CRSS_FLG = activityInfo.Field<string>("CRSS_FLG"),
            };
        }
    }
}
