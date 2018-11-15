using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.VO;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// CDRM專案臨調系統資料存取類別
    /// </summary>
    public class AdjustSystemDAO : BaseDAO
    {
        /// <summary>
        /// 預審生效條件檢核
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>預審生效條件檢核結果</returns>
        public PreAdjustEffectResultDO ValidatePreAdjustEffectConfition(string id)
        {
            PreAdjustEffectResultDO result = null;

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
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

                string rejectReason = command.Parameters["@REJECTREASON"].Value as string;
                string responseCode = command.Parameters["@Resp_code"].Value as string;

                if (String.IsNullOrEmpty(responseCode))
                {
                    throw new InvalidOperationException("responseCode not found");
                }

                result = new PreAdjustEffectResultDO()
                {
                    RejectReason = rejectReason,
                    ResponseCode = responseCode,
                };
            }

            return result;
        }

        /// <summary>
        /// 專案臨調條件檢核
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="jcicQueryDate">JCIC送查日期</param>
        /// <param name="adjustReasonCode">臨調原因代碼</param>
        /// <returns></returns>
        public AdjustValidateResult ValidateAdjustCondition(string customerId, string jcicQueryDate, string adjustReasonCode)
        {
            AdjustValidateResult result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (String.IsNullOrEmpty(jcicQueryDate))
            {
                throw new ArgumentNullException(nameof(jcicQueryDate));
            }
            else if (String.IsNullOrEmpty(adjustReasonCode))
            {
                throw new ArgumentNullException(nameof(adjustReasonCode));
            }

            string query = "SP_ELGB_PAD02";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@vID",                 SqlDbType.NVarChar, 11  ) { Value = customerId,         Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@JCIC_DATE",           SqlDbType.NVarChar, 10  ) { Value = jcicQueryDate,      Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@REASON",              SqlDbType.NVarChar, 2   ) { Value = adjustReasonCode,   Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@ESTIMATE_RESULT",     SqlDbType.NVarChar, 20  ) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@REJECTREASON",        SqlDbType.NVarChar, 120 ) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@PRJ_RESULT",          SqlDbType.NVarChar, 20  ) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@PRJ_REJECTREASON",    SqlDbType.NVarChar, 120 ) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@Resp_code",           SqlDbType.NVarChar, 2   ) { Direction = ParameterDirection.Output });

                connection.Open();
                command.ExecuteNonQuery();

                string responseCode = command.Parameters["@Resp_code"].Value as string;

                if (String.IsNullOrEmpty(responseCode))
                {
                    throw new InvalidOperationException($"{nameof(responseCode)} not found");
                }

                result = new AdjustValidateResult()
                {
                    EstimateResult      = command.Parameters["@ESTIMATE_RESULT"].Value as string,
                    RejectReason        = command.Parameters["@REJECTREASON"].Value as string,
                    ProjectResult       = command.Parameters["@PRJ_RESULT"].Value as string,
                    ProjectRejectReason = command.Parameters["@PRJ_REJECTREASON"].Value as string,
                    ResponseCode        = responseCode,
                };
            }

            return result;
        }


        /// <summary>
        /// 查詢JCIC送查日期
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="loginAccount">登入帳號</param>
        /// <param name="loginName">登入姓名</param>
        /// <returns></returns>
        public JcicQueryResult JcicSendQuery(string customerId, string loginAccount, string loginName)
        {
            JcicQueryResult result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (String.IsNullOrEmpty(loginAccount))
            {
                throw new ArgumentNullException(nameof(loginAccount));
            }
            else if (String.IsNullOrEmpty(loginName))
            {
                throw new ArgumentNullException(nameof(loginName));
            }



            string query = "SP_ELGB_PAD01";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@vID", SqlDbType.NVarChar, 11) { Value = customerId, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar, 7) { Value = loginAccount, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 30) { Value = loginName, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@JCIC_DATE", SqlDbType.NVarChar, 10) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@Resp_code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output });

                connection.Open();
                command.ExecuteNonQuery();

                string jcicDate = command.Parameters["@JCIC_DATE"].Value as string;
                string responseCode = command.Parameters["@Resp_code"].Value as string;

                if (String.IsNullOrEmpty(responseCode))
                {
                    throw new InvalidOperationException("responseCode not found");
                }

                result = new JcicQueryResult()
                {
                    JcicQueryDate = jcicDate,
                    ResponseCode = responseCode,
                };
            }

            return result;
        }
    }
}