﻿using System;
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
    /// 臨調預審處理檔資料存取類別
    /// </summary>
    public class PreAdjustDAO : BaseDAO
    {
        /// <summary>
        /// 新增臨調預審處理資料
        /// </summary>
        /// <param name="preAdjustList">臨調預審處理資料清單</param>
        public void Insert(List<PreAdjustDO> preAdjustList)
        {
            if ((preAdjustList == null) || (preAdjustList.Count == 0))
            {
                throw new ArgumentNullException("preAdjustList");
            }


            string query = @"
INSERT INTO [RG_PADJUST]
    ([CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],
    [KIND],[STATUS],[STMT_CYCLE_DESC],[PAY_DEADLINE],[MOBIL_TEL])
VALUES
    (@CampaignId,@Id,@ProjectName,@ProjectAmount,@CloseDate,@ImportDate,@ChineseName,
    @Kind,@Status,@ClosingDay,@PayDeadline,@MobileTel);";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                foreach (PreAdjustDO preAdjust in preAdjustList)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.VarChar) { Value = preAdjust.CampaignId ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = preAdjust.Id ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ProjectName", SqlDbType.VarChar) { Value = preAdjust.ProjectName ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ProjectAmount", SqlDbType.Decimal) { Value = preAdjust.ProjectAmount ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.VarChar) { Value = preAdjust.CloseDate ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ImportDate", SqlDbType.VarChar) { Value = preAdjust.ImportDate ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ChineseName", SqlDbType.VarChar) { Value = preAdjust.ChineseName ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.VarChar) { Value = preAdjust.Kind ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = preAdjust.Status ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ClosingDay", SqlDbType.VarChar) { Value = preAdjust.ClosingDay ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@PayDeadline", SqlDbType.VarChar) { Value = preAdjust.PayDeadline ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@MobileTel", SqlDbType.VarChar) { Value = preAdjust.MobileTel ?? Convert.DBNull });

                    command.ExecuteNonQuery();
                }

                command = null;
            }
        }
    }
}
