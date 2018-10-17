﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace ThinkPower.CCLPA.DataAccess.DAO.CMPN
{
    /// <summary>
    /// 行銷活動名單檔資料存取類別
    /// </summary>
    public class MarketingCampaignListFileDAO : BaseDAO
    {
        /// <summary>
        /// 取得資料筆數
        /// </summary>
        /// <returns>資料筆數</returns>
        public override int Count()
        {
            int count = 0;

            using (SqlConnection connection = DbConnectionCMPN)
            {
                SqlCommand command = new SqlCommand("SELECT Count(1) FROM CMPN_AVY_CNL_EXBOOK", connection);

                connection.Open();

                count = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return count;
        }

        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <param name="cmpnId">行銷活動編號</param>
        /// <returns>行銷活動名單數量</returns>
        public int GetCampaignListCount(string cmpnId)
        {
            int result = 0;

            if (String.IsNullOrEmpty(cmpnId))
            {
                throw new ArgumentNullException("cmpnId");
            }

            string query = @"
SELECT COUNT(1)
FROM [CMPN_AVY_CNL_EXBOOK]
WHERE CMPN_ID = @CMPN_ID;";

            using (SqlConnection connection = DbConnectionCMPN)
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@CMPN_ID", SqlDbType.VarChar) { Value = cmpnId });

                connection.Open();

                result = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return result;
        }
    }
}