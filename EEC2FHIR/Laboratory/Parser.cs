using EEC2FHIR.Utility;
using Hl7.Fhir.ElementModel.Types;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace EEC2FHIR.Laboratory
{
    public class Parser
    {
        private readonly FhirClient client;
        private XmlNamespaceManager xmlNsMgr;

        public Parser(FhirClient client)
        {
            this.client = client;
        }

        public string SystemCodeGlobal { get; set; } = "https://www.cgmh.org.tw";
        public string SystemCodeLocal { get; set; } = "https://lnk.cgmh.org.tw";
        public static string SystemCodeLoinc { get; } = "http://loinc.org";
        public static string SystemCodeSnomed { get; } = "http://snomed.info/sct";

        /**
         * 長庚醫療財團法人林口長庚紀念醫院 	
         * OID:  2.16.886.104.100565.100008 
         * DN:   ou=林口長庚紀念醫院,o=長庚醫療財團法人,c=tw
         * CODE: 1132070011, root=2.16.886.101.20003.20014
         **/
        public Bundle Parse(XmlDocument xml)
        {
            /** 
             * ref: https://silcoet.ntunhs.edu.tw/Healthycloud/ICvsmodel.html
             * 
             * Bundle要件:
             *   Composition  整份摘要
             *   Patient      病人
             *   Organization 醫院
             *   Practitioner 醫事人員(開立檢驗醫矚單張醫師)
             *   Encounter    就診資料
             *   Observation  檢驗資料
             *   Specimem     檢體來源
             *   
             **/

            // 摘要
            var composition = new Composition();
            // 取得文件本體            
            xmlNsMgr = new XmlNamespaceManager(xml.NameTable);
            xmlNsMgr.AddNamespace("", "urn:hl7-org:v3");
            xmlNsMgr.AddNamespace("ns", "urn:hl7-org:v3");
            xmlNsMgr.AddNamespace("cdp", "http://www.hl7.org.tw/EMR/CDocumentPayload/v1.0");
            xmlNsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            xmlNsMgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.4.1#");

            var clinicalDoc = xml.SelectSingleNode("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument", xmlNsMgr);

            // 製作病人
            var patient = GetPatitentParticipant(clinicalDoc.SelectSingleNode("ns:recordTarget", xmlNsMgr));
            composition.Subject = patient.GetReference();
            var organization = GetOrganizationParticipant(clinicalDoc.SelectSingleNode("ns:custodian/ns:assignedCustodian/ns:representedCustodianOrganization", xmlNsMgr));
            composition.Custodian = organization.GetReference();
            var author = GetAuthorParticipant(clinicalDoc.SelectSingleNode("ns:author", xmlNsMgr));
            composition.Author.Add(author.GetReference());
            var encounter = GetEncounterParticipant(clinicalDoc, composition);
            composition.Encounter = encounter.GetReference();

            var componentRoot = clinicalDoc.SelectSingleNode("ns:component/ns:structuredBody/ns:component/ns:section", xmlNsMgr);
            var organizers = componentRoot.SelectNodes("ns:entry/ns:organizer", xmlNsMgr);
            var observations = new List<Observation>();
            var specimens = new List<Specimen>();
            foreach (XmlNode organizer in organizers)
            {
                var specimen = GetSpecimenParticipant(organizer.SelectSingleNode("ns:specimen", xmlNsMgr), composition);
                var observation = GetObservationParticipant(organizer, clinicalDoc, specimen, composition);

                var sectionComponent = new Composition.SectionComponent();
                sectionComponent.Code = new CodeableConcept(SystemCodeLoinc, "30954-2", "Relevant diagnostic tests and/or laboratory data");
                sectionComponent.Entry.Add(observation.GetReference());
                sectionComponent.Entry.Add(specimen.GetReference());

                observations.Add(observation);
                specimens.Add(specimen);
            }


            // 組裝摘要
            composition.Title = clinicalDoc.SelectSingleNode("ns:title", xmlNsMgr).InnerText;
            composition.Date = DateUtility.Convert(clinicalDoc.SelectSingleNode("ns:effectiveTime", xmlNsMgr).GetAttributeValue("value"), outFormat: "ISO", inFormat: "yyyyMMddHHmmss");
            composition.Status = CompositionStatus.Final;
            composition.Type = new CodeableConcept(SystemCodeLoinc, "11503-0", "檢驗檢查");
            composition = client.Create(composition);


            // 組合bundle
            var bundle = new Bundle();
            bundle.Type = Bundle.BundleType.Document;
            bundle.Identifier = new Identifier("https://twcore.mohw.gov.tw/ig/index.html", "Bundle-EMR");
            bundle.Timestamp = DateTimeOffset.Now;

            bundle.AppendEntryResource(composition);
            bundle.AppendEntryResource(organization);
            bundle.AppendEntryResource(patient);
            foreach (var obs in observations)
                bundle.AppendEntryResource(obs);
            foreach (var spe in specimens)
                bundle.AppendEntryResource(spe);
            bundle.AppendEntryResource(author);
            bundle.AppendEntryResource(encounter);

            return bundle;
        }



        private Patient GetPatitentParticipant(XmlNode node)
        {
            Patient patient = null;

            // 身分證號
            var idNo = node.SelectSingleNode("ns:patientRole/ns:patient/ns:id", xmlNsMgr).GetAttributeValue("extension");
            // 醫院病歷號
            var chtNo = node.SelectSingleNode("ns:patientRole/ns:id", xmlNsMgr).GetAttributeValue("extension");


            // 先查詢看看目前有沒有這個病人，有的話使用現有的病人資料
            var querier = new TWPatientQuerier(client);
            var pat = querier.GetByIdentifier(SystemCodeLocal, chtNo);

            // 如果找不到病人，嘗試使用身分證號取得病人資料，有資料的話就更新他的病歷號
            if (pat == null)
            {
                pat = querier.GetByTwIdentifier(idNo);
                if (pat != null)
                {
                    pat.SetMedicalRecordNumber(SystemCodeLocal, chtNo);
                    client.Update(pat);
                }
            }

            // 如果病人有找到，使用這個病人
            if (pat != null)
            {
                return pat;
            }

            // 如果病人沒有找到，建立新的病人
            patient = new Patient();

            patient.SetTwIdentifier(idNo);
            patient.SetMedicalRecordNumber(SystemCodeLocal, chtNo);

            // 中文姓名
            var cnm = node.SelectSingleNode("ns:patientRole/ns:patient/ns:name", xmlNsMgr).InnerText;
            patient.SetChineseName(cnm);

            // 性別
            var genderCode = node.SelectSingleNode("ns:patientRole/ns:patient/ns:administrativeGenderCode", xmlNsMgr).GetAttributeValue("code");
            // var genderSystem = "http://terminology.hl7.org/CodeSystem/v3-AdministrativeGender"; // 固定使用的系統(來自CDC 2.16.840.1.113883.5.1)            
            patient.SetAdministrativeGenderV3(genderCode);

            // 生日
            var birthDate = node.SelectSingleNode("ns:patientRole/ns:patient/ns:birthTime", xmlNsMgr).GetAttributeValue("value");
            patient.BirthDate = DateUtility.Convert(birthDate);

            // 管理組織            
            patient.ManagingOrganization = GetOrganizationParticipant(node.SelectSingleNode("ns:patientRole/ns:providerOrganization", xmlNsMgr)).GetReference();


            patient = client.Create(patient);

            return patient;
        }

        private Organization GetOrganizationParticipant(XmlNode node)
        {
            Organization organization = null;

            var hospId = node.SelectSingleNode("ns:id", xmlNsMgr).GetAttributeValue("extension");

            // 先查看看有沒有這個醫療院所代碼，有的話就使用這個代碼
            var querier = new TWOrganizationQuerier(client);
            organization = querier.GetByTwIdentifier(hospId);
            if (organization != null)
            {
                return organization;
            }

            organization = new Organization();
            organization.SetTwIdentifier(hospId);

            var hospName = node.SelectSingleNode("ns:name", xmlNsMgr).InnerText;
            organization.Name = hospName;

            organization = client.Create(organization);

            return organization;
        }

        private Practitioner GetAuthorParticipant(XmlNode node)
        {
            Practitioner practitioner = null;

            var empId = node.SelectSingleNode("ns:assignedAuthor/ns:id", xmlNsMgr).GetAttributeValue("extension");

            // 先查看看有沒有這個醫事人員代碼，有的話就使用這個代碼
            var querier = new TWPractitionerQuerier(client);
            practitioner = querier.GetByIdentifier(SystemCodeGlobal, empId);
            if (practitioner != null)
            {
                return practitioner;
            }

            practitioner = new Practitioner();
            practitioner.SetHospitalIdentifier(SystemCodeGlobal, empId);

            // 中文姓名
            var cnm = node.SelectSingleNode("ns:assignedAuthor/ns:assignedPerson/ns:name", xmlNsMgr).InnerText;
            practitioner.SetChineseName(cnm);

            practitioner = client.Create(practitioner);

            return practitioner;
        }

        private Encounter GetEncounterParticipant(XmlNode node, Composition composition)
        {
            Encounter encounter = null;

            var opdNo = node.SelectSingleNode("ns:id", xmlNsMgr).GetAttributeValue("extension");

            // 先查看看有沒有這張就診單，有的話就使用這張單
            var querier = new FhirResourceQuerier<Encounter>(client);
            encounter = querier.GetByIdentifier(SystemCodeLocal, opdNo);
            if (encounter != null)
            {
                return encounter;
            }

            // TODO: 資料轉換來源不清
            encounter = new Encounter();
            encounter.Identifier.Add(new Identifier(SystemCodeLocal, opdNo));
            encounter.Class = new Coding("http://terminology.hl7.org/CodeSystem/v3-ActCode", "OBSENC", "observation encounter");
            encounter.ServiceType = new CodeableConcept(SystemCodeSnomed, "394609007", "General surgery", "Medical Services");
            encounter.Status = Encounter.EncounterStatus.Finished;
            encounter.Subject = composition.Subject;
            encounter.Participant.Add(new Encounter.ParticipantComponent { Individual = composition.Author.FirstOrDefault() });

            // 時間
            var time = node.SelectSingleNode("ns:author/ns:time", xmlNsMgr).GetAttributeValue("value");
            encounter.Period = new Period { Start = DateUtility.Convert(time, inFormat: "yyyyMMddHHmm", outFormat: "yyyy-MM-dd") };

            encounter = client.Create(encounter);

            return encounter;
        }

        /// <summary>
        /// 取得採檢資訊
        /// </summary>        
        private Specimen GetSpecimenParticipant(XmlNode node, Composition composition)
        {
            Specimen specimen = null;

            // 採檢資訊每次都產生新的
            var spicimenPlayingEntityCodeNode = node.SelectSingleNode("ns:specimenRole/ns:specimenPlayingEntity/ns:code", xmlNsMgr);
            var codeSystem = spicimenPlayingEntityCodeNode.GetAttributeValue("codeSystem");
            var code = spicimenPlayingEntityCodeNode.GetAttributeValue("code");
            // TODO: 轉譯 'Blood' => ' BLD'
            if (code == "Blood")
                code = " BLD";

            // 建立採檢資訊
            specimen = new Specimen();
            specimen.Type = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v3-SpecimenType", code);
            specimen.Subject = composition.Subject;


            // TODO: 檢體來源，目前無法對應
            // 因此依照(view-source:https://emr.mohw.gov.tw/FHIRemr/WebSite1/XML_Resources(xml)_/Specimen_BloodExamination128223.xml)
            // method 使用固定值4703008
            // bodysite 使用固定值368234003
            specimen.Collection = new Specimen.CollectionComponent();
            specimen.Collection.Method = new CodeableConcept(SystemCodeSnomed, "4703008", "Cardinal vein structure", null);
            specimen.Collection.BodySite = new CodeableConcept(SystemCodeSnomed, "368234003", "Posterior carpal region");

            specimen = client.Create(specimen);

            return specimen;
        }
        private Observation GetObservationParticipant(XmlNode node, XmlNode root, Specimen specimen, Composition composition)
        {
            Observation observation = null;

            // 檢驗每次都產生新的
            observation = new Observation();

            observation.Status = ObservationStatus.Final;

            // 設定檢驗資訊
            var id = root.SelectSingleNode("ns:id", xmlNsMgr).GetAttributeValue("extension");
            observation.Identifier.Add(new Identifier(SystemCodeLocal, id));

            // 設定observation.Code
            var codeNode = node.SelectSingleNode("ns:code", xmlNsMgr);
            var obsLoincCode = codeNode.GetAttributeValue("code");
            var obsLoincDisplayName = codeNode.GetAttributeValue("displayName");
            var codeTranslationNode = codeNode.SelectSingleNode("ns:translation", xmlNsMgr);
            var obsNhiCode = codeTranslationNode.GetAttributeValue("code");
            var obsNhiDisplayName = codeTranslationNode.GetAttributeValue("displayName");
            // 使用loinc碼
            observation.Code = new CodeableConcept("http://loinc.org", obsLoincCode, obsLoincDisplayName, obsNhiDisplayName);


            observation.Subject = composition.Subject;
            observation.Encounter = composition.Encounter;
            // TODO: 使用composition作者
            observation.Performer.AddRange(composition.Author);
            observation.Specimen = specimen.GetReference();


            // 採檢時間
            var specimenTime = DateUtility.Convert(root.SelectSingleNode("ns:componentOf/ns:encompassingEncounter/ns:effectiveTime", xmlNsMgr).GetAttributeValue("value"), inFormat: "yyyyMMddHHmm");
            // 收件時間
            var receiveTime = DateUtility.Convert(node.SelectSingleNode("ns:effectiveTime", xmlNsMgr).GetAttributeValue("value"), inFormat: "yyyyMMddHHmm");
            observation.Effective = new Period(new FhirDateTime(specimenTime), new FhirDateTime(receiveTime));

            // 項目與結果
            var interpretation = new CodeableConcept(SystemCodeLoinc, "30954-2", "Relevant diagnostic tests and/or laboratory data", null);
            observation.Interpretation.Add(interpretation);

            // 備註            
            observation.Note.Add(new Annotation { Text = new Markdown("無") });

            // 檢體類別
            observation.BodySite = specimen.Collection.BodySite;

            var componentNodes = node.SelectNodes("ns:component", xmlNsMgr);
            foreach (XmlNode componentNode in componentNodes)
            {
                var component = new Observation.ComponentComponent();

                var obsNode = componentNode.SelectSingleNode("ns:observation", xmlNsMgr);
                var loincCode = obsNode.SelectSingleNode("ns:code", xmlNsMgr).GetAttributeValue("code");
                var loincDisplayName = obsNode.SelectSingleNode("ns:code", xmlNsMgr).GetAttributeValue("displayName");
                component.Code = new CodeableConcept(SystemCodeLoinc, loincCode, loincDisplayName);

                /**
                 * 檢驗結果/參考值
                 *   根據文件(https://emr.mohw.gov.tw/myemr/Html/DownloadStandardFile)
                 *   結果有以下幾種
                 *     文字
                 *       <value xsi:type="ST">Positive</value>
                 *     文字帶單位
                 *       <value xsi:type="ST" unit="ppm">Positive</value>
                 *     數字區間
                 *       <value xsi:type="IVL_PQ">
                 *         <low value="3.80" unit="g/dL"/>
                 *         <high value="10.0" unit="g/dl" />
                 *       </value>
                 *     單數字
                 *       <value xsi:type="PQ" value="3.80 unit="mg/dL" />
                 */
                var valueNode = obsNode.SelectSingleNode("ns:value", xmlNsMgr);
                var valueType = valueNode.GetAttributeValue("xsi:type");                
                switch (valueType)
                {
                    case "ST": // 文字結果，只處理數值不處理unit
                        var stringValue = valueNode.GetAttributeValue("value");
                        component.Value = new FhirString(stringValue);
                        break;
                    case "PQ": // 數字結果，處理數值與單位
                        var value = decimal.Parse(valueNode.GetAttributeValue("value"));
                        var unit = valueNode.GetAttributeValue("unit");
                        component.Value = CreateQuantity(value, unit);
                        break;
                    case "IVL_PQ": // 數字區間                        
                        var lowNode = valueNode.SelectSingleNode("ns:low", xmlNsMgr);
                        var lowValue = decimal.Parse(lowNode.GetAttributeValue("value"));
                        var lowUnit = lowNode.GetAttributeValue("unit");
                        var highNode = valueNode.SelectSingleNode("ns:high", xmlNsMgr);
                        var highValue = decimal.Parse(highNode.GetAttributeValue("value"));
                        var highUnit = highNode.GetAttributeValue("unit");
                        var valueRange = new Range();
                        valueRange.Low = CreateQuantity(lowValue, lowUnit);
                        valueRange.High = CreateQuantity(highValue, highUnit);
                        component.Value = valueRange;
                        break;
                    default:
                        throw new NotSupportedException("unsupported value type:" + valueType);
                }


                // 檢驗結果參考值，只參考range
                var rangeComponent = new Observation.ReferenceRangeComponent();
                var valueRangeNode = obsNode.SelectSingleNode("ns:referenceRange/ns:observationRange/ns:value", xmlNsMgr);
                var valueRangeType = valueRangeNode.GetAttributeValue("xsi:type");
                switch (valueRangeType)
                {
                    case "IVL_PQ": // 數字區間                        
                        var lowNode = valueRangeNode.SelectSingleNode("ns:low", xmlNsMgr);
                        var lowValue = decimal.Parse(lowNode.GetAttributeValue("value"));
                        var lowUnit = lowNode.GetAttributeValue("unit");
                        var highNode = valueRangeNode.SelectSingleNode("ns:high", xmlNsMgr);
                        var highValue = decimal.Parse(highNode.GetAttributeValue("value"));
                        var highUnit = highNode.GetAttributeValue("unit");
                        rangeComponent.Low = CreateQuantity(lowValue, lowUnit);
                        rangeComponent.High = CreateQuantity(highValue, highUnit);
                        component.ReferenceRange.Add(rangeComponent);
                        break;
                    default:
                        throw new NotSupportedException("unsupported value type:" + valueType);
                }
                observation.Component.Add(component);
            }

            observation = client.Create(observation);

            return observation;
        }

        private Hl7.Fhir.Model.Quantity CreateQuantity(decimal value, string unit)
        {
            // 進行Unit轉譯
            switch (unit)
            {
                case "sec":
                    unit = "s"; // 秒，ucum使用s
                    break;
                case "-":
                    unit = "%"; // 沒有單位，嘗試使用百分比
                    break;
            }

            return new Hl7.Fhir.Model.Quantity(value, unit);
        }
    }
}
