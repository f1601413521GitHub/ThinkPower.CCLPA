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

                connection.Open();

                foreach (PreAdjustDO preAdjust in preAdjustList)
                {
                    SqlCommand command = new SqlCommand(query, connection);
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
            }
        }









        /// <summary>
        /// 取得所有等待區的預審資訊
        /// </summary>
        /// <returns>等待區預審處理資料</returns>
        public IEnumerable<PreAdjustDO> GetAllWaitData()
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
        public IEnumerable<PreAdjustDO> GetAllEffectData()
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
        /// <param name="preAdjustData">預審處理資料</param>
        /// <returns></returns>
        private PreAdjustDO ConvertPreAdjustDO(DataRow preAdjustData)
        {
            return new PreAdjustDO()
            {
                CampaignId = preAdjustData.Field<string>("CMPN_ID"),
                Id = preAdjustData.Field<string>("ID"),
                ProjectName = preAdjustData.Field<string>("PJNAME"),
                ProjectAmount = preAdjustData.Field<decimal?>("PRE_AMT"),
                CloseDate = preAdjustData.Field<string>("CLOSE_DT"),
                ImportDate = preAdjustData.Field<string>("IMPORT_DT"),
                ChineseName = preAdjustData.Field<string>("CHI_NAME"),
                Kind = preAdjustData.Field<string>("KIND"),
                SmsCheckResult = preAdjustData.Field<string>("SMS_CHECK"),
                Status = preAdjustData.Field<string>("STATUS"),
                ProcessingDateTime = preAdjustData.Field<string>("USER_PROC_DTTM"),
                ProcessingUserId = preAdjustData.Field<string>("USER_ID"),
                DeleteDateTime = preAdjustData.Field<string>("DEL_PROC_DTTM"),
                DeleteUserId = preAdjustData.Field<string>("DEL_ID"),
                Remark = preAdjustData.Field<string>("REMARK"),
                ClosingDay = preAdjustData.Field<string>("STMT_CYCLE_DESC"),
                PayDeadline = preAdjustData.Field<string>("PAY_DEADLINE"),
                AgreeUserId = preAdjustData.Field<string>("SAGREE_ID"),
                MobileTel = preAdjustData.Field<string>("MOBIL_TEL"),
                RejectReasonCode = preAdjustData.Field<string>("REJECTREASON"),
                CcasReplyCode = preAdjustData.Field<string>("CCAS_CODE"),
                CcasReplyStatus = preAdjustData.Field<string>("CCAS_STATUS"),
                CcasReplyDateTime = preAdjustData.Field<string>("CCAS_DT"),
            };
        }


        /// <summary>
        /// 取得等待區的預審資訊
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>等待區預審處理資料</returns>
        public IEnumerable<PreAdjustDO> GetWaitData(string id)
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
        public IEnumerable<PreAdjustDO> GetEffectData(string id)
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
