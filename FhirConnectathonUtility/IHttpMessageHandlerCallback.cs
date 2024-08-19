using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirConn.Utility
{
    public interface IHttpMessageHandlerCallback
    {
        void SaveRequest(string guid, string method, string uri, string requestText);
        void SaveResponse(string guid, string responseText);
    }
}
