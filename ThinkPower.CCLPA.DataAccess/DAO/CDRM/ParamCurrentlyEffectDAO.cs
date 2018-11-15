﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 專案參數目前生效主檔資料存取類別
    /// </summary>
    public class ParamCurrentlyEffectDAO : BaseDAO
    {
        /// <summary>
        /// 取得目前生效主檔資訊
        /// </summary>
        /// <param name="reasonCode">臨調原因代碼</param>
        /// <returns></returns>
        public ParamCurrentlyEffectDO Get(string reasonCode)
        {
            ParamCurrentlyEffectDO result = null;

            if (String.IsNullOrEmpty(reasonCode))
            {
                throw new ArgumentNullException(nameof(reasonCode));
            }


            string query = @"
SELECT 
    [RG_REASON],[EFFECT_DT],[ADJUST_DATE_S],[ADJUST_DATE_E],[APPROVE_AMT_MAX],
    [REMARK],[RG_REJECT],[APPROVE_SCALE_MAX]
FROM [RG_PARA_M_ACTIVE]
WHERE RG_REASON = @ReasonCode;";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@ReasonCode", SqlDbType.NVarChar)
                {
                    Value = reasonCode
                });

                connection.Open();
                command.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 1)
                {
                    throw new InvalidOperationException("CurrentlyEffectData not the only");
                }
                else if (dt.Rows.Count == 1)
                {
                    result = ConvertParamCurrentlyEffectDO(dt.Rows[0]);
                }
            }

            return result;
        }

        /// <summary>
        /// 轉換專案參數目前生效資訊
        /// </summary>
        /// <param name="effectInfo">參數目前生效資訊</param>
        /// <returns></returns>
        private ParamCurrentlyEffectDO ConvertParamCurrentlyEffectDO(DataRow effectInfo)
        {
            return new ParamCurrentlyEffectDO()
            {
                Reason = effectInfo.Field<string>("RG_REASON"),
                EffectDate = effectInfo.Field<string>("EFFECT_DT"),
                AdjustDateStart = effectInfo.Field<string>("ADJUST_DATE_S"),
                AdjustDateEnd = effectInfo.Field<string>("ADJUST_DATE_E"),
                ApproveAmountMax = effectInfo.Field<decimal?>("APPROVE_AMT_MAX"),
                Remark = effectInfo.Field<string>("REMARK"),
                VerifiyCondition = effectInfo.Field<string>("RG_REJECT"),
                ApproveScaleMax = effectInfo.Field<decimal?>("APPROVE_SCALE_MAX"),
            };
        }
    }
}