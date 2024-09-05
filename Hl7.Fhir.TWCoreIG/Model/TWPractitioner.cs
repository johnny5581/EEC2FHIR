using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Model
{
    public static class TWPractitioner
    {
        public static string CodeSystemTwIdentifier = "http://www.moi.gov.tw"; // 內政部核發證件CodeSystem

        /// <summary>
        /// 設定國家證件號識別(身分證字號)
        /// </summary>
        public static Practitioner SetTwIdentifier(this Practitioner practitioner, string id)
        {
            var identifier = new Identifier(CodeSystemTwIdentifier, id)
            {
                Use = Identifier.IdentifierUse.Official,
            };

            // 產生Type
            var identifierType = new CodeableConcept();
            // 產生Type的Coding (因為有extension要另外處理)
            var coding = new Coding("http://terminology.hl7.org/CodeSystem/v2-0203", "NNxxx");
            var codeElementExtension = new Extension
            {
                Url = "https://twcore.mohw.gov.tw/ig/twcore/StructureDefinition/identifier-suffix"
            };
            codeElementExtension.Extension.Add(new Extension("suffix", new FhirString("TWN")));
            codeElementExtension.Extension.Add(new Extension("valueSet", new Canonical("http://hl7.org/fhir/ValueSet/iso3166-1-3")));
            coding.CodeElement.Extension.Add(codeElementExtension);
            identifierType.Coding.Add(coding);
            identifier.Type = identifierType;
            practitioner.Identifier.Add(identifier);

            return practitioner;
        }

        /// <summary>
        /// 設定員工代號識別
        /// </summary>
        public static Practitioner SetHospitalIdentifier(this Practitioner practitioner, string codeSystem, string code)
        {
            var identifier = new Identifier(codeSystem, code)
            {
                Use = Model.Identifier.IdentifierUse.Official,
                Type = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v2-0203", "MD")
            };
            practitioner.Identifier.Add(identifier);
            return practitioner;
        }
        /// <summary>
        /// 設定中文姓名
        /// </summary>        
        public static Practitioner SetChineseName(this Practitioner practitioner, string name, IChineseHumanNameConverter converter = null)
        {
            var humanName = (converter ?? ChineseNameTextOnlyConverter.Default).Convert(name);
            humanName.Use = HumanName.NameUse.Official;
            humanName.Text = name;
            practitioner.Name.Add(humanName);
            return practitioner;
        }

        /// <summary>
        /// 設定性別(AdministrativeGenderV3)
        /// <br/>
        /// M: male / F: female / UN: undifferentiated
        /// </summary>
        public static Practitioner SetAdministrativeGenderV3(this Practitioner practitioner, string code)
        {
            switch (code)
            {
                case "M":
                    practitioner.Gender = AdministrativeGender.Male;
                    break;
                case "F":
                    practitioner.Gender = AdministrativeGender.Female;
                    break;
                case "UN":
                    practitioner.Gender = AdministrativeGender.Unknown;
                    break;
            }
            return practitioner;
        }

        public static string GetIdentifier(this Practitioner practitioner, string codeSystem)
        {
            var identifier = practitioner.Identifier.FirstOrDefault(r => r.System == codeSystem);
            return identifier?.Value;
        }
    }
}
