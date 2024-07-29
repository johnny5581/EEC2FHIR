using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Rest
{
    /// <summary>
    /// 符合TWCoreIG的病人查詢類別
    /// </summary>
    public class TWPatientQuerier : FhirResourceQuerier<Patient>
    {
        public TWPatientQuerier(FhirClient client) : base(client)
        {
        }

        /// <summary>
        /// 透過內政部身分證字號查詢
        /// </summary>        
        public Patient[] SearchByTwIdentifier(string id)
        {
            return SearchByIdentifier(TWPatient.CodeSystemTwIdentifier, id);
        }
        /// <summary>
        /// 透過內政部身分證字號取得病人
        /// </summary>  
        public Patient GetByTwIdentifier(string id)
        {
            return GetByIdentifier(TWPatient.CodeSystemTwIdentifier, id);
        }
    }
}
