using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEC2FHIR.Utility
{
    public static class DateUtility
    {
        /// <summary>
        /// 轉換日期字串
        /// </summary>        
        public static string Convert(string dateStr, string outFormat = "yyyy-MM-dd", string inFormat = "yyyyMMdd")
        {
            var datetime = DateTime.ParseExact(dateStr, inFormat, null);
            if (outFormat == "ISO")
                return new DateTimeOffset(datetime, TimeSpan.FromHours(8)).ToString("o", CultureInfo.InvariantCulture);
            return datetime.ToString(outFormat);
        }
    }
}
