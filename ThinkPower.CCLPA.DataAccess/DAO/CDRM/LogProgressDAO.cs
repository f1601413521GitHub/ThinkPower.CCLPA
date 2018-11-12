using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 臨調紀錄檔資料存取類別
    /// </summary>
    public class LogProgressDAO : BaseDAO
    {
        /// <summary>
        /// 新增臨調紀錄資訊
        /// </summary>
        /// <param name="adjustLog">臨調紀錄資訊</param>
        /// <returns></returns>
        public void Insert(LogProgressDO adjustLog)
        {
            if (adjustLog == null)
            {
                throw new ArgumentNullException(nameof(adjustLog));
            }

            string query = @"
INSERT INTO [LOG_PROGRESS]
    ([APPNO],[APPOPTION],[PROGRESS],[PROCID],[PROCNAME],[PROCDATE],
    [PROCTIME],[SERIALNO],[MEMO])
VALUES
    (@ApplicationNo,@ApplicationKind,@ProgressCode,@ProgressId,@ProgressName,@ProgressDate,
    @ProgressTime,@SerialNo,@Memo);";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@ApplicationNo", SqlDbType.NVarChar) { Value = adjustLog.ApplicationNo });
                command.Parameters.Add(new SqlParameter("@ApplicationKind", SqlDbType.NVarChar) { Value = adjustLog.ApplicationKind ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProgressCode", SqlDbType.NVarChar) { Value = adjustLog.ProgressCode ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProgressId", SqlDbType.NVarChar) { Value = adjustLog.ProgressId ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProgressName", SqlDbType.NVarChar) { Value = adjustLog.ProgressName ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProgressDate", SqlDbType.NVarChar) { Value = adjustLog.ProgressDate });
                command.Parameters.Add(new SqlParameter("@ProgressTime", SqlDbType.NVarChar) { Value = adjustLog.ProgressTime });
                command.Parameters.Add(new SqlParameter("@SerialNo", SqlDbType.Decimal) { Value = adjustLog.SerialNo ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Memo", SqlDbType.NVarChar) { Value = adjustLog.Memo ?? Convert.DBNull });

                connection.Open();
                command.ExecuteNonQuery();

                command = null;
            }
        }
    }
}
