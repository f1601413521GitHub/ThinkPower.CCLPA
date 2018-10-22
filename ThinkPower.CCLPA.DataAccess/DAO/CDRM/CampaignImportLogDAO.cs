﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 行銷活動匯入紀錄檔資料存取類別
    /// </summary>
    public class CampaignImportLogDAO : BaseDAO
    {
        /// <summary>
        /// 取得行銷活動匯入紀錄檔
        /// </summary>
        /// <param name="campaignId">行銷活動編號</param>
        /// <returns>行銷活動匯入紀錄檔</returns>
        public CampaignImportLogDO Get(string campaignId)
        {
            CampaignImportLogDO result = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            string query = @"
SELECT
    [CMPN_ID],[CMPN_EXPC_STRT_DT],[CMPN_EXPC_END_DT],[CNT],
    [IMPORT_USERID],[IMPORT_USERNAME],[IMPORT_DT]
FROM [LOG_RG_ILRC]
WHERE CMPN_ID = @CampaignId;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar)
                {
                    Value = campaignId
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertCampaignImportLogDO(dt.Rows[0]);
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
        /// <param name="importLog">行銷活動匯入紀錄資訊</param>
        /// <returns>行銷活動匯入紀錄資訊</returns>
        private CampaignImportLogDO ConvertCampaignImportLogDO(DataRow importLog)
        {
            return new CampaignImportLogDO()
            {
                CampaignId = importLog.Field<string>("CMPN_ID"),
                ExpectedStartDate = importLog.Field<string>("CMPN_EXPC_STRT_DT"),
                ExpectedEndDate = importLog.Field<string>("CMPN_EXPC_END_DT"),
                Count = importLog.Field<decimal?>("CNT"),
                ImportUserId = importLog.Field<string>("IMPORT_USERID"),
                ImportUserName = importLog.Field<string>("IMPORT_USERNAME"),
                ImportDate = importLog.Field<string>("IMPORT_DT"),
            };
        }

        /// <summary>
        /// 新增行銷活動匯入資料
        /// </summary>
        /// <param name="importLog">行銷活動匯入資料</param>
        public void Insert(CampaignImportLogDO importLog)
        {
            if (importLog == null)
            {
                throw new ArgumentNullException("importLog");
            }

            string query = @"
INSERT INTO [LOG_RG_ILRC]
    ([CMPN_ID],[CMPN_EXPC_STRT_DT],[CMPN_EXPC_END_DT],[CNT],[IMPORT_USERID],[IMPORT_USERNAME],[IMPORT_DT])
VALUES
    (@CampaignId,@ExpectedStartDate,@ExpectedEndDate,@Count,@ImportUserId,@ImportUserName,@ImportDate);";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar) { Value = importLog.CampaignId });
                command.Parameters.Add(new SqlParameter("@ExpectedStartDate", SqlDbType.NVarChar) { Value = importLog.ExpectedStartDate });
                command.Parameters.Add(new SqlParameter("@ExpectedEndDate", SqlDbType.NVarChar) { Value = importLog.ExpectedEndDate });
                command.Parameters.Add(new SqlParameter("@Count", SqlDbType.Decimal) { Value = importLog.Count ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ImportUserId", SqlDbType.NVarChar) { Value = importLog.ImportUserId });
                command.Parameters.Add(new SqlParameter("@ImportUserName", SqlDbType.NVarChar) { Value = importLog.ImportUserName });
                command.Parameters.Add(new SqlParameter("@ImportDate", SqlDbType.NVarChar) { Value = importLog.ImportDate });

                connection.Open();
                command.ExecuteNonQuery();

                command = null;
            }
        }
    }
}
