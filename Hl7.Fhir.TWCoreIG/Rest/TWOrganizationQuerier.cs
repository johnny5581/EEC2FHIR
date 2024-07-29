using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Rest
{
    /// <summary>
    /// 符合TWCoreIG的組織(醫院)查詢類別
    /// </summary>
    public class TWOrganizationQuerier : FhirResourceQuerier<Organization>
    {
        public TWOrganizationQuerier(FhirClient client) : base(client)
        {
        }

        /// <summary>
        /// 透過台灣衛服部院所代碼查詢
        /// </summary>        
        public Organization[] SearchByTwIdentifier(string id)
        {
            return SearchByIdentifier(TWOrganization.CodeSystemTwIdentifier, id);
        }
        /// <summary>
        /// 透過台灣衛服部院所代碼取得組織(醫院)
        /// </summary>        
        public Organization GetByTwIdentifier(string id)
        {
            return GetByIdentifier(TWOrganization.CodeSystemTwIdentifier, id);
        }
    }
}
