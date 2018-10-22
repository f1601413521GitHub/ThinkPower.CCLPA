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
                    command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar) { Value = preAdjust.CampaignId ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar) { Value = preAdjust.Id ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ProjectName", SqlDbType.NVarChar) { Value = preAdjust.ProjectName ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ProjectAmount", SqlDbType.Decimal) { Value = preAdjust.ProjectAmount ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar) { Value = preAdjust.CloseDate ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ImportDate", SqlDbType.NVarChar) { Value = preAdjust.ImportDate ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ChineseName", SqlDbType.NVarChar) { Value = preAdjust.ChineseName ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = preAdjust.Kind ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar) { Value = preAdjust.Status ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@ClosingDay", SqlDbType.NVarChar) { Value = preAdjust.ClosingDay ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@PayDeadline", SqlDbType.NVarChar) { Value = preAdjust.PayDeadline ?? Convert.DBNull });
                    command.Parameters.Add(new SqlParameter("@MobileTel", SqlDbType.NVarChar) { Value = preAdjust.MobileTel ?? Convert.DBNull });

                    command.ExecuteNonQuery();
                }

                command = null;
            }
        }

        /// <summary>
        /// 取得所有等待區的預審資訊
        /// </summary>
        /// <returns>等待區預審處理資料</returns>
        public IEnumerable<PreAdjustDO> GetAllWaitZoneData()
        {
            List<PreAdjustDO> result = null;

            string query = @"
SELECT 
    [CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],[STATUS],
    [USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],[PAY_DEADLINE],
    [SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT]
FROM [RG_PADJUST]
WHERE CLOSE_DT >= @CloseDate
    AND (CCAS_CODE != '00' OR CCAS_CODE IS NULL);";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar)
                {
                    Value = DateTime.Now.ToString("yyyyMMdd"),
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<PreAdjustDO>();
                    PreAdjustDO tempPreAdjustDO = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        tempPreAdjustDO = null;
                        tempPreAdjustDO = ConvertPreAdjustDO(dr);

                        result.Add(tempPreAdjustDO);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 取得所有生效區的預審資訊
        /// </summary>
        /// <returns>生效區預審處理資料</returns>
        public IEnumerable<PreAdjustDO> GetAllEffectZoneData()
        {
            List<PreAdjustDO> result = null;

            string query = @"
SELECT 
    [CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],[STATUS],
    [USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],[PAY_DEADLINE],
    [SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT]
FROM [RG_PADJUST]
WHERE CLOSE_DT >= @CloseDate
    AND (CCAS_CODE = '00');";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar)
                {
                    Value = DateTime.Now.ToString("yyyyMMdd"),
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<PreAdjustDO>();
                    PreAdjustDO tempPreAdjustDO = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        tempPreAdjustDO = null;
                        tempPreAdjustDO = ConvertPreAdjustDO(dr);

                        result.Add(tempPreAdjustDO);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換預審處理資料
        /// </summary>
        /// <param name="preAdjustInfo">預審處理資料</param>
        /// <returns></returns>
        private PreAdjustDO ConvertPreAdjustDO(DataRow preAdjustInfo)
        {
           return new PreAdjustDO(){
               CampaignId = preAdjustInfo.Field<string>("CMPN_ID"),
               Id = preAdjustInfo.Field<string>("ID"),
               ProjectName = preAdjustInfo.Field<string>("PJNAME"),
               ProjectAmount = preAdjustInfo.Field<decimal?>("PRE_AMT"),
               CloseDate = preAdjustInfo.Field<string>("CLOSE_DT"),
               ImportDate = preAdjustInfo.Field<string>("IMPORT_DT"),
               ChineseName = preAdjustInfo.Field<string>("CHI_NAME"),
               Kind = preAdjustInfo.Field<string>("KIND"),
               SmsCheckResult = preAdjustInfo.Field<string>("SMS_CHECK"),
               Status = preAdjustInfo.Field<string>("STATUS"),
               ProcessingDateTime = preAdjustInfo.Field<string>("USER_PROC_DTTM"),
               ProcessingUserId = preAdjustInfo.Field<string>("USER_ID"),
               DeleteDateTime = preAdjustInfo.Field<string>("DEL_PROC_DTTM"),
               DeleteUserId = preAdjustInfo.Field<string>("DEL_ID"),
               Remark = preAdjustInfo.Field<string>("REMARK"),
               ClosingDay = preAdjustInfo.Field<string>("STMT_CYCLE_DESC"),
               PayDeadline = preAdjustInfo.Field<string>("PAY_DEADLINE"),
               AgreeUserId = preAdjustInfo.Field<string>("SAGREE_ID"),
               MobileTel = preAdjustInfo.Field<string>("MOBIL_TEL"),
               RejectReasonCode = preAdjustInfo.Field<string>("REJECTREASON"),
               CcasReplyCode = preAdjustInfo.Field<string>("CCAS_CODE"),
               CcasReplyStatus = preAdjustInfo.Field<string>("CCAS_STATUS"),
               CcasReplyDateTime = preAdjustInfo.Field<string>("CCAS_DT"),
           };
        }


        /// <summary>
        /// 取得等待區的預審資訊
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>等待區預審處理資料</returns>
        public IEnumerable<PreAdjustDO> GetWaitZoneData(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }


            List<PreAdjustDO> result = null;

            string query = @"
SELECT 
    [CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],[STATUS],
    [USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],[PAY_DEADLINE],
    [SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT]
FROM [RG_PADJUST]
WHERE CLOSE_DT >= @CloseDate
    AND (CCAS_CODE != '00' OR CCAS_CODE IS NULL)
    AND ID = @Id;";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar)
                {
                    Value = DateTime.Now.ToString("yyyyMMdd"),
                });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar)
                {
                    Value = id,
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<PreAdjustDO>();
                    PreAdjustDO tempPreAdjustDO = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        tempPreAdjustDO = null;
                        tempPreAdjustDO = ConvertPreAdjustDO(dr);

                        result.Add(tempPreAdjustDO);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 取得生效區的預審資訊
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>生效區預審處理資料</returns>
        public IEnumerable<PreAdjustDO> GetEffectZoneData(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            List<PreAdjustDO> result = null;

            string query = @"
SELECT 
    [CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],[STATUS],
    [USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],[PAY_DEADLINE],
    [SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT]
FROM [RG_PADJUST]
WHERE CLOSE_DT >= @CloseDate
    AND (CCAS_CODE = '00')
    AND ID = @Id;";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar)
                {
                    Value = DateTime.Now.ToString("yyyyMMdd"),
                });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar)
                {
                    Value = id,
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<PreAdjustDO>();
                    PreAdjustDO tempPreAdjustDO = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        tempPreAdjustDO = null;
                        tempPreAdjustDO = ConvertPreAdjustDO(dr);

                        result.Add(tempPreAdjustDO);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }
    }
}
