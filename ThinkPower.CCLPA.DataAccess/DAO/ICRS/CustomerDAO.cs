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
    public class CustomerDAO : BaseDAO
    {
        /// <summary>
        /// 取得歸戶部分基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public CustomerShortDO GetShortData(string customerId)
        {
            CustomerShortDO result = null;

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
                command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)
                {
                    Value = customerId
                });

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    result = ConvertCustomerShortDO(dt.Rows[0]);
                }
                else
                {
                    throw new InvalidOperationException("CustomerData not the only");
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result;
        }

        /// <summary>
        /// 轉換歸戶部分基本資料
        /// </summary>
        /// <param name="customerInfo">歸戶基本資料</param>
        /// <returns></returns>
        private CustomerShortDO ConvertCustomerShortDO(DataRow customerInfo)
        {
            return new CustomerShortDO()
            {
                ChineseName = customerInfo.Field<string>("CHI_NAME"),
                MobileTel = customerInfo.Field<string>("MOBIL_TEL"),
                ClosingDay = customerInfo.Field<string>("STMT_CYCLE_DESC"),
                PayDeadline = customerInfo.Field<string>("PAY_DEADLINE"),
            };
        }

        /// <summary>
        /// 轉換歸戶基本資料
        /// </summary>
        /// <param name="customerInfo">歸戶基本資料</param>
        /// <returns></returns>
        private CustomerDO ConvertCustomerDO(DataRow customerInfo)
        {
            return new CustomerDO()
            {
                AccountId = customerInfo.Field<string>("ACCT_ID"),
                ChineseName = customerInfo.Field<string>("CHI_NAME"),
                BirthDay = customerInfo.Field<DateTime?>("BIRTH_DATE"),
                RiskLevel = customerInfo.Field<string>("RISK_LEVEL"),
                RiskRating = customerInfo.Field<string>("CA_REVOLVE_LEVEL"),
                CreditLimit = customerInfo.Field<decimal?>("TOT_LIMIT"),
                AboutDataStatus = customerInfo.Field<string>("ACNO_STATUS"),
                IssueDate = customerInfo.Field<DateTime?>("CA_FIRST_CARD_ISSU_DATE"),
                LiveCardCount = customerInfo.Field<decimal?>("LV_CARD_COUNT"),
                Status = customerInfo.Field<string>("STATUS"),
                Vocation = customerInfo.Field<string>("VOCATION"),
                BillAddr = customerInfo.Field<string>("BILL_ADDR"),
                TelOffice = customerInfo.Field<string>("TEL_OFFICE"),
                TelHome = customerInfo.Field<string>("TEL_HOME"),
                MobileTel = customerInfo.Field<string>("MOBIL_TEL"),
                Latest1Mnth = customerInfo.Field<string>("LATEST_1_MNTH"),
                Latest2Mnth = customerInfo.Field<string>("LATEST_2_MNTH"),
                Latest3Mnth = customerInfo.Field<string>("LATEST_3_MNTH"),
                Latest4Mnth = customerInfo.Field<string>("LATEST_4_MNTH"),
                Latest5Mnth = customerInfo.Field<string>("LATEST_5_MNTH"),
                Latest6Mnth = customerInfo.Field<string>("LATEST_6_MNTH"),
                Latest7Mnth = customerInfo.Field<string>("LATEST_7_MNTH"),
                Latest8Mnth = customerInfo.Field<string>("LATEST_8_MNTH"),
                Latest9Mnth = customerInfo.Field<string>("LATEST_9_MNTH"),
                Latest10Mnth = customerInfo.Field<string>("LATEST_10_MNTH"),
                Latest11Mnth = customerInfo.Field<string>("LATEST_11_MNTH"),
                Latest12Mnth = customerInfo.Field<string>("LATEST_12_MNTH"),
                Consume1 = customerInfo.Field<decimal?>("CONSUME_1"),
                Consume2 = customerInfo.Field<decimal?>("CONSUME_2"),
                Consume3 = customerInfo.Field<decimal?>("CONSUME_3"),
                Consume4 = customerInfo.Field<decimal?>("CONSUME_4"),
                Consume5 = customerInfo.Field<decimal?>("CONSUME_5"),
                Consume6 = customerInfo.Field<decimal?>("CONSUME_6"),
                Consume7 = customerInfo.Field<decimal?>("CONSUME_7"),
                Consume8 = customerInfo.Field<decimal?>("CONSUME_8"),
                Consume9 = customerInfo.Field<decimal?>("CONSUME_9"),
                Consume10 = customerInfo.Field<decimal?>("CONSUME_10"),
                Consume11 = customerInfo.Field<decimal?>("CONSUME_11"),
                Consume12 = customerInfo.Field<decimal?>("CONSUME_12"),
                PreCash1 = customerInfo.Field<decimal?>("PRECASH_1"),
                PreCash2 = customerInfo.Field<decimal?>("PRECASH_2"),
                PreCash3 = customerInfo.Field<decimal?>("PRECASH_3"),
                PreCash4 = customerInfo.Field<decimal?>("PRECASH_4"),
                PreCash5 = customerInfo.Field<decimal?>("PRECASH_5"),
                PreCash6 = customerInfo.Field<decimal?>("PRECASH_6"),
                PreCash7 = customerInfo.Field<decimal?>("PRECASH_7"),
                PreCash8 = customerInfo.Field<decimal?>("PRECASH_8"),
                PreCash9 = customerInfo.Field<decimal?>("PRECASH_9"),
                PreCash10 = customerInfo.Field<decimal?>("PRECASH_10"),
                PreCash11 = customerInfo.Field<decimal?>("PRECASH_11"),
                PreCash12 = customerInfo.Field<decimal?>("PRECASH_12"),
                CreditRating1 = customerInfo.Field<string>("UN_PAYMENT_RATE_1"),
                CreditRating2 = customerInfo.Field<string>("UN_PAYMENT_RATE_2"),
                CreditRating3 = customerInfo.Field<string>("UN_PAYMENT_RATE_3"),
                CreditRating4 = customerInfo.Field<string>("UN_PAYMENT_RATE_4"),
                CreditRating5 = customerInfo.Field<string>("UN_PAYMENT_RATE_5"),
                CreditRating6 = customerInfo.Field<string>("UN_PAYMENT_RATE_6"),
                CreditRating7 = customerInfo.Field<string>("UN_PAYMENT_RATE_7"),
                CreditRating8 = customerInfo.Field<string>("UN_PAYMENT_RATE_8"),
                CreditRating9 = customerInfo.Field<string>("UN_PAYMENT_RATE_9"),
                CreditRating10 = customerInfo.Field<string>("UN_PAYMENT_RATE_10"),
                CreditRating11 = customerInfo.Field<string>("UN_PAYMENT_RATE_11"),
                CreditRating12 = customerInfo.Field<string>("UN_PAYMENT_RATE_12"),
                ClosingDay = customerInfo.Field<string>("STMT_CYCLE_DESC"),
                PayDeadline = customerInfo.Field<string>("PAY_DEADLINE"),
                ClosingAmount = customerInfo.Field<decimal?>("ACCT_TOT_BILL"),
                MinimumAmountPayable = customerInfo.Field<decimal?>("ACCT_MIN_PAY"),
                RecentPaymentAmount = customerInfo.Field<decimal?>("CA_LAST_PAY_AMT"),
                RecentPaymentDate = customerInfo.Field<DateTime?>("CA_LAST_PAY_DATE"),
                OfferAmount = customerInfo.Field<decimal?>("ACNO_FEIBDM_AMT"),
                UnpaidTotal = customerInfo.Field<decimal?>("C_CA_TOT_UNPAID_AMT"),
                AuthorizedAmountNotAccount = customerInfo.Field<decimal?>("C_CA_TOT_AMT_CONSUME"),
                AdjustReason = customerInfo.Field<string>("ADJ_REASON"),
                AdjustArea = customerInfo.Field<string>("ADJ_AREA"),
                AdjustStartDate = customerInfo.Field<string>("ADJ_EFF_START_DATE"),
                AdjustEndDate = customerInfo.Field<string>("ADJ_EFF_END_DATE"),
                AdjustEffectAmount = customerInfo.Field<decimal?>("ADK_EFF_AMT"),
                VintageMonths = customerInfo.Field<decimal?>("CA_VINTAGE_MONTHS"),
                StatusFlag = customerInfo.Field<string>("CA_SPEC_STATUS_FLAG"),
                GutrFlag = customerInfo.Field<string>("GUTR_FLAG"),
                DelayCount = customerInfo.Field<decimal?>("DELAY_CNT"),
                CcasUsabilityAmount = customerInfo.Field<decimal?>("CCAS_UNPAID_AMT"),
                CcasUnderpaidAmount = customerInfo.Field<decimal?>("CCAS_USABILITY_AMT"),
                CcasUnderpaidRate = customerInfo.Field<decimal?>("CCAS_UNPAID_RATE"),
                DataDate = customerInfo.Field<string>("DATA_DATE"),
                EligibilityForWithdrawal = customerInfo.Field<string>("REVOLVER_YN"),
                SystemAdjustRevFlag = customerInfo.Field<string>("CA_SYSM_ADJ_REV_FLAG"),
                AutomaticDebit = customerInfo.Field<string>("ACNO_DEDUCT_STATUS"),
                DebitBankCode = customerInfo.Field<string>("ACNO_DEDUCT_BANK"),
                EtalStatus = customerInfo.Field<string>("ETAL_STATUS"),
                TelResident = customerInfo.Field<string>("IDNO_TEL_RESIDENT"),
                SendType = customerInfo.Field<string>("IDNO_STMT_SEND_TYPE"),
                ElectronicBillingCustomerNote = customerInfo.Field<string>("IDNO_IB_FLAG"),
                Email = customerInfo.Field<string>("IDNO_EMAIL_ADDR"),
                Industry = customerInfo.Field<string>("IDNO_IND_CAT"),
                JobTitle = customerInfo.Field<string>("IDNO_POSITION"),
                ResidentAddr = customerInfo.Field<string>("IDNO_RESIDENT_ADDR"),
                MailingAddr = customerInfo.Field<string>("IDNO_MAIL_ADDR"),
                CompanyAddr = customerInfo.Field<string>("IDNO_COMPANY_ADDR"),
                AnnualIncome = customerInfo.Field<decimal?>("IDNO_ANNUAL_INCOME"),
                In1 = customerInfo.Field<decimal?>("IDNO_IN_AMT"),
                In2 = customerInfo.Field<decimal?>("IDNO_IN2_AMT"),
                In3 = customerInfo.Field<decimal?>("IDNO_IN3_AMT"),
                ResidentAddrPostalCode = customerInfo.Field<string>("IDNO_RESIDENT_ZIP"),
                MailingAddrPostalCode = customerInfo.Field<string>("IDNO_MAIL_ZIP"),
                CompanyAddrPostalCode = customerInfo.Field<string>("IDNO_COMPANY_ZIP"),
            };
        }
    }
}
