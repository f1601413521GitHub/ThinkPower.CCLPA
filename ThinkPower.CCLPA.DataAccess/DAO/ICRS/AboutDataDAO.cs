using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;

namespace ThinkPower.CCLPA.DataAccess.DAO.ICRS
{
    /// <summary>
    /// 歸戶基本資料資料存取類別
    /// </summary>
    public class AboutDataDAO : BaseDAO
    {
        /// <summary>
        /// 取得預審處理檔所需的歸戶基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public AboutDataDO GetPreAdjustNeeded(string customerId)
        {
            AboutDataDO result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            string query = @"
SELECT 
    CHI_NAME,STMT_CYCLE_DESC,PAY_DEADLINE,MOBIL_TEL
FROM [RG_ID]
WHERE ACCT_ID =@CustomerId;";

            using (SqlConnection connection = DbConnection(Connection.ICRS))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.VarChar)
                {
                    Value = customerId
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertPreAdjustNeededAboutDataDO(dt.Rows[0]);
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換預審處理檔所需的歸戶基本資料
        /// </summary>
        /// <param name="aboutDataInfo">歸戶基本資料</param>
        /// <returns></returns>
        private AboutDataDO ConvertPreAdjustNeededAboutDataDO(DataRow aboutDataInfo)
        {
            return new AboutDataDO()
            {
                ChineseName = aboutDataInfo.Field<string>("CHI_NAME"),
                MobileTel = aboutDataInfo.Field<string>("MOBIL_TEL"),
                ClosingDay = aboutDataInfo.Field<string>("STMT_CYCLE_DESC"),
                PayDeadline = aboutDataInfo.Field<string>("PAY_DEADLINE"),
            };
        }

        /// <summary>
        /// 轉換歸戶基本資料
        /// </summary>
        /// <param name="aboutDataInfo">歸戶基本資料</param>
        /// <returns></returns>
        private AboutDataDO ConvertAboutDataDO(DataRow aboutDataInfo)
        {
            return new AboutDataDO()
            {
                AccountId = aboutDataInfo.Field<string>("ACCT_ID"),
                ChineseName = aboutDataInfo.Field<string>("CHI_NAME"),
                BirthDay = aboutDataInfo.Field<DateTime?>("BIRTH_DATE"),
                RiskLevel = aboutDataInfo.Field<string>("RISK_LEVEL"),
                RiskRating = aboutDataInfo.Field<string>("CA_REVOLVE_LEVEL"),
                CreditLimit = aboutDataInfo.Field<decimal?>("TOT_LIMIT"),
                AboutDataStatus = aboutDataInfo.Field<string>("ACNO_STATUS"),
                IssueDate = aboutDataInfo.Field<DateTime?>("CA_FIRST_CARD_ISSU_DATE"),
                LiveCardCount = aboutDataInfo.Field<decimal?>("LV_CARD_COUNT"),
                Status = aboutDataInfo.Field<string>("STATUS"),
                Vocation = aboutDataInfo.Field<string>("VOCATION"),
                BillAddr = aboutDataInfo.Field<string>("BILL_ADDR"),
                TelOffice = aboutDataInfo.Field<string>("TEL_OFFICE"),
                TelHome = aboutDataInfo.Field<string>("TEL_HOME"),
                MobileTel = aboutDataInfo.Field<string>("MOBIL_TEL"),
                Latest1Mnth = aboutDataInfo.Field<string>("LATEST_1_MNTH"),
                Latest2Mnth = aboutDataInfo.Field<string>("LATEST_2_MNTH"),
                Latest3Mnth = aboutDataInfo.Field<string>("LATEST_3_MNTH"),
                Latest4Mnth = aboutDataInfo.Field<string>("LATEST_4_MNTH"),
                Latest5Mnth = aboutDataInfo.Field<string>("LATEST_5_MNTH"),
                Latest6Mnth = aboutDataInfo.Field<string>("LATEST_6_MNTH"),
                Latest7Mnth = aboutDataInfo.Field<string>("LATEST_7_MNTH"),
                Latest8Mnth = aboutDataInfo.Field<string>("LATEST_8_MNTH"),
                Latest9Mnth = aboutDataInfo.Field<string>("LATEST_9_MNTH"),
                Latest10Mnth = aboutDataInfo.Field<string>("LATEST_10_MNTH"),
                Latest11Mnth = aboutDataInfo.Field<string>("LATEST_11_MNTH"),
                Latest12Mnth = aboutDataInfo.Field<string>("LATEST_12_MNTH"),
                Consume1 = aboutDataInfo.Field<decimal?>("CONSUME_1"),
                Consume2 = aboutDataInfo.Field<decimal?>("CONSUME_2"),
                Consume3 = aboutDataInfo.Field<decimal?>("CONSUME_3"),
                Consume4 = aboutDataInfo.Field<decimal?>("CONSUME_4"),
                Consume5 = aboutDataInfo.Field<decimal?>("CONSUME_5"),
                Consume6 = aboutDataInfo.Field<decimal?>("CONSUME_6"),
                Consume7 = aboutDataInfo.Field<decimal?>("CONSUME_7"),
                Consume8 = aboutDataInfo.Field<decimal?>("CONSUME_8"),
                Consume9 = aboutDataInfo.Field<decimal?>("CONSUME_9"),
                Consume10 = aboutDataInfo.Field<decimal?>("CONSUME_10"),
                Consume11 = aboutDataInfo.Field<decimal?>("CONSUME_11"),
                Consume12 = aboutDataInfo.Field<decimal?>("CONSUME_12"),
                PreCash1 = aboutDataInfo.Field<decimal?>("PRECASH_1"),
                PreCash2 = aboutDataInfo.Field<decimal?>("PRECASH_2"),
                PreCash3 = aboutDataInfo.Field<decimal?>("PRECASH_3"),
                PreCash4 = aboutDataInfo.Field<decimal?>("PRECASH_4"),
                PreCash5 = aboutDataInfo.Field<decimal?>("PRECASH_5"),
                PreCash6 = aboutDataInfo.Field<decimal?>("PRECASH_6"),
                PreCash7 = aboutDataInfo.Field<decimal?>("PRECASH_7"),
                PreCash8 = aboutDataInfo.Field<decimal?>("PRECASH_8"),
                PreCash9 = aboutDataInfo.Field<decimal?>("PRECASH_9"),
                PreCash10 = aboutDataInfo.Field<decimal?>("PRECASH_10"),
                PreCash11 = aboutDataInfo.Field<decimal?>("PRECASH_11"),
                PreCash12 = aboutDataInfo.Field<decimal?>("PRECASH_12"),
                CreditRating1 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_1"),
                CreditRating2 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_2"),
                CreditRating3 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_3"),
                CreditRating4 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_4"),
                CreditRating5 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_5"),
                CreditRating6 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_6"),
                CreditRating7 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_7"),
                CreditRating8 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_8"),
                CreditRating9 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_9"),
                CreditRating10 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_10"),
                CreditRating11 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_11"),
                CreditRating12 = aboutDataInfo.Field<string>("UN_PAYMENT_RATE_12"),
                ClosingDay = aboutDataInfo.Field<string>("STMT_CYCLE_DESC"),
                PayDeadline = aboutDataInfo.Field<string>("PAY_DEADLINE"),
                ClosingAmount = aboutDataInfo.Field<decimal?>("ACCT_TOT_BILL"),
                MinimumAmountPayable = aboutDataInfo.Field<decimal?>("ACCT_MIN_PAY"),
                RecentPaymentAmount = aboutDataInfo.Field<decimal?>("CA_LAST_PAY_AMT"),
                RecentPaymentDate = aboutDataInfo.Field<DateTime?>("CA_LAST_PAY_DATE"),
                OfferAmount = aboutDataInfo.Field<decimal?>("ACNO_FEIBDM_AMT"),
                UnpaidTotal = aboutDataInfo.Field<decimal?>("C_CA_TOT_UNPAID_AMT"),
                AuthorizedAmountNotAccount = aboutDataInfo.Field<decimal?>("C_CA_TOT_AMT_CONSUME"),
                AdjustReason = aboutDataInfo.Field<string>("ADJ_REASON"),
                AdjustArea = aboutDataInfo.Field<string>("ADJ_AREA"),
                AdjustStartDate = aboutDataInfo.Field<string>("ADJ_EFF_START_DATE"),
                AdjustEndDate = aboutDataInfo.Field<string>("ADJ_EFF_END_DATE"),
                AdjustEffectAmount = aboutDataInfo.Field<decimal?>("ADK_EFF_AMT"),
                VintageMonths = aboutDataInfo.Field<decimal?>("CA_VINTAGE_MONTHS"),
                StatusFlag = aboutDataInfo.Field<string>("CA_SPEC_STATUS_FLAG"),
                GutrFlag = aboutDataInfo.Field<string>("GUTR_FLAG"),
                DelayCount = aboutDataInfo.Field<decimal?>("DELAY_CNT"),
                CcasUsabilityAmount = aboutDataInfo.Field<decimal?>("CCAS_UNPAID_AMT"),
                CcasUnderpaidAmount = aboutDataInfo.Field<decimal?>("CCAS_USABILITY_AMT"),
                CcasUnderpaidRate = aboutDataInfo.Field<decimal?>("CCAS_UNPAID_RATE"),
                DataDate = aboutDataInfo.Field<string>("DATA_DATE"),
                EligibilityForWithdrawal = aboutDataInfo.Field<string>("REVOLVER_YN"),
                SystemAdjustRevFlag = aboutDataInfo.Field<string>("CA_SYSM_ADJ_REV_FLAG"),
                AutomaticDebit = aboutDataInfo.Field<string>("ACNO_DEDUCT_STATUS"),
                DebitBankCode = aboutDataInfo.Field<string>("ACNO_DEDUCT_BANK"),
                EtalStatus = aboutDataInfo.Field<string>("ETAL_STATUS"),
                TelResident = aboutDataInfo.Field<string>("IDNO_TEL_RESIDENT"),
                SendType = aboutDataInfo.Field<string>("IDNO_STMT_SEND_TYPE"),
                ElectronicBillingCustomerNote = aboutDataInfo.Field<string>("IDNO_IB_FLAG"),
                Email = aboutDataInfo.Field<string>("IDNO_EMAIL_ADDR"),
                Industry = aboutDataInfo.Field<string>("IDNO_IND_CAT"),
                JobTitle = aboutDataInfo.Field<string>("IDNO_POSITION"),
                ResidentAddr = aboutDataInfo.Field<string>("IDNO_RESIDENT_ADDR"),
                MailingAddr = aboutDataInfo.Field<string>("IDNO_MAIL_ADDR"),
                CompanyAddr = aboutDataInfo.Field<string>("IDNO_COMPANY_ADDR"),
                AnnualIncome = aboutDataInfo.Field<decimal?>("IDNO_ANNUAL_INCOME"),
                In1 = aboutDataInfo.Field<decimal?>("IDNO_IN_AMT"),
                In2 = aboutDataInfo.Field<decimal?>("IDNO_IN2_AMT"),
                In3 = aboutDataInfo.Field<decimal?>("IDNO_IN3_AMT"),
                ResidentAddrPostalCode = aboutDataInfo.Field<string>("IDNO_RESIDENT_ZIP"),
                MailingAddrPostalCode = aboutDataInfo.Field<string>("IDNO_MAIL_ZIP"),
                CompanyAddrPostalCode = aboutDataInfo.Field<string>("IDNO_COMPANY_ZIP"),
            };
        }
    }
}
