using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;

namespace ThinkPower.CCLPA.DataAccess.DAO.CMPN
{
    /// <summary>
    /// 行銷活動名單檔資料存取類別
    /// </summary>
    public class CampaignDetailDAO : BaseDAO
    {
        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <param name="campaignId">行銷活動編號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns>行銷活動名單數量</returns>
        public int Count(string campaignId, decimal? executionPathway)
        {
            int result = 0;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException(nameof(campaignId));
            }

            string query = @"
SELECT COUNT(1)
FROM [CMPN_AVY_CNL_EXBOOK]
WHERE CMPN_ID = @CampaignId
    AND CMPN_CNL_ID = @ExecutionPathway;";

            using (SqlConnection connection = DbConnection(Connection.CMPN))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar)
                {
                    Value = campaignId
                });
                command.Parameters.Add(new SqlParameter("@ExecutionPathway", SqlDbType.Decimal)
                {
                    Value = executionPathway
                });

                connection.Open();

                result = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return result;
        }

        /// <summary>
        /// 取得行銷活動名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        public IEnumerable<CampaignDetailDO> Get(string campaignId, decimal? executionPathway)
        {
            List<CampaignDetailDO> result = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException(nameof(campaignId));
            }

            string query = @"
SELECT 
    [CMPN_ID],[CMPN_SEQ],[CMPN_AVY_ID],[CMPN_CNL_ID],[CMPN_CELL_ID],[CUSTOMER_ID],[CMPN_ACT_STRT_DT],
    [CMPN_ACT_END_DT],[COL_1],[COL_2],[COL_3],[COL_4],[COL_5],[COL_6],[COL_7],[COL_8],[COL_9],[COL_10],
    [COL_11],[COL_12],[COL_13],[COL_14],[COL_15],[COL_16],[COL_17],[COL_18],[COL_19],[COL_20],[UPDT_DT],
    [UPL_FILE_NM],[MTN_DT]
FROM [CMPN_AVY_CNL_EXBOOK]
WHERE CMPN_ID = @CampaignId
    AND CMPN_CNL_ID = @ExecutionPathway;";

            using (SqlConnection connection = DbConnection(Connection.CMPN))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar)
                {
                    Value = campaignId
                });
                command.Parameters.Add(new SqlParameter("@ExecutionPathway", SqlDbType.Decimal)
                {
                    Value = executionPathway
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<CampaignDetailDO>();

                    CampaignDetailDO campaignListDO = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        campaignListDO = null;
                        campaignListDO = ConvertCampaignListDO(dr);
                        result.Add(campaignListDO);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換行銷活動名單資料
        /// </summary>
        /// <param name="campaignDetail">行銷活動名單資料</param>
        /// <returns></returns>
        private CampaignDetailDO ConvertCampaignListDO(DataRow campaignDetail)
        {
            return new CampaignDetailDO()
            {
                CampaignId = campaignDetail.Field<string>("CMPN_ID"),
                Sequence = campaignDetail.Field<decimal>("CMPN_SEQ"),
                SchemeId = campaignDetail.Field<decimal?>("CMPN_AVY_ID"),
                PathwayId = campaignDetail.Field<decimal?>("CMPN_CNL_ID"),
                GroupId = campaignDetail.Field<decimal?>("CMPN_CELL_ID"),
                CustomerId = campaignDetail.Field<string>("CUSTOMER_ID"),
                PerformStartDate = campaignDetail.Field<string>("CMPN_ACT_STRT_DT"),
                PerformEndDate = campaignDetail.Field<string>("CMPN_ACT_END_DT"),
                Col1 = campaignDetail.Field<string>("COL_1"),
                Col2 = campaignDetail.Field<string>("COL_2"),
                Col3 = campaignDetail.Field<string>("COL_3"),
                Col4 = campaignDetail.Field<string>("COL_4"),
                Col5 = campaignDetail.Field<string>("COL_5"),
                Col6 = campaignDetail.Field<string>("COL_6"),
                Col7 = campaignDetail.Field<string>("COL_7"),
                Col8 = campaignDetail.Field<string>("COL_8"),
                Col9 = campaignDetail.Field<string>("COL_9"),
                Col10 = campaignDetail.Field<string>("COL_10"),
                Col11 = campaignDetail.Field<string>("COL_11"),
                Col12 = campaignDetail.Field<string>("COL_12"),
                Col13 = campaignDetail.Field<string>("COL_13"),
                Col14 = campaignDetail.Field<string>("COL_14"),
                Col15 = campaignDetail.Field<string>("COL_15"),
                Col16 = campaignDetail.Field<string>("COL_16"),
                Col17 = campaignDetail.Field<string>("COL_17"),
                Col18 = campaignDetail.Field<string>("COL_18"),
                Col19 = campaignDetail.Field<string>("COL_19"),
                Col20 = campaignDetail.Field<string>("COL_20"),
                UpdateDate = campaignDetail.Field<string>("UPDT_DT"),
                UpdateFileName = campaignDetail.Field<string>("UPL_FILE_NM"),
                MtnDt = campaignDetail.Field<string>("MTN_DT"),
            };
        }
    }
}