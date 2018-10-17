﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 行銷活動匯入紀錄檔資料存取類別
    /// </summary>
    public class MarketingActivitiesRecordFileDAO : BaseDAO
    {
        /// <summary>
        /// 取得資料筆數
        /// </summary>
        /// <returns>資料筆數</returns>
        public override int Count()
        {
            int count = 0;

            using (SqlConnection connection = DbConnectionCDRM)
            {
                SqlCommand command = new SqlCommand("SELECT Count(1) FROM LOG_RG_ILRC", connection);

                connection.Open();

                count = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return count;
        }
    }
}
