using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Rest
{
    /// <summary>
    /// 符合TWCoreIG的醫護人員查詢類別
    /// </summary>
    public class TWPractitionerQuerier : FhirResourceQuerier<Practitioner>
    {
        public TWPractitionerQuerier(FhirClient client) : base(client)
        {
        }

        /// <summary>
        /// 透過身分證號查詢
        /// </summary>
        public Practitioner[] SearchByTwIdentifier(string id)
        {
            return SearchByIdentifier(TWPractitioner.CodeSystemTwIdentifier, id);
        }

        /// <summary>
        /// 透過身分證號取得醫護人員
        /// </summary>
        public Practitioner GetByTwIdentifier(string id)
        {
            return GetByIdentifier(TWPractitioner.CodeSystemTwIdentifier, id);
        }
    }
}
