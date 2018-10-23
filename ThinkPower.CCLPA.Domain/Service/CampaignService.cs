using System;
using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service.Interface;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 行銷活動服務
    /// </summary>
    public class CampaignService : ICampaign
    {
        private CampaignEntity _campaignEntity;

        /// <summary>
        /// 此建構式將會設定行銷活動資訊
        /// </summary>
        /// <param name="campaignEntity"></param>
        public CampaignService(CampaignEntity campaignEntity)
        {
            _campaignEntity = campaignEntity;
        }

        public CampaignService() { }




        /// <summary>
        /// 取得行銷活動資訊
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        public CampaignEntity GetCampaign(string campaignId)
        {
            CampaignEntity campaignEntity = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            CampaignDO campaignDO = new CampaignDAO().Get(campaignId);

            if (campaignDO != null)
            {
                campaignEntity = new CampaignEntity()
                {
                    CampaignId = campaignDO.CampaignId,
                    CampaignName = campaignDO.CampaignName,
                    CampaignDescript = campaignDO.CampaignDescript,
                    CampaignTypeId = campaignDO.CampaignTypeId,
                    ProposalUnitNo = campaignDO.ProposalUnitNo,
                    ProposalEmployeeNo = campaignDO.ProposalEmployeeNo,
                    ProductId = campaignDO.ProductId,
                    SortPrinciple = campaignDO.SortPrinciple,
                    ExpectedStartDateTime = campaignDO.ExpectedStartDateTime,
                    ExpectedEndDateTime = campaignDO.ExpectedEndDateTime,
                    ExpectedCloseDate = campaignDO.ExpectedCloseDate,
                    DetailDescript = campaignDO.DetailDescript,
                    ExecutionChannel = campaignDO.ExecutionChannel,
                    ActivityFrequency = campaignDO.ActivityFrequency,
                    BaseDescript = campaignDO.BaseDescript,
                    ApproveState = campaignDO.ApproveState,
                    AssignMIS = campaignDO.AssignMIS,
                    CreatedDate = campaignDO.CreatedDate,
                    LastMaintenanceDate = campaignDO.LastMaintenanceDate,
                    CrossSellProposalNotes = campaignDO.CrossSellProposalNotes,
                };
            }

            return campaignEntity;
        }

        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <returns>行銷活動名單數量</returns>
        internal int? GetDetailCount()
        {
            if (_campaignEntity == null)
            {
                throw new ArgumentNullException("_campaignEntity");
            }

            return new CampaignDetailDAO().Count(_campaignEntity.CampaignId,
                _campaignEntity.ExecutionChannel);
        }

        /// <summary>
        /// 取得行銷活動名單
        /// </summary>
        /// <returns>行銷活動名單資料集合</returns>
        internal IEnumerable<CampaignDetailEntity> GetDetailList()
        {
            List<CampaignDetailEntity> detailList = null;

            if (_campaignEntity==null)
            {
                throw new ArgumentNullException("_campaignEntity");
            }

            IEnumerable<CampaignDetailDO> campaignDetailList = new CampaignDetailDAO().Get(
                _campaignEntity.CampaignId, _campaignEntity.ExecutionChannel);

            if (campaignDetailList != null)
            {
                detailList = new List<CampaignDetailEntity>();

                foreach (CampaignDetailDO campaignDetail in campaignDetailList)
                {
                    detailList.Add(new CampaignDetailEntity()
                    {
                        CampaignId = campaignDetail.CampaignId,
                        Sequence = campaignDetail.Sequence,
                        SchemeId = campaignDetail.SchemeId,
                        PathwayId = campaignDetail.PathwayId,
                        GroupId = campaignDetail.GroupId,
                        CustomerId = campaignDetail.CustomerId,
                        PerformStartDate = campaignDetail.PerformStartDate,
                        PerformEndDate = campaignDetail.PerformEndDate,
                        Col1 = campaignDetail.Col1,
                        Col2 = campaignDetail.Col2,
                        Col3 = campaignDetail.Col3,
                        Col4 = campaignDetail.Col4,
                        Col5 = campaignDetail.Col5,
                        Col6 = campaignDetail.Col6,
                        Col7 = campaignDetail.Col7,
                        Col8 = campaignDetail.Col8,
                        Col9 = campaignDetail.Col9,
                        Col10 = campaignDetail.Col10,
                        Col11 = campaignDetail.Col11,
                        Col12 = campaignDetail.Col12,
                        Col13 = campaignDetail.Col13,
                        Col14 = campaignDetail.Col14,
                        Col15 = campaignDetail.Col15,
                        Col16 = campaignDetail.Col16,
                        Col17 = campaignDetail.Col17,
                        Col18 = campaignDetail.Col18,
                        Col19 = campaignDetail.Col19,
                        Col20 = campaignDetail.Col20,
                        UpdateDate = campaignDetail.UpdateDate,
                        UpdateFileName = campaignDetail.UpdateFileName,
                        MtnDt = campaignDetail.MtnDt,
                    });
                }
            }


            return detailList;
        }
    }
}