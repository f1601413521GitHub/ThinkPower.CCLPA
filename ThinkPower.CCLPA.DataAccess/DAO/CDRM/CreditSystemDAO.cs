using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 授信系統資料存取類別
    /// </summary>
    public class CreditSystemDAO : BaseDAO
    {
        /// <summary>
        /// 預審生效條件檢核
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>預審生效條件檢核結果</returns>
        public PreAdjustEffectDO PreAdjustEffectCondition(string id)
        {
            PreAdjustEffectDO result = null;

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            string query = "SP_ELGB_PAD03";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@vID", SqlDbType.NVarChar, 11)
                {
                    Value = id,
                    Direction = ParameterDirection.Input
                });

                command.Parameters.Add(new SqlParameter("@REJECTREASON", SqlDbType.NVarChar, 60)
                {
                    Direction = ParameterDirection.Output
                });

                command.Parameters.Add(new SqlParameter("@Resp_code", SqlDbType.NVarChar, 2)
                {
                    Direction = ParameterDirection.Output
                });



                connection.Open();
                command.ExecuteNonQuery();

                result = new PreAdjustEffectDO()
                {
                    RejectReason = command.Parameters["@REJECTREASON"].Value as string,
                    ResponseCode = command.Parameters["@Resp_code"].Value as string,
                };
            }

            return result;
        }
    }
}