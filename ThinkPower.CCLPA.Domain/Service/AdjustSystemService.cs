using System;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.VO;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// CDRM臨調系統服務
    /// </summary>
    public class AdjustSystemService : BaseService, IAdjustSystem
    {
        #region Property

        private AdjustSystemDAO _creditDAO;

        /// <summary>
        /// 授信系統
        /// </summary>
        public AdjustSystemDAO CreditDAO
        {
            get
            {
                if (_creditDAO == null)
                {
                    _creditDAO = new AdjustSystemDAO();
                }

                return _creditDAO;
            }
        }

        #endregion



        #region PublicMethod

        /// <summary>
        /// JCIC送查日期回傳
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public JcicSendQueryResult JcicSendQuery(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException(nameof(UserInfo));
            }

            JcicQueryResult jcicDateResult = new AdjustSystemDAO().
                JcicSendQuery(customerId, UserInfo.Id, UserInfo.Name);

            if (jcicDateResult == null)
            {
                throw new InvalidOperationException($"{nameof(jcicDateResult)} not found");
            }

            return ConvertJcicSendQueryResult(jcicDateResult);
        }

        /// <summary>
        /// 專案臨調條件檢核
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="jcicQueryDate">JCIC送查日期</param>
        /// <param name="adjustReasonCode">臨調原因代碼</param>
        /// <returns></returns>
        public AdjustConditionValidateResult ValidateAdjustCondition(string customerId, string jcicQueryDate,
            string adjustReasonCode)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (String.IsNullOrEmpty(jcicQueryDate))
            {
                throw new ArgumentNullException(nameof(jcicQueryDate));
            }
            else if (String.IsNullOrEmpty(adjustReasonCode))
            {
                throw new ArgumentNullException(nameof(adjustReasonCode));
            }



            AdjustValidateResult result = CreditDAO.
                ValidateAdjustCondition(customerId, jcicQueryDate, adjustReasonCode);

            return ConvertAdjustConditionResult(result);
        }

        /// <summary>
        /// 預審生效條件檢核
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns></returns>
        public PreAdjustEffectResult ValidatePreAdjustEffectConfition(string id)
        {
            PreAdjustEffectResult result = null;

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            PreAdjustEffectResultDO preAdjustEffect = CreditDAO.ValidatePreAdjustEffectConfition(id);

            result = ConvertPreAdjustEffectEntity(preAdjustEffect);

            return result;
        }

        #endregion



        #region PrivateMethod

        /// <summary>
        /// 轉換JCIC送查日期
        /// </summary>
        /// <param name="jcicDateResult">JCIC送查日期</param>
        /// <returns></returns>
        private JcicSendQueryResult ConvertJcicSendQueryResult(JcicQueryResult jcicDateResult)
        {
            return (jcicDateResult == null) ? null : new JcicSendQueryResult()
            {
                JcicQueryDate = jcicDateResult.JcicQueryDate,
                ResponseCode = jcicDateResult.ResponseCode,
            };
        }

        /// <summary>
        /// 轉換臨調檢核結果
        /// </summary>
        /// <param name="adjustValidateResult">臨調檢核結果</param>
        /// <returns></returns>
        private AdjustConditionValidateResult ConvertAdjustConditionResult(AdjustValidateResult adjustValidateResult)
        {
            return (adjustValidateResult == null) ? null : new AdjustConditionValidateResult()
            {
                EstimateResult = adjustValidateResult.EstimateResult,
                ProjectRejectReason = adjustValidateResult.ProjectRejectReason,
                ProjectResult = adjustValidateResult.ProjectResult,
                RejectReason = adjustValidateResult.RejectReason,
                ResponseCode = adjustValidateResult.ResponseCode,
            };
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
                throw new ArgumentNullException(nameof(preAdjustEffect));
            }

            return new PreAdjustEffectResult()
            {
                RejectReason = preAdjustEffect.RejectReason,
                ResponseCode = preAdjustEffect.ResponseCode,
            };
        }

        #endregion
    }
}