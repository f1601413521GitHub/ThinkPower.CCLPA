using System;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 條件檢核類別
    /// </summary>
    public class ConditionValidateService : BaseService, IConditionValidate
    {
        private CreditSystemDAO _creditDAO;

        /// <summary>
        /// 授信系統
        /// </summary>
        public CreditSystemDAO CreditDAO
        {
            get
            {
                if (_creditDAO == null)
                {
                    _creditDAO = new CreditSystemDAO();
                }

                return _creditDAO;
            }
        }

        /// <summary>
        /// JCIC送查日期回傳
        /// </summary>
        /// <returns></returns>
        public object JcicSendDate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 專案臨調條件檢核
        /// </summary>
        /// <returns></returns>
        public object Adjust()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 預審生效條件檢核
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns></returns>
        public PreAdjustEffectResult PreAdjustEffect(string id)
        {
            PreAdjustEffectResult result = null;

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            PreAdjustEffectResultDO preAdjustEffect = CreditDAO.PreAdjustEffectCondition(id);

            result = ConvertPreAdjustEffectEntity(preAdjustEffect);

            return result;
        }

        /// <summary>
        /// 轉換預審生效條件檢核結果
        /// </summary>
        /// <param name="preAdjustEffect">預審生效條件檢核結果</param>
        /// <returns></returns>
        private PreAdjustEffectResult ConvertPreAdjustEffectEntity(PreAdjustEffectResultDO preAdjustEffect)
        {
            if (preAdjustEffect == null)
            {
                throw new ArgumentNullException("preAdjustEffect");
            }

            return new PreAdjustEffectResult()
            {
                RejectReason = preAdjustEffect.RejectReason,
                ResponseCode = preAdjustEffect.ResponseCode,
            };
        }
    }
}