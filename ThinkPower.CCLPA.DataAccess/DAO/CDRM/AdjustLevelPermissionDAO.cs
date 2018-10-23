using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 臨調權限設定檔資料存取類別
    /// </summary>
    public class AdjustLevelPermissionDAO : BaseDAO
    {
        /// <summary>
        /// 取得臨調權限設定檔資料
        /// </summary>
        /// <param name="levelCode">權限等級</param>
        /// <returns></returns>
        public AdjustLevelPermissionDO Get(string levelCode)
        {
            AdjustLevelPermissionDO result = null;

            if (String.IsNullOrEmpty(levelCode))
            {
                throw new ArgumentNullException("levelCode");
            }


            string query = @"
SELECT [LEVELCODE],[CL],[ADJUST_QUERY],[ADJUST_EXEC],[VERIFY_NORMAL],[VERIFY_ADV],[SEQNO]
FROM [COD_ADJUSTLEVEL]
WHERE LEVELCODE = @LevelCode;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@LevelCode", SqlDbType.NVarChar)
                {
                    Value = levelCode
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertAdjustLevelPermissionDO(dt.Rows[0]);
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("AdjustLevelPermission not the only");
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換權限設定檔資料
        /// </summary>
        /// <param name="permissionInfo">權限設定檔資料</param>
        /// <returns></returns>
        private AdjustLevelPermissionDO ConvertAdjustLevelPermissionDO(DataRow permissionInfo)
        {
            return new AdjustLevelPermissionDO()
            {
                LevelCode = permissionInfo.Field<string>("LEVELCODE"),
                Amount = permissionInfo.Field<string>("CL"),
                AdjustQuery = permissionInfo.Field<string>("ADJUST_QUERY"),
                AdjustExecute = permissionInfo.Field<string>("ADJUST_EXEC"),
                VerifyNormal = permissionInfo.Field<string>("VERIFY_NORMAL"),
                VerifySupervisor = permissionInfo.Field<string>("VERIFY_ADV"),
                SequenceNo = permissionInfo.Field<decimal?>("SEQNO"),
            };
        }
    }
}