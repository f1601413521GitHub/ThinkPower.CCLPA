using System;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 客戶服務
    /// </summary>
    public class CustomerService
    {
        /// <summary>
        /// 取得歸戶基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public Customer Get(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            CustomerDO customer = new CustomerDAO().Get(customerId);

            return ConvertCustomerInfo(customer);
        }

        /// <summary>
        /// 取得貴賓資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="date">資料年月</param>
        /// <returns></returns>
        public VipInfo GetVip(string customerId, DateTime? date)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (date == null)
            {
                throw new ArgumentNullException(nameof(date));
            }

            VipDO vipData = new VipDAO().Get(customerId, date.Value);

            return ConvertVipInfo(vipData);
        }

        /// <summary>
        /// 轉換貴賓資料
        /// </summary>
        /// <param name="vipData">貴賓資料</param>
        /// <returns></returns>
        private VipInfo ConvertVipInfo(VipDO vipData)
        {
            return (vipData == null) ? null : new VipInfo()
            {
                CustomerId = vipData.CustomerId,
                DataDate = vipData.DataDate,
                DataChangeDate = vipData.DataChangeDate,
                ApplicableStarLevel = vipData.ApplicableStarLevel,
                ApplicableStarValidityPeriod = vipData.ApplicableStarValidityPeriod,
                MonthStarLevel = vipData.MonthStarLevel,
                MonthStarValidityPeriod = vipData.MonthStarValidityPeriod,
                BusinessBalance = vipData.BusinessBalance,
                AverageBalance = vipData.AverageBalance,
                InventotyBalance = vipData.InventotyBalance,
                PremiunsPaid = vipData.PremiunsPaid,
                AUM = vipData.AUM,
                NearlyYearSwipeAmount = vipData.NearlyYearSwipeAmount,
                MortgageBalance = vipData.MortgageBalance,
                ForexMargin = vipData.ForexMargin,
                ConvertiblePrincipal = vipData.ConvertiblePrincipal,
                ReDelegate = vipData.ReDelegate,
                CardVipFlag = vipData.CardVipFlag,
                HouseholdExceptionStars = vipData.HouseholdExceptionStars,
                LegalExceptionStars = vipData.LegalExceptionStars,
            };
        }





        /// <summary>
        /// 轉換歸戶基本資料
        /// </summary>
        /// <param name="customer">歸戶基本資料</param>
        /// <returns></returns>
        private Customer ConvertCustomerInfo(CustomerDO customer)
        {
            return (customer == null) ? null : new Customer()
            {
                AccountId = customer.AccountId,
                ChineseName = customer.ChineseName,
                BirthDay = customer.BirthDay,
                RiskLevel = customer.RiskLevel,
                RiskRating = customer.RiskRating,
                CreditLimit = customer.CreditLimit,
                AboutDataStatus = customer.AboutDataStatus,
                IssueDate = customer.IssueDate,
                LiveCardCount = customer.LiveCardCount,
                Status = customer.Status,
                Vocation = customer.Vocation,
                BillAddr = customer.BillAddr,
                TelOffice = customer.TelOffice,
                TelHome = customer.TelHome,
                MobileTel = customer.MobileTel,
                Latest1Mnth = customer.Latest1Mnth,
                Latest2Mnth = customer.Latest2Mnth,
                Latest3Mnth = customer.Latest3Mnth,
                Latest4Mnth = customer.Latest4Mnth,
                Latest5Mnth = customer.Latest5Mnth,
                Latest6Mnth = customer.Latest6Mnth,
                Latest7Mnth = customer.Latest7Mnth,
                Latest8Mnth = customer.Latest8Mnth,
                Latest9Mnth = customer.Latest9Mnth,
                Latest10Mnth = customer.Latest10Mnth,
                Latest11Mnth = customer.Latest11Mnth,
                Latest12Mnth = customer.Latest12Mnth,
                Consume1 = customer.Consume1,
                Consume2 = customer.Consume2,
                Consume3 = customer.Consume3,
                Consume4 = customer.Consume4,
                Consume5 = customer.Consume5,
                Consume6 = customer.Consume6,
                Consume7 = customer.Consume7,
                Consume8 = customer.Consume8,
                Consume9 = customer.Consume9,
                Consume10 = customer.Consume10,
                Consume11 = customer.Consume11,
                Consume12 = customer.Consume12,
                PreCash1 = customer.PreCash1,
                PreCash2 = customer.PreCash2,
                PreCash3 = customer.PreCash3,
                PreCash4 = customer.PreCash4,
                PreCash5 = customer.PreCash5,
                PreCash6 = customer.PreCash6,
                PreCash7 = customer.PreCash7,
                PreCash8 = customer.PreCash8,
                PreCash9 = customer.PreCash9,
                PreCash10 = customer.PreCash10,
                PreCash11 = customer.PreCash11,
                PreCash12 = customer.PreCash12,
                CreditRating1 = customer.CreditRating1,
                CreditRating2 = customer.CreditRating2,
                CreditRating3 = customer.CreditRating3,
                CreditRating4 = customer.CreditRating4,
                CreditRating5 = customer.CreditRating5,
                CreditRating6 = customer.CreditRating6,
                CreditRating7 = customer.CreditRating7,
                CreditRating8 = customer.CreditRating8,
                CreditRating9 = customer.CreditRating9,
                CreditRating10 = customer.CreditRating10,
                CreditRating11 = customer.CreditRating11,
                CreditRating12 = customer.CreditRating12,
                ClosingDay = customer.ClosingDay,
                PayDeadline = customer.PayDeadline,
                ClosingAmount = customer.ClosingAmount,
                MinimumAmountPayable = customer.MinimumAmountPayable,
                RecentPaymentAmount = customer.RecentPaymentAmount,
                RecentPaymentDate = customer.RecentPaymentDate,
                OfferAmount = customer.OfferAmount,
                UnpaidTotal = customer.UnpaidTotal,
                AuthorizedAmountNotAccount = customer.AuthorizedAmountNotAccount,
                AdjustReason = customer.AdjustReason,
                AdjustArea = customer.AdjustArea,
                AdjustStartDate = customer.AdjustStartDate,
                AdjustEndDate = customer.AdjustEndDate,
                AdjustEffectAmount = customer.AdjustEffectAmount,
                VintageMonths = customer.VintageMonths,
                StatusFlag = customer.StatusFlag,
                GutrFlag = customer.GutrFlag,
                DelayCount = customer.DelayCount,
                CcasUnderpaidAmount = customer.CcasUnderpaidAmount,
                CcasUsabilityAmount = customer.CcasUsabilityAmount,
                CcasUnderpaidRate = customer.CcasUnderpaidRate,
                DataDate = customer.DataDate,
                EligibilityForWithdrawal = customer.EligibilityForWithdrawal,
                SystemAdjustRevFlag = customer.SystemAdjustRevFlag,
                AutomaticDebit = customer.AutomaticDebit,
                DebitBankCode = customer.DebitBankCode,
                EtalStatus = customer.EtalStatus,
                TelResident = customer.TelResident,
                SendType = customer.SendType,
                ElectronicBillingCustomerNote = customer.ElectronicBillingCustomerNote,
                Email = customer.Email,
                Industry = customer.Industry,
                JobTitle = customer.JobTitle,
                ResidentAddr = customer.ResidentAddr,
                MailingAddr = customer.MailingAddr,
                CompanyAddr = customer.CompanyAddr,
                AnnualIncome = customer.AnnualIncome,
                In1 = customer.In1,
                In2 = customer.In2,
                In3 = customer.In3,
                ResidentAddrPostalCode = customer.ResidentAddrPostalCode,
                MailingAddrPostalCode = customer.MailingAddrPostalCode,
                CompanyAddrPostalCode = customer.CompanyAddrPostalCode,
            };
        }

    }
}