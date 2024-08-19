using EEC2FHIR.Utility;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EECViewer
{
    public static class SearchParamsExtension
    {
        public static SearchParams AddDate(this SearchParams searchParam, string name, string value, string text, string outFormat = "yyyy-MM-dd", string inFormat = "yyyyMMdd")
        {
            var formattedValue = DateUtility.Convert(value, outFormat, inFormat);
            if (formattedValue == null)
                throw new Exception($"{text}格式錯誤({inFormat.ToUpper()})");
            return searchParam.Add(name, formattedValue);
        }
        public static SearchParams AddDateRange(this SearchParams searchParam, string name, string from, string to, string text, string outFormat = "yyyy-MM-dd", string inFormat = "yyyyMMdd")
        {
            if (from != null)
            {
                var formattedValue = DateUtility.Convert(from, outFormat, inFormat);
                if (formattedValue == null)
                    throw new Exception($"{text}格式錯誤({inFormat.ToUpper()})");
                searchParam.Add(name, "gt" + formattedValue);
            }

            if (to != null)
            {
                var formattedValue = DateUtility.Convert(to, outFormat, inFormat);
                if (formattedValue == null)
                    throw new Exception($"{text}格式錯誤({inFormat.ToUpper()})");
                searchParam.Add(name, "lt" + formattedValue);
            }

            return searchParam;
        }
    }
}
