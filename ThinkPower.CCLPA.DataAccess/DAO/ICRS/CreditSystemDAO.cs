using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.VO;

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
        public string IncomeTaxCardAdjust(IncomeTaxCardAdjust adjustInfo)
        {
            string result = null;

            if (adjustInfo == null)
            {
                throw new ArgumentNullException(nameof(adjustInfo));
            }

            string query = "SP_ICRS_TO_CCAS_ADJ_TAX";

            using (SqlConnection connection = DbConnection(Connection.ICRS))
            {
                SqlCommand command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

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


        /// <summary>
        /// 查詢ICRS掛帳金額 (含已授權未清算)、可用額度
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="serialNo">客戶ID序號</param>
        /// <returns>回覆碼</returns>
        public IcrsAmount QueryIcrsAmount(string customerId, string serialNo)
        {
            IcrsAmount result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }



            string query = "SP_ICRS_CONSUME_QUERY";

            using (SqlConnection connection = DbConnection(Connection.ICRS))
            {
                SqlCommand command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@LS_CARD_ACCT_ID", SqlDbType.NVarChar, 10) { Value = customerId, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@LS_CARD_ACCT_ID_SEQ", SqlDbType.NVarChar, 1) { Value = serialNo??Convert.DBNull, Direction = ParameterDirection.Input });

                command.Parameters.Add(new SqlParameter("@LL_TOT_AMT_CONSUME", SqlDbType.Decimal, 4) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@LL_REMAIN", SqlDbType.Decimal, 4) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@LS_RISK_LEVEL", SqlDbType.NVarChar, 1) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@LS_SPEC_FLAG", SqlDbType.NVarChar, 1) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@LS_RESP_CODE", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output });

                connection.Open();
                command.ExecuteNonQuery();



                decimal? amount = command.Parameters["@LL_TOT_AMT_CONSUME"].Value as decimal?;
                decimal? availableCredit = command.Parameters["@LL_REMAIN"].Value as decimal?;
                string level = command.Parameters["@LS_RISK_LEVEL"].Value as string;
                string flag = command.Parameters["@LS_SPEC_FLAG"].Value as string;
                string responseCode = command.Parameters["@LS_RESP_CODE"].Value as string;

                if (String.IsNullOrEmpty(responseCode))
                {
                    throw new InvalidOperationException("responseCode not found");
                }

                result = new IcrsAmount()
                {
                    Amount = amount,
                    AvailableCredit = availableCredit,
                    Level = level,
                    Flag = flag,
                    ResponseCode = responseCode,
                };
            }

            return result;
        }
    }
}