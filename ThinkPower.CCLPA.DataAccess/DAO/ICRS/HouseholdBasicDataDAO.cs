using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.DataAccess.DAO.ICRS
{
    /// <summary>
    /// 歸戶基本資料資料存取類別
    /// </summary>
    public class HouseholdBasicDataDAO : BaseDAO
    {
        /// <summary>
        /// 取得資料筆數
        /// </summary>
        /// <returns>資料筆數</returns>
        public override int Count()
        {
            int count = 0;

            using (SqlConnection connection = DbConnectionICRS)
            {
                SqlCommand command = new SqlCommand("SELECT Count(1) FROM RG_ID", connection);

                connection.Open();

                count = Convert.ToInt32(command.ExecuteScalar());

                command = null;
            }

            return count;
        }
    }
}
