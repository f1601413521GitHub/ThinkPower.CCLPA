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
    /// 臨調處理檔資料存取類別
    /// </summary>
    public class AdjustDAO : BaseDAO
    {
        /// <summary>
        /// 新增臨調處理資訊
        /// </summary>
        /// <param name="adjustInfo">臨調處理資訊</param>
        public void Insert(AdjustDO adjustInfo)
        {
            if (adjustInfo == null)
            {
                throw new ArgumentNullException("adjustInfo");
            }

            string query = @"
INSERT INTO [RG_ADJUST]
    ([ID],[APPLY_DATE],[APPLY_TIME],[ACCT_ID],[NAME],[TOT_LIMIT],[APPLY_AMT],[USESITE],[PLACE],
    [ADJUST_DATE_S],[ADJUST_DATE_E],[REASON1],[REASON2],[REASON3],[REASON],[REMARK],[FAC_AUTH],
    [APPROVE_AMT_MAX],[USABILITY_AMT],[OVERPAY_AMT_PRO],[APPROVE_AMT],[OVERPAY_AMT],[ESTIMATE_RESULT],
    [REJECTREASON],[APPROVE_RESULT],[CHIEF_FLAG],[CHIEF_REMARK],[PENDING_FLAG],[USER_ID],[USER_NAME],
    [CHIEF_ID],[CHIEF_NAME],[JCIC_DATE],[TYPE],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT],[PROC_DATE],
    [PROC_TIME],[ICARE_YN],[PRJ_YN],[PRJ_RESULT],[PRJ_REJECTREASON],
    [CREDIT_AMT])
VALUES
    (@Id,@ApplyDate,@ApplyTime,@CustomerId,@CustomerName,@CreditLimit,@ApplyAmount,@UseSite,@Place,
    @AdjustDateStart,@AdjustDateEnd,@Reason1,@Reason2,@Reason3,@Reason,@Remark,@ForceAuthenticate,
    @ApproveAmountMax,@UsabilityAmount,@OverpayAmountPro,@ApproveAmount,@OverpayAmount,@EstimateResult,
    @RejectReason,@ApproveResult,@ChiefFlag,@ChiefRemark,@PendingFlag,@UserId,@UserName,
    @ChiefId,@ChiefName,@JcicDate,@Type,@CcasCode,@CcasStatus,@CcasDateTime,@ProcessDate,
    @ProcessTime,@IcareStatus,@ProjectStatus,@ProjectAdjustResult,@ProjectAdjustRejectReason,
    @CreditAmount);";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar) { Value = adjustInfo.Id });
                command.Parameters.Add(new SqlParameter("@ApplyDate", SqlDbType.NVarChar) { Value = adjustInfo.ApplyDate });
                command.Parameters.Add(new SqlParameter("@ApplyTime", SqlDbType.NVarChar) { Value = adjustInfo.ApplyTime });
                command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar) { Value = adjustInfo.CustomerId ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@CustomerName", SqlDbType.NVarChar) { Value = adjustInfo.CustomerName ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@CreditLimit", SqlDbType.Decimal) { Value = adjustInfo.CreditLimit ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ApplyAmount", SqlDbType.Decimal) { Value = adjustInfo.ApplyAmount ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@UseSite", SqlDbType.NVarChar) { Value = adjustInfo.UseSite ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Place", SqlDbType.NVarChar) { Value = adjustInfo.Place ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@AdjustDateStart", SqlDbType.NVarChar) { Value = adjustInfo.AdjustDateStart ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@AdjustDateEnd", SqlDbType.NVarChar) { Value = adjustInfo.AdjustDateEnd ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Reason1", SqlDbType.NVarChar) { Value = adjustInfo.Reason1 ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Reason2", SqlDbType.NVarChar) { Value = adjustInfo.Reason2 ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Reason3", SqlDbType.NVarChar) { Value = adjustInfo.Reason3 ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar) { Value = adjustInfo.Reason ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Remark", SqlDbType.NVarChar) { Value = adjustInfo.Remark ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ForceAuthenticate", SqlDbType.NVarChar) { Value = adjustInfo.ForceAuthenticate ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ApproveAmountMax", SqlDbType.Decimal) { Value = adjustInfo.ApproveAmountMax ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@UsabilityAmount", SqlDbType.Decimal) { Value = adjustInfo.UsabilityAmount ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@OverpayAmountPro", SqlDbType.Decimal) { Value = adjustInfo.OverpayAmountPro ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ApproveAmount", SqlDbType.Decimal) { Value = adjustInfo.ApproveAmount ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@OverpayAmount", SqlDbType.Decimal) { Value = adjustInfo.OverpayAmount ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@EstimateResult", SqlDbType.NVarChar) { Value = adjustInfo.EstimateResult ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@RejectReason", SqlDbType.NVarChar) { Value = adjustInfo.RejectReason ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ApproveResult", SqlDbType.NVarChar) { Value = adjustInfo.ApproveResult ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ChiefFlag", SqlDbType.NVarChar) { Value = adjustInfo.ChiefFlag ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ChiefRemark", SqlDbType.NVarChar) { Value = adjustInfo.ChiefRemark ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@PendingFlag", SqlDbType.NVarChar) { Value = adjustInfo.PendingFlag ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = adjustInfo.UserId ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = adjustInfo.UserName ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ChiefId", SqlDbType.NVarChar) { Value = adjustInfo.ChiefId ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ChiefName", SqlDbType.NVarChar) { Value = adjustInfo.ChiefName ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@JcicDate", SqlDbType.NVarChar) { Value = adjustInfo.JcicDate ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar) { Value = adjustInfo.Type ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@CcasCode", SqlDbType.NVarChar) { Value = adjustInfo.CcasCode ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@CcasStatus", SqlDbType.NVarChar) { Value = adjustInfo.CcasStatus ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@CcasDateTime", SqlDbType.NVarChar) { Value = adjustInfo.CcasDateTime ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProcessDate", SqlDbType.NVarChar) { Value = adjustInfo.ProcessDate ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProcessTime", SqlDbType.NVarChar) { Value = adjustInfo.ProcessTime ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@IcareStatus", SqlDbType.NVarChar) { Value = adjustInfo.IcareStatus ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProjectStatus", SqlDbType.NVarChar) { Value = adjustInfo.ProjectStatus ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProjectAdjustResult", SqlDbType.NVarChar) { Value = adjustInfo.ProjectAdjustResult ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@ProjectAdjustRejectReason", SqlDbType.NVarChar) { Value = adjustInfo.ProjectAdjustRejectReason ?? Convert.DBNull });
                command.Parameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Decimal) { Value = adjustInfo.CreditAmount ?? Convert.DBNull });

                connection.Open();
                command.ExecuteNonQuery();

                command = null;
            }
        }

        /// <summary>
        /// 取得臨調處理檔資訊
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public IEnumerable<AdjustDO> Get(string customerId)
        {
            List<AdjustDO> result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            string query = @"
SELECT 
    [ID],[APPLY_DATE],[APPLY_TIME],[ACCT_ID],[NAME],[TOT_LIMIT],[APPLY_AMT],[USESITE],[PLACE],
    [ADJUST_DATE_S],[ADJUST_DATE_E],[REASON1],[REASON2],[REASON3],[REASON],[REMARK],[FAC_AUTH],
    [APPROVE_AMT_MAX],[USABILITY_AMT],[OVERPAY_AMT_PRO],[APPROVE_AMT],[OVERPAY_AMT],[ESTIMATE_RESULT],
    [REJECTREASON],[APPROVE_RESULT],[CHIEF_FLAG],[CHIEF_REMARK],[PENDING_FLAG],[USER_ID],[USER_NAME],
    [CHIEF_ID],[CHIEF_NAME],[JCIC_DATE],[TYPE],[CCAS_CODE],[CCAS_STATUS],[CCAS_DT],[PROC_DATE],[PROC_TIME],
    [ICARE_YN],[PRJ_YN],[PRJ_RESULT],[PRJ_REJECTREASON],[CREDIT_AMT]
FROM [RG_ADJUST]
WHERE ID = @Id;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar)
                {
                    Value = customerId
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<AdjustDO>();

                    AdjustDO tempAdjust = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        tempAdjust = ConvertAdjustDO(dr);

                        result.Add(tempAdjust);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result ?? new List<AdjustDO>();
        }

        /// <summary>
        /// 轉換臨調處理資訊
        /// </summary>
        /// <param name="adjustInfo">臨調處理資訊</param>
        /// <returns></returns>
        private AdjustDO ConvertAdjustDO(DataRow adjustInfo)
        {
            return new AdjustDO()
            {
                Id = adjustInfo.Field<string>("ID"),
                ApplyDate = adjustInfo.Field<string>("APPLY_DATE"),
                ApplyTime = adjustInfo.Field<string>("APPLY_TIME"),
                CustomerId = adjustInfo.Field<string>("ACCT_ID"),
                CustomerName = adjustInfo.Field<string>("NAME"),
                CreditLimit = adjustInfo.Field<decimal?>("TOT_LIMIT"),
                ApplyAmount = adjustInfo.Field<decimal?>("APPLY_AMT"),
                UseSite = adjustInfo.Field<string>("USESITE"),
                Place = adjustInfo.Field<string>("PLACE"),
                AdjustDateStart = adjustInfo.Field<string>("ADJUST_DATE_S"),
                AdjustDateEnd = adjustInfo.Field<string>("ADJUST_DATE_E"),
                Reason1 = adjustInfo.Field<string>("REASON1"),
                Reason2 = adjustInfo.Field<string>("REASON2"),
                Reason3 = adjustInfo.Field<string>("REASON3"),
                Reason = adjustInfo.Field<string>("REASON"),
                Remark = adjustInfo.Field<string>("REMARK"),
                ForceAuthenticate = adjustInfo.Field<string>("FAC_AUTH"),
                ApproveAmountMax = adjustInfo.Field<decimal?>("APPROVE_AMT_MAX"),
                UsabilityAmount = adjustInfo.Field<decimal?>("USABILITY_AMT"),
                OverpayAmountPro = adjustInfo.Field<decimal?>("OVERPAY_AMT_PRO"),
                ApproveAmount = adjustInfo.Field<decimal?>("APPROVE_AMT"),
                OverpayAmount = adjustInfo.Field<decimal?>("OVERPAY_AMT"),
                EstimateResult = adjustInfo.Field<string>("ESTIMATE_RESULT"),
                RejectReason = adjustInfo.Field<string>("REJECTREASON"),
                ApproveResult = adjustInfo.Field<string>("APPROVE_RESULT"),
                ChiefFlag = adjustInfo.Field<string>("CHIEF_FLAG"),
                ChiefRemark = adjustInfo.Field<string>("CHIEF_REMARK"),
                PendingFlag = adjustInfo.Field<string>("PENDING_FLAG"),
                UserId = adjustInfo.Field<string>("USER_ID"),
                UserName = adjustInfo.Field<string>("USER_NAME"),
                ChiefId = adjustInfo.Field<string>("CHIEF_ID"),
                ChiefName = adjustInfo.Field<string>("CHIEF_NAME"),
                JcicDate = adjustInfo.Field<string>("JCIC_DATE"),
                Type = adjustInfo.Field<string>("TYPE"),
                CcasCode = adjustInfo.Field<string>("CCAS_CODE"),
                CcasStatus = adjustInfo.Field<string>("CCAS_STATUS"),
                CcasDateTime = adjustInfo.Field<string>("CCAS_DT"),
                ProcessDate = adjustInfo.Field<string>("PROC_DATE"),
                ProcessTime = adjustInfo.Field<string>("PROC_TIME"),
                IcareStatus = adjustInfo.Field<string>("ICARE_YN"),
                ProjectStatus = adjustInfo.Field<string>("PRJ_YN"),
                ProjectAdjustResult = adjustInfo.Field<string>("PRJ_RESULT"),
                ProjectAdjustRejectReason = adjustInfo.Field<string>("PRJ_REJECTREASON"),
                CreditAmount = adjustInfo.Field<decimal?>("CREDIT_AMT"),
            };
        }
    }
}
