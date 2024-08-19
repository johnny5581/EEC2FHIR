using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirConn.Utility
{
    public class TransactionHistory
    {
        public TransactionHistory(string token)
        {
            Token = token;
        }

        public string Token { get; }
        public DateTime Time { get; set; } = DateTime.Now;
        public RequestHistoryCollection Requests { get; set; } = new RequestHistoryCollection();

        public override string ToString()
        {
            return $"{Time:HH:mm:ss} - {Token}";
        }
    }
    public class RequestHistory
    {
        public RequestHistory(string guid, string method, string url)
        {
            Guid = guid;
            Method = method;
            Url = url;
        }
        public string Guid { get; }
        public string Method { get; }
        public string Url { get; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Time { get; } = DateTime.Now;
        public override string ToString()
        {
            return $"{Time:HH:mm:ss} - {Method} - {Url}";
        }
    }
    public class RequestHistoryCollection : KeyedCollection<string, RequestHistory>
    {
        protected override string GetKeyForItem(RequestHistory item)
        {
            return item.Guid;
        }
    }
}
