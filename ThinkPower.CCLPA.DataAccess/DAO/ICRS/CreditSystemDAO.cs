using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;

namespace ThinkPower.CCLPA.DataAccess.DAO.ICRS
{
    /// <summary>
    /// CCAS授信系統資料存取類別
    /// </summary>
    public class CreditSystemDAO : BaseDAO
    {
        /// <summary>
        /// 所得稅卡戶臨調檢核
        /// </summary>
        /// <param name="adjustInfo">所得稅卡戶臨調資料</param>
        /// <returns></returns>
        public string IncomeTaxCardAdjust(IncomeTaxCardAdjustDO adjustInfo)
        {
            string result = null;

            if (adjustInfo == null)
            {
                throw new ArgumentNullException("id");
            }

            string query = "SP_ICRS_TO_CCAS_ADJ_TAX";

            using (SqlConnection connection = DbConnection(Connection.ICRS))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Aadj_action", SqlDbType.NVarChar) { Value = adjustInfo.ActionCode, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Acard_acct_id", SqlDbType.NVarChar) { Value = adjustInfo.CustomerId, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Acard_acct_id_seq", SqlDbType.NVarChar) { Value = adjustInfo.CustomerIdNo, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Aadj_proj_code", SqlDbType.NVarChar) { Value = adjustInfo.ProjectName, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Aadj_amt", SqlDbType.Decimal, 17) { Value = adjustInfo.IncomeTaxAdjustAmount ?? Convert.DBNull, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Aadj_effend_date", SqlDbType.NVarChar) { Value = adjustInfo.AdjustCloseDate, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Aadj_user", SqlDbType.NVarChar) { Value = adjustInfo.AdjustUserId, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Aresp_code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output });

                connection.Open();
                command.ExecuteNonQuery();

                string responseCode = command.Parameters["@Aresp_code"].Value as string;

                if (String.IsNullOrEmpty(responseCode))
                {
                    throw new InvalidOperationException("responseCode not found");
                }

                result = responseCode;
            }

            return result;
        }
    }
}