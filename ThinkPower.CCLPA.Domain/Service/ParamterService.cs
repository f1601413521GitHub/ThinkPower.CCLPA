using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ThinkPower.CCLPA.DataAccess.Condition;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.Domain.Condition;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 專案參數檔服務
    /// </summary>
    public class ParamterService
    {
        #region PublicMethod

        /// <summary>
        /// 取得調整原因代碼
        /// </summary>
        public IEnumerable<AdjustReasonCode> GetAdjustReasonCode()
        {

            IEnumerable<AdjustReasonCodeDO> adjustReasonCodeList = new AdjustReasonCodeDAO().
                Query(new AdjustReasonCodeCondition()
                {
                    UseFlag = "Y",
                    OrderBy = AdjustReasonCodeCondition.OrderByKind.None,
                });

            if (!adjustReasonCodeList.Any())
            {
                throw new InvalidOperationException($"{nameof(adjustReasonCodeList)} not found");
            }

            return ConvertAdjustReasonCode(adjustReasonCodeList);
        }

        /// <summary>
        /// 取得參數生效資料
        /// </summary>
        /// <param name="adjustReasonCodeList">調整原因代碼</param>
        /// <returns></returns>
        public IEnumerable<ParamCurrentlyEffect> GetParamEffect(IEnumerable<string> adjustReasonCodeList)
        {
            List<ParamCurrentlyEffectDO> effectList = null;

            if ((adjustReasonCodeList == null) || !adjustReasonCodeList.Any())
            {
                throw new ArgumentNullException(nameof(adjustReasonCodeList));
            }

            effectList = new List<ParamCurrentlyEffectDO>();

            ParamCurrentlyEffectDAO effectDAO = new ParamCurrentlyEffectDAO();
            ParamCurrentlyEffectDO tempEffect = null;

            DateTime currentDate = DateTime.Today;
            DateTime tempStartTime;
            DateTime tempEndTime;


            foreach (string adjustReasonCode in adjustReasonCodeList)
            {
                tempEffect = effectDAO.Get(adjustReasonCode);

                if (tempEffect == null)
                {
                    continue;
                }
                else if (!DateTime.TryParseExact(tempEffect.AdjustDateStart, "yyyy/MM/dd", null,
                        DateTimeStyles.None, out tempStartTime))
                {
                    throw new InvalidOperationException("Convert AdjustDateStart fail");
                }
                else if (!DateTime.TryParseExact(tempEffect.AdjustDateEnd, "yyyy/MM/dd", null,
                        DateTimeStyles.None, out tempEndTime))
                {
                    throw new InvalidOperationException("Convert AdjustDateEnd fail");
                }
                else if ((currentDate >= tempStartTime) &&
                         (currentDate <= tempEndTime))
                {
                    effectList.Add(tempEffect);
                }
            }

            return ConvertParamCurrentlyEffect(effectList);
        }

        /// <summary>
        /// 取得目前生效的調整原因
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdjustReason> GetEffectiveAdjustReason()
        {
            IEnumerable<AdjustReasonCode> adjustReasonCodeList = GetAdjustReasonCode();

            if ((adjustReasonCodeList == null) || !adjustReasonCodeList.Any())
            {
                throw new InvalidOperationException($"{nameof(adjustReasonCodeList)} not found");
            }


            IEnumerable<ParamCurrentlyEffect> paramCurrentlyEffectList =
                GetParamEffect(adjustReasonCodeList.Select(x => x.Code));

            if ((paramCurrentlyEffectList == null) || !paramCurrentlyEffectList.Any())
            {
                throw new InvalidOperationException($"{nameof(paramCurrentlyEffectList)} not found");
            }


            List<AdjustReason> adjustReasonList = new List<AdjustReason>();

            foreach (ParamCurrentlyEffect effectReason in paramCurrentlyEffectList)
            {
                adjustReasonList.Add(new AdjustReason() {
                    ReasonCode = adjustReasonCodeList.Where(x => x.Code == effectReason.Reason).First(),
                    ReasonEffectInfo = effectReason,
                });
            }

            return adjustReasonList;
        }

        #endregion



        #region PrivateMethod

        /// <summary>
        /// 轉換參數生效資料
        /// </summary>
        /// <param name="currentlyEffectList">參數生效資料</param>
        /// <returns></returns>
        private IEnumerable<ParamCurrentlyEffect> ConvertParamCurrentlyEffect(
            IEnumerable<ParamCurrentlyEffectDO> currentlyEffectList)
        {
            return currentlyEffectList?.Select(x => new ParamCurrentlyEffect()
            {
                Reason = x.Reason,
                VerifiyCondition = x.VerifiyCondition,
                ApproveScaleMax = x.ApproveScaleMax,
                Remark = x.Remark,
                ApproveAmountMax = x.ApproveAmountMax,
                AdjustDateStart = x.AdjustDateStart,
                AdjustDateEnd = x.AdjustDateEnd,
                EffectDate = x.EffectDate,
            });
        }

        /// <summary>
        /// 轉換調整原因
        /// </summary>
        /// <param name="adjustReasonList">調整原因</param>
        /// <returns></returns>
        private IEnumerable<AdjustReasonCode> ConvertAdjustReasonCode(IEnumerable<AdjustReasonCodeDO> adjustReasonList)
        {
            return adjustReasonList?.Select(x => new AdjustReasonCode()
            {
                Code = x.Code,
                Name = x.Name,
                UseFlag = x.UseFlag
            });
        }

        #endregion
    }
}