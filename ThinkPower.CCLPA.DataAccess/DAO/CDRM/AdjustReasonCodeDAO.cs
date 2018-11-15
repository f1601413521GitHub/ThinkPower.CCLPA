using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ThinkPower.CCLPA.DataAccess.Condition;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM
{
    /// <summary>
    /// 調整原因代碼檔資料存取類別
    /// </summary>
    public class AdjustReasonCodeDAO : BaseDAO
    {
        /// <summary>
        /// 取出所有調整原因代碼
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdjustReasonCodeDO> GetAll()
        {
            List<AdjustReasonCodeDO> result = null;

            string query = @"
SELECT 
    [CODE],[NAME],[USE_YN]
FROM [COD_RG_REASON];";

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = new List<AdjustReasonCodeDO>();
                    AdjustReasonCodeDO item = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        item = ConvertIncreaseReasonCodeDO(dr);

                        result.Add(item);
                    }
                }

                adapter = null;
                dt = null;
                command = null;
            }

            return result ?? new List<AdjustReasonCodeDO>();
        }

        /// <summary>
        /// 查詢調整原因代碼
        /// </summary>
        /// <param name="condition">調整原因代碼資料查詢條件</param>
        /// <returns></returns>
        public IEnumerable<AdjustReasonCodeDO> Query(AdjustReasonCodeCondition condition)
        {
            List<AdjustReasonCodeDO> adjustReasonCodeList = null;

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }



            StringBuilder sqlCommandBuilder = new StringBuilder(@"
SELECT 
    [CODE],[NAME],[USE_YN]
FROM [COD_RG_REASON]");


            List<string> queryCommand = new List<string>();
            List<string> pagingCommand = new List<string>();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            if (!String.IsNullOrEmpty(condition.UseFlag))
            {
                queryCommand.Add("[USE_YN] = @UseFlag");
                sqlParameters.Add(new SqlParameter("@UseFlag", SqlDbType.NVarChar)
                {
                    Value = condition.UseFlag
                });
            }

            switch (condition.OrderBy)
            {
                case AdjustReasonCodeCondition.OrderByKind.None:
                    pagingCommand.Add("ORDER BY [CODE]");
                    break;
            }

            if ((condition.PageIndex != null && condition.PageIndex >= 1) &&
                (condition.PagingSize != null && condition.PagingSize >= 1))
            {

                pagingCommand.Add("OFFSET     @Skip ROWS");
                sqlParameters.Add(new SqlParameter("@Skip", SqlDbType.Int)
                {
                    Value = ((condition.PageIndex - 1) * condition.PagingSize)
                });

                pagingCommand.Add("FETCH NEXT @Take ROWS ONLY");
                sqlParameters.Add(new SqlParameter("@Take", SqlDbType.Int)
                {
                    Value = condition.PagingSize
                });
            }

            if (queryCommand.Any())
            {
                sqlCommandBuilder.Append(" WHERE ");
                sqlCommandBuilder.Append(String.Join(" AND ", queryCommand));
            }

            if (pagingCommand.Any())
            {
                sqlCommandBuilder.Append(" ");
                sqlCommandBuilder.Append(String.Join(" ", pagingCommand));
            }

            sqlCommandBuilder.Append(";");

            using (SqlConnection connection = DbConnection(Connection.CDRM))
            {
                SqlCommand command = new SqlCommand(sqlCommandBuilder.ToString(), connection);
                command.Parameters.AddRange(sqlParameters.ToArray());

                connection.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    adjustReasonCodeList = new List<AdjustReasonCodeDO>();
                    AdjustReasonCodeDO temp = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        temp = ConvertIncreaseReasonCodeDO(dr);
                        adjustReasonCodeList.Add(temp);
                    }
                }
            }

            return adjustReasonCodeList ?? new List<AdjustReasonCodeDO>();
        }

        /// <summary>
        /// 轉換調整原因代碼
        /// </summary>
        /// <param name="reasonCode">調整原因代碼</param>
        /// <returns></returns>
        private AdjustReasonCodeDO ConvertIncreaseReasonCodeDO(DataRow reasonCode)
        {
            return new AdjustReasonCodeDO()
            {
                Code = reasonCode.Field<string>("CODE"),
                Name = reasonCode.Field<string>("NAME"),
                UseFlag = reasonCode.Field<string>("USE_YN"),
            };
        }
    }
}