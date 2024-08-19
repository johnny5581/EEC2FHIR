using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Model
{
    /// <summary>
    /// TW Core IG - Patient擴充
    /// 根據CoreIG的範例(https://build.fhir.org/ig/cctwFHIRterm/MOHW_TWCoreIG_Build/Patient-pat-example.html)產生
    /// </summary>
    public static class TWPatient
    {
        public static string CodeSystemTwIdentifier = "http://www.moi.gov.tw/"; // 內政部核發證件CodeSystem

        /// <summary>
        /// 設定國家證件號識別(身分證字號)
        /// </summary>
        public static Patient SetTwIdentifier(this Patient patient, string id)
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

            patient.Identifier.Add(identifier);

            return patient;
        }

        /// <summary>
        /// 設定病歷號識別
        /// </summary>
        public static Patient SetMedicalRecordNumber(this Patient patient, string codeSystem, string id)
        {
            var identifier = new Identifier(codeSystem, id)
            {
                Use = Model.Identifier.IdentifierUse.Official,
                Type = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v2-0203", "MR")
            };
            patient.Identifier.Add(identifier);
            return patient;
        }
        /// <summary>
        /// 設定中文姓名
        /// </summary>        
        public static Patient SetChineseName(this Patient patient, string name, IChineseHumanNameConverter converter = null)
        {
            var humanName = (converter ?? ChineseNameByLengthConverter.Default).Convert(name);
            humanName.Use = HumanName.NameUse.Official;
            humanName.Text = name;
            patient.Name.Add(humanName);
            return patient;
        }

        /// <summary>
        /// 設定性別(AdministrativeGenderV3)
        /// <br/>
        /// M: male / F: female / UN: undifferentiated
        /// </summary>
        public static Patient SetAdministrativeGenderV3(this Patient patient, string code)
        {
            switch(code)
            {
                case "M":
                    patient.Gender = AdministrativeGender.Male;
                    break;
                case "F":
                    patient.Gender = AdministrativeGender.Female;
                    break;
                case "UN":
                    patient.Gender = AdministrativeGender.Unknown;
                    break;
            }
            return patient;
        }

        public static string GetTwIdentifier(this Patient patient)
        {
            return patient.GetIdentifier(CodeSystemTwIdentifier);
        }
    }

    /// <summary>
    /// 中文姓名轉換介面
    /// </summary>
    public interface IChineseHumanNameConverter
    {
        /// <summary>
        /// 轉換
        /// </summary>

        HumanName Convert(string name);
    }


    public class ChineseNameByLengthConverter : IChineseHumanNameConverter
    {
        public static ChineseNameByLengthConverter Default { get; } = new ChineseNameByLengthConverter();

        public HumanName Convert(string name)
        {

            if (name.Length == 2 || name.Length == 3)
            {
                // 二字姓名或三字姓名
                return new HumanName
                {
                    Family = name.Substring(0, 1),
                    Given = new string[] { name.Substring(1) }
                };
            }
            else if (name.Length > 3)
            {
                // 四字以上姓名，抓前兩個字當姓，其餘為名
                return new HumanName
                {
                    Family = name.Substring(0, 2),
                    Given = new string[] { name.Substring(2) }
                };
            }
            else
                throw new NotSupportedException("不支援的姓名格式");
        }
    }
}
