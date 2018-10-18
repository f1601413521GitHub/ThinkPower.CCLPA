using System;
using System.Data;
using System.Data.SqlClient;

namespace ThinkPower.CCLPA.DataAccess.DAO.CMPN
{
    /// <summary>
    /// 行銷活動名單檔資料存取類別
    /// </summary>
    public class CampaignListDAO : BaseDAO
    {
        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <param name="campaignId">行銷活動編號</param>
        /// <returns>行銷活動名單數量</returns>
        public int Count(string campaignId)
        {
            int result = 0;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            string query = @"
SELECT COUNT(1)
FROM [CMPN_AVY_CNL_EXBOOK]
WHERE CMPN_ID = @CampaignId;";

            using (SqlConnection connection = DbConnection(Connection.CMPN))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.VarChar)
                {
                    Value = campaignId
                });

                connection.Open();

                result = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return result;
        }
    }
}