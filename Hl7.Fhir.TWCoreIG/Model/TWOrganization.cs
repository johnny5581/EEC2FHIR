using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Model
{
    public static class TWOrganization
    {
        // 衛服部醫療院所代碼CodeSystem
        public static string CodeSystemTwIdentifier = "https://twcore.mohw.gov.tw/ig/twcore/CodeSystem/organization-identifier-tw";

        /// <summary>
        /// 設定台灣衛服部醫療院所代碼
        /// </summary>        
        public static Organization SetTwIdentifier(this Organization organization, string id)
        {
            var identifier = new Identifier(CodeSystemTwIdentifier, id);
            identifier.Type = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v2-0203", "PRN");
            organization.Identifier.Add(identifier);
            return organization;
        }

        public static string GetTwIdentifier(this Organization organization)
        {
            return organization.GetIdentifier(CodeSystemTwIdentifier);
        }
    }
}
