using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;

namespace ThinkPower.CCLPA.DataAccess.DAO.CMPN
{
    /// <summary>
    /// 行銷活動檔資料存取類別
    /// </summary>
    public class CampaignDAO : BaseDAO
    {
        /// <summary>
        /// 取得行銷活動資訊
        /// </summary>
        /// <param name="campaignId">行銷活動編號</param>
        /// <returns>行銷活動資訊</returns>
        public CampaignDO Get(string campaignId)
        {
            CampaignDO result = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            string query = @"
SELECT 
    [CMPN_ID],[CMPN_NM],[CMPN_DSC],[CMPN_TP_ID],[INL_OU_ID],[INL_EMPE_NO],[CMPN_PROM_PRD_ID],
    [CMPN_FILTER_TYPID],[CMPN_EXPC_STRT_DT],[CMPN_EXPC_END_DT],[CMPN_EXPC_CLOSE_DT],[CMPN_DETAIL_DSC],
    [CMPN_EXPC_CNL_ID],[CMPN_FRQ_TP_ID],[CMPN_BASE_DSC],[CMPN_APPROVE_STS],[CMPN_ASSIGN_MIS],
    [CREATED_DT],[LST_MTN_DT],[CRSS_FLG]
FROM [CMPN]
WHERE CMPN_ID = @CampaignId;";

            using (SqlConnection connection = DbConnection(Connection.CMPN))
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
                    result = ConvertCampaignDO(dt.Rows[0]);
                }
                else if(dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("Campaign not the only");
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
        /// <param name="campaignInfo">行銷活動資訊</param>
        /// <returns>行銷活動資訊</returns>
        private CampaignDO ConvertCampaignDO(DataRow campaignInfo)
        {
            return new CampaignDO()
            {
                CampaignId = campaignInfo.Field<string>("CMPN_ID"),
                CampaignName = campaignInfo.Field<string>("CMPN_NM"),
                CampaignDescript = campaignInfo.Field<string>("CMPN_DSC"),
                CampaignTypeId = campaignInfo.Field<decimal?>("CMPN_TP_ID"),
                ProposalUnitNo = campaignInfo.Field<decimal?>("INL_OU_ID"),
                ProposalEmployeeNo = campaignInfo.Field<string>("INL_EMPE_NO"),
                ProductId = campaignInfo.Field<string>("CMPN_PROM_PRD_ID"),
                SortPrinciple = campaignInfo.Field<decimal?>("CMPN_FILTER_TYPID"),
                ExpectedStartDateTime = campaignInfo.Field<string>("CMPN_EXPC_STRT_DT"),
                ExpectedEndDateTime = campaignInfo.Field<string>("CMPN_EXPC_END_DT"),
                ExpectedCloseDate = campaignInfo.Field<string>("CMPN_EXPC_CLOSE_DT"),
                DetailDescript = campaignInfo.Field<string>("CMPN_DETAIL_DSC"),
                ExecutionChannel = campaignInfo.Field<decimal?>("CMPN_EXPC_CNL_ID"),
                ActivityFrequency = campaignInfo.Field<decimal?>("CMPN_FRQ_TP_ID"),
                BaseDescript = campaignInfo.Field<string>("CMPN_BASE_DSC"),
                ApproveState = campaignInfo.Field<decimal?>("CMPN_APPROVE_STS"),
                AssignMIS = campaignInfo.Field<string>("CMPN_ASSIGN_MIS"),
                CreatedDate = campaignInfo.Field<string>("CREATED_DT"),
                LastMaintenanceDate = campaignInfo.Field<DateTime?>("LST_MTN_DT"),
                CrossSellProposalNotes = campaignInfo.Field<string>("CRSS_FLG"),
            };
        }
    }
}
