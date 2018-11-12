using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 專案參數檔服務
    /// </summary>
    public class ParamterService
    {
        /// <summary>
        /// 取得調整原因代碼
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdjustReasonCodeInfo> GetActiveAdjustReasonCode()
        {
            IEnumerable<AdjustReasonCodeDO> adjustReasonList = new AdjustReasonCodeDAO().GetAll().
                Where(x => x.UseFlag == "Y");

            if (!adjustReasonList.Any())
            {
                throw new InvalidOperationException($"{nameof(adjustReasonList)} not found");
            }

            return ConvertAdjustReasonCodeInfo(adjustReasonList);
        }

        /// <summary>
        /// 取得目前生效主檔資料
        /// </summary>
        /// <param name="adjustReasonCodeList">調整原因代碼</param>
        /// <returns></returns>
        public IEnumerable<ParamCurrentlyEffectInfo> GetParamEffectData(
            IEnumerable<string> adjustReasonCodeList)
        {
            List<ParamCurrentlyEffectDO> result = null;

            if (adjustReasonCodeList == null || !adjustReasonCodeList.Any())
            {
                throw new ArgumentNullException(nameof(adjustReasonCodeList));
            }

            result = new List<ParamCurrentlyEffectDO>();

            ParamCurrentlyEffectDAO paramEffectDAO = new ParamCurrentlyEffectDAO();
            ParamCurrentlyEffectDO temp = null;

            DateTime currentTime = DateTime.Today;
            DateTime tempStartTime;
            DateTime tempEndTime;


            foreach (string adjustReasonCode in adjustReasonCodeList)
            {
                temp = paramEffectDAO.Get(adjustReasonCode);

                if (temp == null)
                {
                    continue;
                }
                else if (!DateTime.TryParseExact(temp.AdjustDateStart, "yyyy/MM/dd", null,
                        DateTimeStyles.None, out tempStartTime))
                {
                    throw new InvalidOperationException("Convert AdjustDateStart fail");
                }
                else if (!DateTime.TryParseExact(temp.AdjustDateEnd, "yyyy/MM/dd", null,
                        DateTimeStyles.None, out tempEndTime))
                {
                    throw new InvalidOperationException("Convert AdjustDateEnd fail");
                }
                else if ((currentTime >= tempStartTime) &&
                         (currentTime <= tempEndTime))
                {
                    result.Add(temp);
                }
            }


            return ConvertParamCurrentlyEffectInfo(result);
        }

        /// <summary>
        /// 轉換參數生效資料
        /// </summary>
        /// <param name="paramEffectList">參數生效資料</param>
        /// <returns></returns>
        private IEnumerable<ParamCurrentlyEffectInfo> ConvertParamCurrentlyEffectInfo(
            IEnumerable<ParamCurrentlyEffectDO> paramEffectList)
        {
            return (paramEffectList == null) ? null : paramEffectList.Select(x =>
                new ParamCurrentlyEffectInfo()
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
        private IEnumerable<AdjustReasonCodeInfo> ConvertAdjustReasonCodeInfo(
            IEnumerable<AdjustReasonCodeDO> adjustReasonList)
        {
            return (adjustReasonList == null) ? null :
                adjustReasonList.Select(x => new AdjustReasonCodeInfo()
                {
                    Code = x.Code,
                    Name = x.Name,
                    UseFlag = x.UseFlag
                });
        }
    }
}