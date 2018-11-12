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

        /// <summary>
        /// JCIC送查日期回傳
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public JcicDateInfo QueryJcicDate(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException(nameof(UserInfo));
            }

            JcicDateResult jcicDateResult = new AdjustSystemDAO().
                QueryJcicDate(customerId, UserInfo.Id, UserInfo.Name);

            if (jcicDateResult == null)
            {
                throw new InvalidOperationException($"{nameof(jcicDateResult)} not found");
            }

            return ConvertJcicDateInfo(jcicDateResult);
        }

        /// <summary>
        /// 轉換JCIC送查日期
        /// </summary>
        /// <param name="jcicDateResult">JCIC送查日期</param>
        /// <returns></returns>
        private JcicDateInfo ConvertJcicDateInfo(JcicDateResult jcicDateResult)
        {
            return (jcicDateResult == null) ? null : new JcicDateInfo()
            {
                JcicQueryDate = jcicDateResult.JcicQueryDate,
                ResponseCode = jcicDateResult.ResponseCode,
            };
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
                throw new ArgumentNullException(nameof(id));
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
                throw new ArgumentNullException(nameof(preAdjustEffect));
            }

            return new PreAdjustEffectResult()
            {
                RejectReason = preAdjustEffect.RejectReason,
                ResponseCode = preAdjustEffect.ResponseCode,
            };
        }
    }
}