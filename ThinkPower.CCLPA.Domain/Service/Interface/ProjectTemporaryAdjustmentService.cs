using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 專案臨調服務
    /// </summary>
    public class ProjectTemporaryAdjustmentService : IProjectTemporaryAdjustment
    {
        /// <summary>
        /// 檢核預審名單
        /// </summary>
        /// <param name="cmpnId">行銷活動代號</param>
        /// <returns></returns>
        public TemporaryAdjustmentEntity ValidatePreTrialList(string cmpnId)
        {
            TemporaryAdjustmentEntity result = null;
            string validateResult = null;
            int? campaignListCount = null;


            if (String.IsNullOrEmpty(cmpnId))
            {
                throw new ArgumentNullException("cmpnId");
            }

            MarketingActivityFileDO activityInfo = new MarketingActivityFileDAO().Get(cmpnId);

            if (activityInfo == null)
            {
                validateResult = "ILRC行銷活動編碼，輸入錯誤。";
            }
            else if (!DateTime.TryParseExact(activityInfo.CMPN_EXPC_CLOSE_DT, "yyyyMMdd", null,
                System.Globalization.DateTimeStyles.None, out DateTime tempCloseDate))
            {
                throw new InvalidOperationException("Convert CMPN_EXPC_CLOSE_DT Fail");
            }
            else if (tempCloseDate < DateTime.Now.Date)
            {
                validateResult = "此行銷活動已結案，無法進入匯入作業。";
            }

            if (String.IsNullOrEmpty(validateResult))
            {
                MarketingActivitiesRecordFileDO recordList = new MarketingActivitiesRecordFileDAO().Get(
                    activityInfo.CMPN_ID);

                if (recordList != null)
                {
                    validateResult = $"此行銷活動已於{recordList.IMPORT_DT}匯入過，無法再進行匯入。";
                }

                campaignListCount = new MarketingCampaignListFileDAO().GetCampaignListCount(
                   activityInfo.CMPN_ID);
            }

            result = new TemporaryAdjustmentEntity()
            {
                ValidatePreTrialResult = new ValidatePreTrialEntity() {

                    ValidateMessage = validateResult,
                    CampaignListCount = campaignListCount,
                }
            };

            return result;
        }

        /// <summary>
        /// 預審名單匯入
        /// </summary>
        /// <param name="cmpnId">行銷活動代號</param>
        /// <returns></returns>
        public TemporaryAdjustmentEntity ImportPreTrialList(string cmpnId)
        {
            //Entity: 新增到行銷活動匯入紀錄檔(LOG_RG_ILRC)、臨調預審處理檔(RG_PADJUST)
            //MarketingActivitiesRecordEntity
            //TemporaryAdjustmentPreTrialProcessingEntity
            throw new NotImplementedException();
        }

        /// <summary>
        /// 預審名單處理
        /// </summary>
        public void PreTrialListProcessing()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 臨調處理
        /// </summary>
        public void Processing()
        {
            throw new NotImplementedException();
        }
    }
}
