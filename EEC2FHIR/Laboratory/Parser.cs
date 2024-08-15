using EEC2FHIR.Utility;
using Hl7.Fhir.ElementModel.Types;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EEC2FHIR.Laboratory
{
    public class Parser : ParserBase
    {
        public Parser(FhirClient client) : base(client)
        {
        }

        public string SystemCodeGlobal { get; set; } = "https://www.cgmh.org.tw";
        public string SystemCodeLocal { get; set; } = "https://lnk.cgmh.org.tw";
        public static string SystemCodeLoinc { get; } = "http://loinc.org";
        public static string SystemCodeSnomed { get; } = "http://snomed.info/sct";

        public Bundle Parse(string xml)
        {
            var doc = ConvertToDoc(xml);
            var nsMgr = CreateNamespaceManager(xml);

            // 新增命名空間
            nsMgr.AddNamespace("", "urn:hl7-org:v3");
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");
            nsMgr.AddNamespace("cdp", "http://www.hl7.org.tw/EMR/CDocumentPayload/v1.0");
            nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            nsMgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.4.1#");
            nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // 取得根目錄
            var root = doc.XPathSelectElement("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument", nsMgr);

            // 建立摘要
            var composition = new Composition();
            composition.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckComposition");

            // 取得醫院資訊
            var organization = GetOrganizationResource(root, "ns:custodian/ns:assignedCustodian/ns:representedCustodianOrganization", nsMgr, composition);
            composition.Custodian = organization.GetReference();
            composition.Author.Add(organization.GetReference());

            // 取得病人資料
            var patient = GetPatientResource(root, "ns:recordTarget", nsMgr, composition);
            composition.Subject = patient.GetReference();

            // 取得檢驗報告醫技人員資訊
            var author = GetPractitionerResource(root, "ns:author/ns:assignedAuthor", nsMgr, composition);
            composition.Author.Add(author.GetReference());

            // 取得開單資訊
            var encounter = GetEncounterResource(root, "ns:documentationOf/ns:serviceEvent", nsMgr, composition);
            composition.Encounter = encounter.GetReference();

            // 取得檢驗內容
            var componentRoot = root.XPathSelectElement("ns:component/ns:structuredBody/ns:component/ns:section", nsMgr);
            var sectionLoincCode = componentRoot.XPathEvaluateString("ns:code/@code", nsMgr);
            var sectionDisplayName = componentRoot.XPathEvaluateString("ns:code/@displayName", nsMgr);
            var organizers = componentRoot.XPathSelectElements("ns:entry/ns:organizer", nsMgr);
            var observations = new List<Observation>();
            var specimens = new List<Specimen>();
            foreach (var organizer in organizers)
            {
                var sectionComponent = new Composition.SectionComponent();
                sectionComponent.Code = new CodeableConcept(SystemCodeLoinc, sectionLoincCode, sectionDisplayName);

                // 處理採檢資訊
                var specimen = CreateSpecimenResource(organizer, "ns:specimen", nsMgr, composition);
                // 給予特定ID
                specimen.Id = Guid.NewGuid().ToString();
                sectionComponent.Entry.Add(specimen.GetReference(ResourceReferenceType.IdOnly));
                composition.Contained.Add(specimen);
                specimens.Add(specimen);

                var observation = CreateObservationResource(organizer, "", nsMgr, composition, specimen);
                // 給予特定ID
                observation.Id = Guid.NewGuid().ToString();
                sectionComponent.Entry.Add(observation.GetReference(ResourceReferenceType.IdOnly));
                composition.Contained.Add(observation);
                observations.Add(observation);

                composition.Section.Add(sectionComponent);
            }

            // 摘要標題
            composition.Title = root.XPathEvaluateString("ns:title", nsMgr);

            // 摘要時間
            var effectiveTime = root.XPathEvaluateString("ns:effectiveTime/@value", nsMgr);
            composition.Date = DateUtility.Convert(effectiveTime, outFormat: "ISO", inFormat: "yyyyMMddHHmmss");

            // 摘要狀態
            composition.Status = CompositionStatus.Final;

            // 摘要類型
            composition.Type = new CodeableConcept(SystemCodeLoinc, "11503-0", "檢驗檢查");

            // 產生composition
            CreateResource(composition);



            // 組合bundle
            // 組合bundle
            var bundle = new Bundle();
            bundle.SetMetaProfile("https://twcore.mohw.gov.tw/ig/twcore/StructureDefinition/Bundle-twcore");
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


        private Patient GetPatientResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        {
            Patient patient = null;

            var node = root.XPathSelectElement(xpath, nsMgr);

            // 醫院病歷號
            var chtNo = node.XPathEvaluateString("ns:patientRole/ns:id/@extension", nsMgr);
            // 身份證字號
            var idno = node.XPathEvaluateString("ns:patientRole/ns:patient/ns:id/@extension", nsMgr);

            // 先透過病歷號查詢看看有沒有這個病人，有的話就使用目前資料
            var querier = new TWPatientQuerier(client);
            patient = querier.GetByIdentifier(SystemCodeLocal, chtNo);
            if (patient != null)
                return patient;

            // 再來透過身分證字號查詢，如果有的話，把病歷號合併
            patient = querier.GetByTwIdentifier(idno);
            if (patient != null)
            {
                patient.SetMedicalRecordNumber(SystemCodeLocal, chtNo);
                return UpdateResource(patient); // 更新這筆病人資料
            }

            // 建立新的病人資料
            patient = new Patient();
            patient.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckPatient");
            patient.SetTwIdentifier(idno);
            patient.SetMedicalRecordNumber(SystemCodeLocal, chtNo);

            // 中文姓名
            var cnm = node.XPathEvaluateString("ns:patientRole/ns:patient/ns:name", nsMgr);
            patient.SetChineseName(cnm);

            // 性別
            var gender = node.XPathEvaluateString("ns:patientRole/ns:patient/ns:administrativeGenderCode/@code", nsMgr);
            patient.SetAdministrativeGenderV3(gender);

            // 生日
            var birthDat = node.XPathEvaluateString("ns:patientRole/ns:patient/ns:birthTime/@value", nsMgr);
            patient.BirthDate = DateUtility.Convert(birthDat);

            // 管理機構
            patient.ManagingOrganization = GetOrganizationResource(node, "ns:patientRole/ns:providerOrganization", nsMgr, composition).GetReference();

            return CreateResource(patient);
        }

        private Organization GetOrganizationResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        {
            Organization organization = null;

            var node = root.XPathSelectElement(xpath, nsMgr);

            // 醫療院所代碼
            var hospId = node.XPathEvaluateString("ns:id/@extension", nsMgr);

            // 先檢查有沒有這個機構，有的話就使用目前資料
            var querier = new TWOrganizationQuerier(client);
            organization = querier.GetByTwIdentifier(hospId);
            if (organization != null)
                return organization;

            // 因為FHIR建立資源比較慢，檢核composition.Custodian的資源是否相同
            if (composition.Custodian != null)
            {
                var reference = composition.Custodian.Reference;
                var org = client.Read<Organization>(reference);
                if (org != null && org.GetTwIdentifier() == hospId)
                    return org;
            }

            // 建立新的機構資料
            organization = new Organization();
            organization.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckOrganization");
            organization.SetTwIdentifier(hospId);

            var hospName = node.XPathEvaluateString("ns:name", nsMgr);
            organization.Name = hospName;

            return CreateResource(organization);
        }

        private Practitioner GetPractitionerResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        {
            Practitioner practitioner = null;

            var node = root.XPathSelectElement(xpath, nsMgr);

            var empId = node.XPathEvaluateString("ns:id/@extension", nsMgr);

            // 先查看看有沒有這個醫事人員的代碼，有的話就用這個代碼
            var querier = new TWPractitionerQuerier(client);
            practitioner = querier.GetByIdentifier(SystemCodeGlobal, empId);
            if (practitioner != null)
                return practitioner;

            // !! 因為HAPI FHIR cahce的關係，檢查composition.author是否有這個資源，有的話就使用
            if (composition.Author != null && !composition.Author.IsNullOrEmpty())
            {
                // composition.Author[0] 是院區，跳過
                for (var i=1; i<composition.Author.Count; i++)
                {
                    var prac = client.Read<Practitioner>(composition.Author[i].Reference);
                    if (prac != null && prac.GetIdentifier(SystemCodeGlobal) == empId)
                        return prac;
                }
            }

            // 建立新的醫事人員資料
            practitioner = new Practitioner();
            practitioner.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckPractitioner");
            practitioner.SetHospitalIdentifier(SystemCodeGlobal, empId);

            // 中文姓名
            var cnm = node.XPathEvaluateString("ns:assignedPerson/ns:name", nsMgr);
            practitioner.SetChineseName(cnm);

            return CreateResource(practitioner);
        }
        private Encounter GetEncounterResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        {
            Encounter encounter = null;

            var node = root.XPathSelectElement(xpath, nsMgr);

            var opdNo = root.XPathEvaluateString("ns:id/@extension", nsMgr);

            // 先查看看有沒有這個就診紀錄，有的話就使用
            var querier = new FhirResourceQuerier<Encounter>(client);
            encounter = querier.GetByIdentifier(SystemCodeLocal, opdNo);
            if (encounter != null)
                return encounter;

            // 建立新的就診紀錄
            encounter = new Encounter();
            encounter.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckEncounter");
            encounter.Identifier.Add(new Identifier(SystemCodeLocal, opdNo));
            encounter.Class = new Coding("http://terminology.hl7.org/CodeSystem/v3-ActCode", "OBSENC", "observation encounter");
            encounter.ServiceType = new CodeableConcept(SystemCodeSnomed, "394609007", "General surgery", "Medical Services");
            encounter.Status = Encounter.EncounterStatus.Finished;
            encounter.Subject = composition.Subject;

            // 開單醫師
            var practitoner = GetPractitionerResource(node, "ns:performer/ns:assignedEntity", nsMgr, composition);
            encounter.Participant.Add(new Encounter.ParticipantComponent
            {
                Individual = practitoner.GetReference(),
            });

            // 開單日期
            var time = node.XPathEvaluateString("ns:effectiveTime/@value", nsMgr);
            encounter.Period = new Period
            {
                Start = DateUtility.Convert(time, inFormat: "yyyyMMddHHmm")
            };

            return CreateResource(encounter);
        }

        private Specimen CreateSpecimenResource(XElement organizer, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        {
            var node = organizer.XPathSelectElement(xpath, nsMgr);

            // 注意: 每次都產生新的Specimen
            var specimen = new Specimen();
            specimen.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckSpecimen");

            var specimenCode = node.XPathEvaluateString("ns:specimenRole/ns:specimenPlayingEntity/ns:code/@code", nsMgr);
            if (specimenCode == "Blood")
                specimenCode = " BLD"; // 轉譯
            specimen.Type = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v3-SpecimenType", specimenCode);
            specimen.Subject = composition.Subject;

            // 檢體來源，使用固定值
            specimen.Collection = new Specimen.CollectionComponent();
            specimen.Collection.BodySite = new CodeableConcept(SystemCodeSnomed, "408512008", "Posterior carpal region", "Posterior carpal region");

            return specimen;
        }
        private Observation CreateObservationResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition, Specimen specimen)
        {
            var node = string.IsNullOrEmpty(xpath) ? root : root.XPathSelectElement(xpath, nsMgr);

            // 注意: 每次都產生新的Observation
            var observation = new Observation();
            observation.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckObservation");
            observation.Status = ObservationStatus.Final;
            observation.Subject = composition.Subject;
            observation.Encounter = composition.Encounter;
            observation.Performer.Add(composition.Author[1]);
            observation.Specimen = specimen.GetReference();

            // 設定檢驗單資訊            
            var id = root.Document.Root.XPathEvaluateString("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument/ns:id/@extension", nsMgr);
            observation.Identifier.Add(new Identifier(SystemCodeLocal, id));

            // 設定檢驗項目            
            var obsLoincCode = node.XPathEvaluateString("ns:code/@code", nsMgr);
            var obsLoincDisplayName = node.XPathEvaluateString("ns:code/@displayName", nsMgr);
            var obsNhiCode = node.XPathEvaluateString("ns:code/ns:translation/@code", nsMgr);
            var obsNhiDisplayName = node.XPathEvaluateString("ns:code/ns:translation/@displayName", nsMgr);
            observation.Code = new CodeableConcept("http://loinc.org", obsLoincCode, obsLoincDisplayName, obsNhiDisplayName ?? obsLoincDisplayName);

            // 採檢時間
            var specTime = root.Document.Root.XPathEvaluateString("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:effectiveTime/@value", nsMgr);
            // 收件時間
            var rcvTime = node.XPathEvaluateString("ns:effectiveTime/@value", nsMgr);
            observation.Effective = new Period
            {
                Start = DateUtility.Convert(specTime, inFormat: "yyyyMMddHHmm"),
                End = DateUtility.Convert(rcvTime, inFormat: "yyyyMMddHHmm")
            };

            // 項目與結果
            var interpretation = new CodeableConcept(SystemCodeLoinc, "30954-2", "Relevant diagnostic tests and/or laboratory data");
            observation.Interpretation.Add(interpretation);

            // 備註            
            observation.Note.Add(new Annotation { Text = new Markdown("無") });

            // 檢體類別
            observation.BodySite = specimen.Collection.BodySite;

            // 檢驗內容
            var components = node.XPathSelectElements("ns:component", nsMgr);
            foreach (var component in components)
            {
                var obsComponent = new Observation.ComponentComponent();

                // 檢驗細項代碼
                var loincCode = component.XPathEvaluateString("ns:observation/ns:code/@code", nsMgr);
                var loincDisplayName = component.XPathEvaluateString("ns:observation/ns:code/@displayName", nsMgr);
                obsComponent.Code = new CodeableConcept(SystemCodeLoinc, loincCode, loincDisplayName);


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
                var valueNode = component.XPathSelectElement("ns:observation/ns:value", nsMgr);
                var valueType = valueNode.XPathEvaluateString("@xsi:type", nsMgr);
                switch (valueType)
                {
                    case "ST": // 文字結果，只處理數值不處理unit
                        var stringValue = valueNode.XPathEvaluateString("@value", nsMgr);
                        obsComponent.Value = new FhirString(stringValue);
                        break;
                    case "PQ": // 數字結果，處理數值與單位
                        var value = decimal.Parse(valueNode.XPathEvaluateString("@value", nsMgr));
                        var unit = valueNode.XPathEvaluateString("@unit", nsMgr);
                        obsComponent.Value = CreateQuantity(value, unit);
                        break;
                    case "IVL_PQ": // 數字區間                                                
                        var lowValue = decimal.Parse(valueNode.XPathEvaluateString("ns:low/@value", nsMgr));
                        var lowUnit = valueNode.XPathEvaluateString("ns:low/@unit", nsMgr);
                        var highValue = decimal.Parse(valueNode.XPathEvaluateString("ns:high/@value", nsMgr));
                        var highUnit = valueNode.XPathEvaluateString("ns:high/@unit", nsMgr);
                        var valueRange = new Range();
                        valueRange.Low = CreateQuantity(lowValue, lowUnit);
                        valueRange.High = CreateQuantity(highValue, highUnit);
                        obsComponent.Value = valueRange;
                        break;
                    default:
                        throw new NotSupportedException("unsupported value type:" + valueType);
                }

                // 參考值，只參考range
                var rangeComponent = new Observation.ReferenceRangeComponent();
                var valueRangeNode = component.XPathSelectElement("ns:observation/ns:referenceRange/ns:observationRange/ns:value", nsMgr);
                var valueRangeType = valueRangeNode.XPathEvaluateString("@xsi:type", nsMgr);
                switch (valueRangeType)
                {
                    case "IVL_PQ": // 數字區間                        
                        var lowValue = decimal.Parse(valueRangeNode.XPathEvaluateString("ns:low/@value", nsMgr));
                        var lowUnit = valueRangeNode.XPathEvaluateString("ns:low/@unit", nsMgr);
                        var highValue = decimal.Parse(valueRangeNode.XPathEvaluateString("ns:high/@value", nsMgr));
                        var highUnit = valueRangeNode.XPathEvaluateString("ns:high/@unit", nsMgr);
                        rangeComponent.Low = CreateQuantity(lowValue, lowUnit);
                        rangeComponent.High = CreateQuantity(highValue, highUnit);
                        obsComponent.ReferenceRange.Add(rangeComponent);
                        break;
                    default:
                        throw new NotSupportedException("unsupported value type:" + valueType);
                }
                observation.Component.Add(obsComponent);
            }
            return observation;
        }

        private XmlNamespaceManager CreateNamespaceManager(string xml)
        {
            var reader = XmlReader.Create(new StringReader(xml));
            var nameTable = reader.NameTable;
            var namespaceManager = new XmlNamespaceManager(nameTable);
            return namespaceManager;
        }
        private Hl7.Fhir.Model.Quantity CreateQuantity(decimal value, string unit)
        {
            return new Hl7.Fhir.Model.Quantity(value, unit, "http://www.cgmh.org.tw/unit");
        }
    }
}
