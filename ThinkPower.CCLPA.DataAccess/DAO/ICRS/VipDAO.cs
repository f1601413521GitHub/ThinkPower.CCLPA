using System;
using System.Data;
using System.Data.SqlClient;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;

namespace ThinkPower.CCLPA.DataAccess.DAO.ICRS
{
    /// <summary>
    /// 貴賓資訊資料存取類別
    /// </summary>
    public class VipDAO : BaseDAO
    {
        /// <summary>
        /// 取得貴賓資訊
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="date">資料年月</param>
        /// <returns></returns>
        public VipDO Get(string customerId, DateTime date)
        {
            VipDO result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }
            else if (date == null)
            {
                throw new ArgumentNullException("date");
            }



            string query = @"
SELECT
    [CST_ID],[YYYYMM],[UPD_DT],[ELG_VIP_TP_ID],[ELG_VIP_TP_EFF_DT],[VIP_TP_ID],
    [VIP_TP_EFF_DT],[VIP_BAL_AMT],[DEP_AVG_BAL_AMT],[NT_INVSM_BAL_INCLD_PREPAID],
    [NT_CMLV_PREM_AMT],[AUM],[LST_1YR_CRD_SPND_TOT_AMT],[MRTG_BAL],[FRX_EXG_MRG_TRD],
    [CB_NMNL_PRNCPL],[FEIS_SUB_BRKRG],[CRD_VIP_TOB_FLG],[HW_FLG],[CRP_FLG]
FROM [OD_VIP]
WHERE CST_ID = @CustomerId
    AND YYYYMM = @Date;";

            using (SqlConnection connection = DbConnection(Connection.ICRS))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)
                {
                    Value = customerId
                });

                command.Parameters.Add(new SqlParameter("@Date", SqlDbType.NVarChar)
                {
                    Value = date.ToString("yyyyMM")
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    throw new InvalidOperationException("VipData not found");
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("VipData not the only");
                }
                else if (dt.Rows.Count == 1)
                {
                    result = ConvertVipDO(dt.Rows[0]);
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換貴賓資訊
        /// </summary>
        /// <param name="vipInfo">貴賓資訊</param>
        /// <returns></returns>
        private VipDO ConvertVipDO(DataRow vipInfo)
        {
            return new VipDO()
            {
                CustomerId = vipInfo.Field<string>("CST_ID"),
                DataDate = vipInfo.Field<string>("YYYYMM"),
                DataChangeDate = vipInfo.Field<string>("UPD_DT"),
                ApplicableStarLevel = vipInfo.Field<decimal?>("ELG_VIP_TP_ID"),
                ApplicableStarValidityPeriod = vipInfo.Field<string>("ELG_VIP_TP_EFF_DT"),
                MonthStarLevel = vipInfo.Field<decimal?>("VIP_TP_ID"),
                MonthStarValidityPeriod = vipInfo.Field<string>("VIP_TP_EFF_DT"),
                BusinessBalance = vipInfo.Field<decimal?>("VIP_BAL_AMT"),
                AverageBalance = vipInfo.Field<decimal?>("DEP_AVG_BAL_AMT"),
                InventotyBalance = vipInfo.Field<decimal?>("NT_INVSM_BAL_INCLD_PREPAID"),
                PremiunsPaid = vipInfo.Field<decimal?>("NT_CMLV_PREM_AMT"),
                AUM = vipInfo.Field<decimal?>("AUM"),
                NearlyYearSwipeAmount = vipInfo.Field<decimal?>("LST_1YR_CRD_SPND_TOT_AMT"),
                MortgageBalance = vipInfo.Field<decimal?>("MRTG_BAL"),
                ForexMargin = vipInfo.Field<decimal?>("FRX_EXG_MRG_TRD"),
                ConvertiblePrincipal = vipInfo.Field<decimal?>("CB_NMNL_PRNCPL"),
                ReDelegate = vipInfo.Field<decimal?>("FEIS_SUB_BRKRG"),
                CardVipFlag = vipInfo.Field<decimal?>("CRD_VIP_TOB_FLG"),
                HouseholdExceptionStars = vipInfo.Field<decimal?>("HW_FLG"),
                LegalExceptionStars = vipInfo.Field<decimal?>("CRP_FLG"),
            };
        }
    }
}