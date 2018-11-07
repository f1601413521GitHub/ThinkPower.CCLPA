using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.Condition;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 臨調預審處理檔資料存取類別
    /// </summary>
    public class PreAdjustDAO : BaseDAO
    {
        /// <summary>
        /// 新增預審名單
        /// </summary>
        /// <param name="preAdjust">預審名單清單</param>
        public void Insert(PreAdjustDO preAdjust)
        {
            if (preAdjust == null)
            {
                throw new ArgumentNullException("preAdjust");
            }


            string query = @"
INSERT INTO [RG_PADJUST]
    ([CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],
    [STATUS],[USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],
    [PAY_DEADLINE],[SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT])
VALUES
    (@CampaignId,@Id,@ProjectName,@ProjectAmount,@CloseDate,@ImportDate,@ChineseName,@Kind,@SmsCheckResult,
    @Status,@ProcessingDateTime,@ProcessingUserId,@DeleteDateTime,@DeleteUserId,@Remark,@ClosingDay,
    @PayDeadline,@AgreeUserId,@MobileTel,@RejectReasonCode,@CcasReplyCode,@CcasReplyStatus,@CcasReplyDateTime);";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar) { Value = preAdjust.CampaignId, });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar) { Value = preAdjust.Id, });
                command.Parameters.Add(new SqlParameter("@ProjectName", SqlDbType.NVarChar) { Value = preAdjust.ProjectName ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ProjectAmount", SqlDbType.Decimal) { Value = preAdjust.ProjectAmount ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar) { Value = preAdjust.CloseDate ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ImportDate", SqlDbType.NVarChar) { Value = preAdjust.ImportDate ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ChineseName", SqlDbType.NVarChar) { Value = preAdjust.ChineseName ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = preAdjust.Kind ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@SmsCheckResult", SqlDbType.NVarChar) { Value = preAdjust.SmsCheckResult ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar) { Value = preAdjust.Status ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ProcessingDateTime", SqlDbType.NVarChar) { Value = preAdjust.ProcessingDateTime ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ProcessingUserId", SqlDbType.NVarChar) { Value = preAdjust.ProcessingUserId ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@DeleteDateTime", SqlDbType.NVarChar) { Value = preAdjust.DeleteDateTime ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@DeleteUserId", SqlDbType.NVarChar) { Value = preAdjust.DeleteUserId ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@Remark", SqlDbType.NVarChar) { Value = preAdjust.Remark ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ClosingDay", SqlDbType.NVarChar) { Value = preAdjust.ClosingDay ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@PayDeadline", SqlDbType.NVarChar) { Value = preAdjust.PayDeadline ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@AgreeUserId", SqlDbType.NVarChar) { Value = preAdjust.ForceAgreeUserId ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@MobileTel", SqlDbType.NVarChar) { Value = preAdjust.MobileTel ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@RejectReasonCode", SqlDbType.NVarChar) { Value = preAdjust.RejectReasonCode ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CcasReplyCode", SqlDbType.NVarChar) { Value = preAdjust.CcasReplyCode ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CcasReplyStatus", SqlDbType.NVarChar) { Value = preAdjust.CcasReplyStatus ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CcasReplyDateTime", SqlDbType.NVarChar) { Value = preAdjust.CcasReplyDateTime ?? Convert.DBNull, });

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 更新預審名單
        /// </summary>
        /// <param name="preAdjust">預審名單資料</param>
        public void Update(PreAdjustDO preAdjust)
        {
            if (preAdjust == null)
            {
                throw new ArgumentNullException("preAdjust");
            }

            string query = @"
UPDATE  [RG_PADJUST]
   SET  [CMPN_ID]         = @CampaignId,
        [ID]              = @Id,
        [PJNAME]          = @ProjectName,
        [PRE_AMT]         = @ProjectAmount,
        [CLOSE_DT]        = @CloseDate,
        [IMPORT_DT]       = @ImportDate,
        [CHI_NAME]        = @ChineseName,
        [KIND]            = @Kind,
        [SMS_CHECK]       = @SmsCheckResult,
        [STATUS]          = @Status,
        [USER_PROC_DTTM]  = @ProcessingDateTime,
        [USER_ID]         = @ProcessingUserId,
        [DEL_PROC_DTTM]   = @DeleteDateTime,
        [DEL_ID]          = @DeleteUserId,
        [REMARK]          = @Remark,
        [STMT_CYCLE_DESC] = @ClosingDay,
        [PAY_DEADLINE]    = @PayDeadline,
        [SAGREE_ID]       = @AgreeUserId,
        [MOBIL_TEL]       = @MobileTel,
        [REJECTREASON]    = @RejectReasonCode,
        [CCAS_CODE]       = @CcasReplyCode,
        [CCAS_STATUS]     = @CcasReplyStatus,
        [CCAS_DT]         = @CcasReplyDateTime
 WHERE CMPN_ID = @CampaignId 
    AND ID = @Id;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar) { Value = preAdjust.CampaignId, });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar) { Value = preAdjust.Id, });
                command.Parameters.Add(new SqlParameter("@ProjectName", SqlDbType.NVarChar) { Value = preAdjust.ProjectName ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ProjectAmount", SqlDbType.Decimal) { Value = preAdjust.ProjectAmount ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar) { Value = preAdjust.CloseDate ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ImportDate", SqlDbType.NVarChar) { Value = preAdjust.ImportDate ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ChineseName", SqlDbType.NVarChar) { Value = preAdjust.ChineseName ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = preAdjust.Kind ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@SmsCheckResult", SqlDbType.NVarChar) { Value = preAdjust.SmsCheckResult ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar) { Value = preAdjust.Status ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ProcessingDateTime", SqlDbType.NVarChar) { Value = preAdjust.ProcessingDateTime ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ProcessingUserId", SqlDbType.NVarChar) { Value = preAdjust.ProcessingUserId ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@DeleteDateTime", SqlDbType.NVarChar) { Value = preAdjust.DeleteDateTime ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@DeleteUserId", SqlDbType.NVarChar) { Value = preAdjust.DeleteUserId ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@Remark", SqlDbType.NVarChar) { Value = preAdjust.Remark ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@ClosingDay", SqlDbType.NVarChar) { Value = preAdjust.ClosingDay ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@PayDeadline", SqlDbType.NVarChar) { Value = preAdjust.PayDeadline ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@AgreeUserId", SqlDbType.NVarChar) { Value = preAdjust.ForceAgreeUserId ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@MobileTel", SqlDbType.NVarChar) { Value = preAdjust.MobileTel ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@RejectReasonCode", SqlDbType.NVarChar) { Value = preAdjust.RejectReasonCode ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CcasReplyCode", SqlDbType.NVarChar) { Value = preAdjust.CcasReplyCode ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CcasReplyStatus", SqlDbType.NVarChar) { Value = preAdjust.CcasReplyStatus ?? Convert.DBNull, });
                command.Parameters.Add(new SqlParameter("@CcasReplyDateTime", SqlDbType.NVarChar) { Value = preAdjust.CcasReplyDateTime ?? Convert.DBNull, });

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 查詢臨調預審名單
        /// </summary>
        /// <param name="condition">預審名單資料查詢條件</param>
        /// <returns></returns>
        public IEnumerable<PreAdjustDO> Query(PreAdjustCondition condition)
        {
            List<PreAdjustDO> result = null;

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }



            StringBuilder querySB = new StringBuilder(@"
SELECT 
    [CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],[STATUS],
    [USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],[PAY_DEADLINE],
    [SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT]
FROM [RG_PADJUST]");



            List<string> queryCommand = new List<string>();
            List<string> pagingCommand = new List<string>();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();



            if (condition.CloseDate != null)
            {
                queryCommand.Add("CLOSE_DT >= @CloseDate");
                sqlParameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar)
                {
                    Value = condition.CloseDate.Value.ToString("yyyy/MM/dd")
                });
            }

            if (String.IsNullOrEmpty(condition.CcasReplyCode) || (condition.CcasReplyCode != "00"))
            {
                queryCommand.Add("(CCAS_CODE != '00' OR CCAS_CODE IS NULL)");
            }
            else
            {
                queryCommand.Add("(CCAS_CODE = '00')");
            }

            if (!String.IsNullOrEmpty(condition.CustomerId))
            {
                queryCommand.Add("ID = @CustomerId");
                sqlParameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)
                {
                    Value = condition.CustomerId
                });
            }

            if (!String.IsNullOrEmpty(condition.CampaignId))
            {
                queryCommand.Add("CMPN_ID = @CampaignId");
                sqlParameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar)
                {
                    Value = condition.CampaignId
                });
            }

            if ((condition.PageIndex != null && condition.PageIndex >= 1) &&
                (condition.PagingSize != null && condition.PagingSize >= 1))
            {
                switch (condition.OrderBy)
                {
                    case PreAdjustCondition.OrderByKind.None:
                        pagingCommand.Add("ORDER BY [CMPN_ID]");
                        break;
                    case PreAdjustCondition.OrderByKind.CustomerId:
                        pagingCommand.Add("ORDER BY [ID]");
                        break;
                    case PreAdjustCondition.OrderByKind.ImportDate:
                        pagingCommand.Add("ORDER BY [IMPORT_DT]");
                        break;
                    case PreAdjustCondition.OrderByKind.ImportDateByDescending:
                        pagingCommand.Add("ORDER BY [IMPORT_DT] DESC");
                        break;
                    case PreAdjustCondition.OrderByKind.CustomerIdAndImportDate:
                        pagingCommand.Add("ORDER BY [ID], [IMPORT_DT]");
                        break;
                }

                pagingCommand.Add("OFFSET     @Skip ROWS");
                sqlParameters.Add(new SqlParameter("@Skip", SqlDbType.Int)
                {
                    Value = ((condition.PageIndex - 1) * condition.PagingSize)
                });

                pagingCommand.Add("FETCH NEXT @Take ROWS ONLY");
                sqlParameters.Add(new SqlParameter("@Take", SqlDbType.Int)
                {
                    Value = condition.PagingSize
                });
            }

            if (queryCommand.Count > 0)
            {
                querySB.Append(" WHERE ");
                querySB.Append(String.Join(" AND ", queryCommand));
            }

            if (pagingCommand.Count > 0)
            {
                querySB.Append(" ");
                querySB.Append(String.Join(" ", pagingCommand));
            }

            querySB.Append(";");



            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(querySB.ToString(), connection);
                command.Parameters.AddRange(sqlParameters.ToArray());

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<PreAdjustDO>();
                    PreAdjustDO preAdjustDO = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        preAdjustDO = ConvertPreAdjustDO(dr);

                        result.Add(preAdjustDO);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result ?? new List<PreAdjustDO>();
        }


        /// <summary>
        /// 取得臨調預審名單
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="campaignId">行銷活動代碼</param>
        /// <returns></returns>
        public PreAdjustDO Get(string customerId, string campaignId)
        {
            PreAdjustDO result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }


            string query = @"
SELECT 
    [CMPN_ID],[ID],[PJNAME],[PRE_AMT],[CLOSE_DT],[IMPORT_DT],[CHI_NAME],[KIND],[SMS_CHECK],[STATUS],
    [USER_PROC_DTTM],[USER_ID],[DEL_PROC_DTTM],[DEL_ID],[REMARK],[STMT_CYCLE_DESC],[PAY_DEADLINE],
    [SAGREE_ID],[MOBIL_TEL],[REJECTREASON],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT]
FROM [RG_PADJUST]
WHERE ID = @CustomerId
    AND CMPN_ID = @CampaignId;";


            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)
                {
                    Value = customerId
                });
                command.Parameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar)
                {
                    Value = campaignId
                });


                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertPreAdjustDO(dt.Rows[0]);
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("PreAdjust not the only");
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }



        /// <summary>
        /// 查詢臨調預審名單總筆數
        /// </summary>
        /// <param name="condition">預審名單資料查詢條件</param>
        /// <returns></returns>
        public int Count(PreAdjustCondition condition)
        {
            int result = 0;

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }



            StringBuilder querySB = new StringBuilder(@"SELECT COUNT(1) FROM [RG_PADJUST]");


            List<string> queryCommand = new List<string>();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();



            if (condition.CloseDate != null)
            {
                queryCommand.Add("CLOSE_DT >= @CloseDate");
                sqlParameters.Add(new SqlParameter("@CloseDate", SqlDbType.NVarChar)
                {
                    Value = condition.CloseDate.Value.ToString("yyyy/MM/dd")
                });
            }

            if (String.IsNullOrEmpty(condition.CcasReplyCode) || (condition.CcasReplyCode != "00"))
            {
                queryCommand.Add("(CCAS_CODE != '00' OR CCAS_CODE IS NULL)");
            }
            else
            {
                queryCommand.Add("(CCAS_CODE = '00')");
            }

            if (!String.IsNullOrEmpty(condition.CustomerId))
            {
                queryCommand.Add("ID = @CustomerId");
                sqlParameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)
                {
                    Value = condition.CustomerId
                });
            }

            if (!String.IsNullOrEmpty(condition.CampaignId))
            {
                queryCommand.Add("CMPN_ID = @CampaignId");
                sqlParameters.Add(new SqlParameter("@CampaignId", SqlDbType.NVarChar)
                {
                    Value = condition.CampaignId
                });
            }

            if (queryCommand.Count > 0)
            {
                querySB.Append(" WHERE ");
                querySB.Append(String.Join(" AND ", queryCommand));
            }

            querySB.Append(";");

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {

                SqlCommand command = new SqlCommand(querySB.ToString(), connection);
                command.Parameters.AddRange(sqlParameters.ToArray());

                connection.Open();

                result = Convert.ToInt32(command.ExecuteScalar());
            }

            return result;
        }



        #region Private Method

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
                ForceAgreeUserId = preAdjustData.Field<string>("SAGREE_ID"),
                MobileTel = preAdjustData.Field<string>("MOBIL_TEL"),
                RejectReasonCode = preAdjustData.Field<string>("REJECTREASON"),
                CcasReplyCode = preAdjustData.Field<string>("CCAS_CODE"),
                CcasReplyStatus = preAdjustData.Field<string>("CCAS_STATUS"),
                CcasReplyDateTime = preAdjustData.Field<string>("CCAS_DT"),
            };
        }


        #endregion
    }
}
