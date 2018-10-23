using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 臨調預審服務
    /// </summary>
    public class PreAdjustService : IPreAdjust
    {
        private CampaignService _campaignService;

        /// <summary>
        /// 行銷活動服務
        /// </summary>
        private CampaignService CampaignService
        {
            get
            {
                if (_campaignService == null)
                {
                    _campaignService = new CampaignService();
                }

                return _campaignService;
            }
        }

        /// <summary>
        /// 使用者資訊
        /// </summary>
        public UserInfoVO UserInfo { get; set; }

        /// <summary>
        /// 匯入臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executeImport">是否執行匯入</param>
        /// <returns>行銷活動名單數量</returns>
        public int? ImportPreAdjust(string campaignId, bool executeImport = false)
        {
            int? campaignDetailCount = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }




            CampaignDO campaign = CampaignService.GetCampaign(campaignId);

            if (campaign == null)
            {
                var e = new InvalidOperationException("Campaign not found");
                e.Data["ErrorMsg"] = "ILRC行銷活動編碼，輸入錯誤。";
                throw e;

            }
            else if (!DateTime.TryParseExact(campaign.ExpectedCloseDate, "yyyyMMdd", null,
                DateTimeStyles.None, out DateTime tempCloseDate))
            {
                throw new InvalidOperationException("Convert ExpectedCloseDate Fail");
            }
            else if (tempCloseDate < DateTime.Now.Date)
            {
                var e = new InvalidOperationException("Campaign closed, not can't import data");
                e.Data["ErrorMsg"] = "此行銷活動已結案，無法進入匯入作業。";
                throw e;
            }




            CampaignImportLogDO campaignImportLog = CampaignService.GetImportLog(campaign.CampaignId);

            if (campaignImportLog != null)
            {
                var e = new InvalidOperationException("Campaign imported, not can't again import");
                e.Data["ErrorMsg"] = $"此行銷活動已於{campaignImportLog.ImportDate}匯入過，無法再進行匯入。";
                throw e;
            }




            if (!executeImport)
            {
                campaignDetailCount = CampaignService.GetCampaignDetailCount(
                    campaign.CampaignId, campaign.ExecutionChannel);

                return campaignDetailCount;
            }




            IEnumerable<CampaignDetailDO> campaignDetailList = CampaignService.GetDetailList(
                campaign.CampaignId, campaign.ExecutionChannel);

            if ((campaignDetailList == null) || (campaignDetailList.Count() == 0))
            {
                throw new InvalidOperationException("CampaignDetailList not found");
            }




            DateTime currentTime = DateTime.Now;

            CampaignImportLogDO importLog = new CampaignImportLogDO()
            {
                CampaignId = campaign.CampaignId,
                ExpectedStartDate = campaign.ExpectedStartDateTime,
                ExpectedEndDate = campaign.ExpectedEndDateTime,
                Count = campaignDetailList.Count(),
                ImportUserId = UserInfo.Id,
                ImportUserName = UserInfo.Name,
                ImportDate = currentTime.ToString("yyyy/MM/dd"),
            };

            List<PreAdjustDO> preAdjustList = new List<PreAdjustDO>();

            foreach (CampaignDetailDO campaignDetail in campaignDetailList)
            {
                preAdjustList.Add(new PreAdjustDO()
                {
                    CampaignId = campaign.CampaignId,
                    Id = campaignDetail.CustomerId,
                    ProjectName = campaignDetail.Col1,
                    ProjectAmount = Convert.ToDecimal(campaignDetail.Col2),
                    CloseDate = DateTime.TryParseExact(campaignDetail.Col3, "yyyyMMdd", null,
                        DateTimeStyles.None, out DateTime temp) ?
                        temp.ToString("yyyy/MM/dd") :
                        throw new InvalidOperationException("Convert campaignDetail Col3 Fail"),
                    ImportDate = currentTime.ToString("yyyy/MM/dd"),
                    Kind = campaignDetail.Col4,
                    Status = "待生效",
                });
            }




            CustomerDAO customerDAO = new CustomerDAO();
            CustomerShortDO customerShortData = null;
            PreAdjustDO tempPreAdjust = null;

            foreach (CampaignDetailDO item in campaignDetailList)
            {
                customerShortData = customerDAO.GetShortData(item.CustomerId);

                if (customerShortData == null)
                {
                    throw new InvalidOperationException("CustomerShortData not found");
                }

                tempPreAdjust = preAdjustList.FirstOrDefault(x => x.Id == item.CustomerId);

                if (tempPreAdjust == null)
                {
                    throw new InvalidOperationException("tempPreAdjust not found");
                }

                tempPreAdjust.ChineseName = customerShortData.ChineseName;
                tempPreAdjust.ClosingDay = customerShortData.ClosingDay;
                tempPreAdjust.PayDeadline = customerShortData.PayDeadline;
                tempPreAdjust.MobileTel = customerShortData.MobileTel;
            }



            SaveCampaignData(importLog, preAdjustList);



            return campaignDetailCount;
        }


        /// <summary>
        /// 處理臨調預審名單
        /// </summary>
        public void PreAdjustProcessing()
        {
            throw new NotImplementedException();
        }







        /// <summary>
        /// 紀錄行銷活動匯入紀錄與臨調預審處理檔
        /// </summary>
        /// <param name="importLog">行銷活動匯入紀錄</param>
        /// <param name="preAdjustList">臨調預審處理檔資料集合</param>
        /// <returns></returns>
        private void SaveCampaignData(CampaignImportLogDO importLog, List<PreAdjustDO> preAdjustList)
        {
            if (importLog == null)
            {
                throw new ArgumentNullException("importLog");
            }
            else if ((preAdjustList == null) || (preAdjustList.Count == 0))
            {
                throw new ArgumentNullException("preAdjustList");
            }

            using (TransactionScope scope = new TransactionScope())
            {
                new CampaignImportLogDAO().Insert(importLog);
                new PreAdjustDAO().Insert(preAdjustList);

                scope.Complete();
            }
        }
    }
}
