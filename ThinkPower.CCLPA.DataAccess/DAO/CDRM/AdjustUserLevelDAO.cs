using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 臨調人員權限資料存取類別
    /// </summary>
    public class AdjustUserLevelDAO : BaseDAO
    {
        /// <summary>
        /// 取得臨調人員權限資料
        /// </summary>
        /// <param name="icrsId">ICRS帳號</param>
        /// <returns></returns>
        public AdjustUserLevelDO Get(string icrsId)
        {
            AdjustUserLevelDO result = null;

            if (String.IsNullOrEmpty(icrsId))
            {
                throw new ArgumentNullException("icrsId");
            }


            string query = @"
SELECT [USERID],[LEVELCODE] 
FROM [COD_ADJUSTUSER]
WHERE USERID = @UserId;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar)
                {
                    Value = icrsId
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertAdjustUserLevelDO(dt.Rows[0]);
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("AdjustUserLevel not the only");
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換臨調人員權限資料
        /// </summary>
        /// <param name="userLevelInfo">臨調人員權限資料</param>
        /// <returns></returns>
        private AdjustUserLevelDO ConvertAdjustUserLevelDO(DataRow userLevelInfo)
        {
            return new AdjustUserLevelDO()
            {
                UserID = userLevelInfo.Field<string>("USERID"),
                LevelCode = userLevelInfo.Field<string>("LEVELCODE"),
            };
        }
    }
}