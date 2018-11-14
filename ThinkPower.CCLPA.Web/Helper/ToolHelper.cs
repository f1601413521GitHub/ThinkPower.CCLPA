using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.Web.Helper
{
    /// <summary>
    /// 工具類別
    /// </summary>
    public class ToolHelper
    {
        /// <summary>
        /// 貨幣格式
        /// </summary>
        public enum NumericFormat
        {
            None = 0,
            Unit,
            Thousand,
            DecimalPointTwoBit
        }

        /// <summary>
        /// 日期時間格式
        /// </summary>
        public enum DateTimeFormat
        {
            None = 0,
            Date
        }

        /// <summary>
        /// 字串格式
        /// </summary>
        public enum Format
        {
            None = 0,
            Date
        }


        /// <summary>
        /// 貨幣格式化
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="format">格式</param>
        public static string FormatDecimal(decimal? data, NumericFormat format)
        {
            string formatting = null;

            switch (format)
            {
                case NumericFormat.None:
                    formatting = "N";
                    break;
                case NumericFormat.Unit:
                    formatting = "#";
                    break;
                case NumericFormat.Thousand:
                    formatting = "#,#";
                    break;
                case NumericFormat.DecimalPointTwoBit:
                    formatting = "#.##";
                    break;
            }

            return (data == null) ? null : data.Value.ToString(formatting);
        }


        /// <summary>
        /// 日期時間格式化
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="format">格式</param>
        public static string FormatDateTime(DateTime? data, DateTimeFormat format)
        {
            string formatting = null;

            switch (format)
            {
                case DateTimeFormat.None:
                    formatting = "0"; // ISO 8601
                    break;
                case DateTimeFormat.Date:
                    formatting = "yyyy/MM/dd";
                    break;
            }

            return (data == null) ? null : data.Value.ToString(formatting);
        }


        /// <summary>
        /// 字串格式化
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string FormatString(string data, Format format)
        {
            string result = null;

            if (!String.IsNullOrEmpty(data))
            {
                switch (format)
                {
                    case Format.None:
                        result = data;
                        break;
                    case Format.Date:
                        if (data.Length == 8)
                        {
                            result = $"{data.Substring(0, 4)}/{data.Substring(4, 2)}/{data.Substring(6, 2)}";
                        }
                        break;
                }
            }

            return result;
        }
    }
}
