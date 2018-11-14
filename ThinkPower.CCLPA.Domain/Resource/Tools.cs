using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkPower.CCLPA.Domain.Resource
{
    /// <summary>
    /// 工具類別
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// 貨幣格式
        /// </summary>
        public enum NumericFormats
        {
            None = 0,
            Unit,
            Thousand,
            DecimalPointTwoBits
        }

        /// <summary>
        /// 日期時間格式
        /// </summary>
        public enum DateTimeFormats
        {
            None = 0,
            Date
        }

        /// <summary>
        /// 字串格式
        /// </summary>
        public enum Formats
        {
            None = 0,
            Date
        }


        /// <summary>
        /// 貨幣格式化
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="format">格式</param>
        public static string FormatDecimal(decimal? data, NumericFormats format)
        {
            string formatting = null;

            switch (format)
            {
                case NumericFormats.None:
                    formatting = "N";
                    break;
                case NumericFormats.Unit:
                    formatting = "#";
                    break;
                case NumericFormats.Thousand:
                    formatting = "#,#";
                    break;
                case NumericFormats.DecimalPointTwoBits:
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
        public static string FormatDateTime(DateTime? data, DateTimeFormats format)
        {
            string formatting = null;

            switch (format)
            {
                case DateTimeFormats.None:
                    formatting = "0"; // ISO 8601
                    break;
                case DateTimeFormats.Date:
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
        public static string FormatString(string data, Formats format)
        {
            string result = null;

            if (!String.IsNullOrEmpty(data))
            {
                switch (format)
                {
                    case Formats.None:
                        result = data;
                        break;
                    case Formats.Date:
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
