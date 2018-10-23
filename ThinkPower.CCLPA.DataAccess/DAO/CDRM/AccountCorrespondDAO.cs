using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 帳號對應資料存取類別
    /// </summary>
    public class AccountCorrespondDAO : BaseDAO
    {
        /// <summary>
        /// 取得帳號對應資訊
        /// </summary>
        /// <param name="id">使用者代號</param>
        /// <returns></returns>
        public AccountCorrespondDO Get(string id)
        {
            AccountCorrespondDO result = null;

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }


            string query = @"
SELECT [USER_ID], [ICRS_ID] 
FROM [PORTAL_ICRS_USER]
WHERE [USER_ID] = @UserId;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar)
                {
                    Value = id
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertAccountCorrespondDO(dt.Rows[0]);
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("AccountCorrespond not the only");
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換帳號對應資訊
        /// </summary>
        /// <param name="correspondInfo">帳號對應資訊</param>
        /// <returns></returns>
        private AccountCorrespondDO ConvertAccountCorrespondDO(DataRow correspondInfo)
        {
            return new AccountCorrespondDO()
            {
                UserId = correspondInfo.Field<string>("USER_ID"),
                IcrsId = correspondInfo.Field<string>("ICRS_ID"),
            };
        }
    }
}