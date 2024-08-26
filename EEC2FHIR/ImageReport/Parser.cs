using EEC2FHIR.Utility;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;

namespace EEC2FHIR.ImageReport
{
    public class Parser : ParserBase
    {
        private const string SystemObservationCategory = "http://terminology.hl7.org/CodeSystem/observation-category";

        public Parser(FhirClient client) : base(client)
        {
        }

        public override Bundle Parse(string xml)
        {
            var doc = ConvertToDoc(xml);
            var nsMgr = doc.CreateCdaR2NamespaceManager();


            // 取得根目錄
            var root = doc.XPathSelectElement("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument", nsMgr);

            // 建立摘要
            var composition = new Composition();
            composition.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/ImageComposition");

            // 取得醫院資訊
            var organization = GetOrganizationResource(root, "ns:custodian/ns:assignedCustodian/ns:representedCustodianOrganization", nsMgr, composition);
            composition.Custodian = organization.GetReference();
            composition.Author.Add(organization.GetReference()); //只能有一個

            // 取得病人資料
            var patient = GetPatientResource(root, "ns:recordTarget", nsMgr, composition);
            composition.Subject = patient.GetReference();

            //// 取得影像報告人員資訊
            var author = GetPractitionerResource(root, "ns:author/ns:assignedAuthor", nsMgr, composition);
            //composition.Author.Add(author.GetReference());

            //// 取得影像報告認證人員資訊
            var legalAuthor = GetPractitionerResource(root, "ns:legalAuthenticator/ns:assignedEntity", nsMgr, composition);
            //composition.Author.Add(legalAuthor.GetReference());

            // 取得開單資訊
            var encounter = GetEncounterResource(root, "ns:componentOf/ns:encompassingEncounter", nsMgr, composition);
            composition.Encounter = encounter.GetReference();

            // 取得影像報告內容
            var components = root.XPathSelectElements("ns:component/ns:structuredBody/ns:component/ns:section", nsMgr);



            // 摘要標題
            composition.Title = root.XPathEvaluateString("ns:title", nsMgr);

            // 摘要時間
            var effectiveTime = root.XPathEvaluateString("ns:effectiveTime/@value", nsMgr);
            composition.Date = DateUtility.Convert(effectiveTime, outFormat: "ISO", inFormat: "yyyyMMddHHmmss");

            // 摘要狀態
            composition.Status = CompositionStatus.Final;

            // 摘要類型
            composition.Type = new CodeableConcept(SystemCodeLoinc, "18782-3", "Radiology Study observation (narrative)", "Radiology Study observation (narrative)");



            var observation = new Observation();
            observation.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/Observation-Imaging-Result");
            var pPACSNOList = root.XPathSelectElements("ns:inFulfillmentOf/ns:order/ns:id", nsMgr);
            string pPACSNOValueRaw = null;
            foreach (var pPACSNO in pPACSNOList)
            {
                var pPACSNOValue = pPACSNO.XPathEvaluateString("@extension", nsMgr);
                if (pPACSNOValueRaw == null)
                {
                    pPACSNOValueRaw = pPACSNOValue;
                }
                observation.Identifier.Add(new Identifier(SystemCodeLocal, pPACSNOValue));//可能有缺要注意
            }
            observation.Status = ObservationStatus.Final;
            observation.Category.Add(new CodeableConcept(SystemObservationCategory, "imaging", "Imaging", "Imaging"));
            observation.Code = new CodeableConcept("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/ICD-10-procedurecode", "BW24ZZZ", "Computerized Tomography (CT Scan) of Chest and Abdomen", "Computerized Tomography (CT Scan) of Chest and Abdomen");//需要設定對應檔案
            observation.Subject = composition.Subject;
            observation.Effective = new FhirDateTime(composition.Date);

            observation = CreateResource(observation);

            var endpoint = new Endpoint();
            endpoint.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/MitwEndpoint");
            endpoint.Address = "http://localhost:8081/dicom-web";//影像網址!
            endpoint.Status = Endpoint.EndpointStatus.Active;
            endpoint.ConnectionType = new Coding("http://terminology.hl7.org/CodeSystem/endpoint-connection-type", "dicom-wado-rs", "DICOM WADO-RS");
            endpoint.PayloadType.Add(new CodeableConcept("", "", "DICOM"));
            endpoint = CreateResource(endpoint);

            var condition = new Condition();
            var imagingStudy = new ImagingStudy();
            string pBodySite = "";
            string pReport = "";
            foreach (var component in components)
            {
                var pCode = component.XPathEvaluateString("ns:code/@code", nsMgr);
                var pDisplayName = component.XPathEvaluateString("ns:code/@displayName", nsMgr);
                if (pCode == "55286-9")//病史 應該只有一個
                {
                    pBodySite = component.XPathEvaluateString("ns:text", nsMgr);
                }
                if (pCode == "11515-4")//病史 應該只有一個
                {
                    pReport = component.XPathEvaluateString("ns:text", nsMgr);
                }

            }
            foreach (var component in components)
            {
                var pCode = component.XPathEvaluateString("ns:code/@code", nsMgr);
                var pDisplayName = component.XPathEvaluateString("ns:code/@displayName", nsMgr);
                if (pCode == "10164-2")//病史 應該只有一個
                {
                    var pText = component.XPathEvaluateString("ns:text", nsMgr);
                    condition.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/TWCoreCondition");
                    condition.ClinicalStatus = new CodeableConcept("http://terminology.hl7.org/CodeSystem/condition-clinical", "active", "Active", "Active");//???
                    condition.Category.Add(new CodeableConcept("http://terminology.hl7.org/CodeSystem/condition-category", "encounter-diagnosis", "Encounter Diagnosis", "Encounter Diagnosis"));//???
                    condition.Code = new CodeableConcept(SystemCodeLoinc, pCode, pDisplayName, pText);
                    condition.Subject = composition.Subject;
                    condition = CreateResource(condition);
                }
                if (pCode == "121181")
                {
                    var pStudyUID = component.XPathEvaluateString("ns:entry/ns:act/ns:id/@root", nsMgr);

                    imagingStudy.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/ImagingStudyBase");
                    var pStudyUID_identifier = new Identifier();
                    pStudyUID_identifier.Use = Identifier.IdentifierUse.Official;
                    pStudyUID_identifier.Type = new CodeableConcept("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/ImageIdentifierType", "SIUID", "Study instancce UID", "DICOM Study Instance UID");
                    pStudyUID_identifier.System = "urn:dicom:uid";
                    pStudyUID_identifier.Value = "urn:oid:"+pStudyUID;
                    imagingStudy.Identifier.Add(pStudyUID_identifier);
                    var pAccession_ID_identifier = new Identifier();
                    pAccession_ID_identifier.Use = Identifier.IdentifierUse.Official;
                    pAccession_ID_identifier.Type = new CodeableConcept("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/ImageIdentifierType", "ACSN", "Accession ID", "Accession No 檢查單號");
                    pAccession_ID_identifier.System = SystemCodeLocal;
                    pAccession_ID_identifier.Value = pPACSNOValueRaw;
                    imagingStudy.Identifier.Add(pAccession_ID_identifier);
                    imagingStudy.Status = ImagingStudy.ImagingStudyStatus.Available;
                    imagingStudy.Subject = composition.Subject;
                    imagingStudy.Started = composition.Date;//需要改為影像時間(收件時間)，需要從DICOM來
                    imagingStudy.Endpoint.Add( endpoint.GetReference());
                    var pSeries = component.XPathSelectElements("ns:entry/ns:act/ns:entryRelationship/ns:act", nsMgr);
                    var pImageNumber = component.XPathSelectElements("ns:entry/ns:act/ns:entryRelationship/ns:act/ns:entryRelationship", nsMgr);

                    imagingStudy.NumberOfSeries = pSeries.Count();
                    imagingStudy.NumberOfInstances = pImageNumber.Count();
                    imagingStudy.ProcedureCode.Add(new CodeableConcept("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/ICD-10-procedurecode", "BW24ZZZ", "Computerized Tomography (CT Scan) of Chest and Abdomen", ""));//需要對應檔

                    foreach (var pSerie in pSeries)
                    {

                        var pUID = pSerie.XPathEvaluateString("ns:id/@root", nsMgr);
                        var pMod = pSerie.XPathEvaluateString("ns:code/ns:qualifier/ns:value/@code", nsMgr);

                        var pImageNumberS = pSerie.XPathSelectElements("ns:entryRelationship", nsMgr);

                        List<Hl7.Fhir.Model.ImagingStudy.InstanceComponent> Instance = new List<ImagingStudy.InstanceComponent>();
                        foreach (var imageInstance in pImageNumberS)
                        {
                            var pInsUID = imageInstance.XPathEvaluateString("ns:observation/ns:id/@root", nsMgr);
                            Instance.Add(new ImagingStudy.InstanceComponent
                            {
                                Uid = pInsUID,
                                SopClass = new Coding("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/DicomsopClass", "urn:oid:1.2.840.10008.5.1.4.1.1.2", "CT Image Storage"),//需要有對應資料
                            });
                        }
                        imagingStudy.Series.Add(new ImagingStudy.SeriesComponent
                        {
                            Uid = pUID,
                            Modality = new Coding("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/AcquisitionModality", pMod),
                            BodySite = new Coding(SystemCodeLoinc, "55286-9", pBodySite),//需要有對應資料
                            Instance = Instance,
                        });
                    }
                }
            }
            imagingStudy = CreateResource(imagingStudy);

            var diagnosticReport = new DiagnosticReport();
            diagnosticReport.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/DiagnosticReport-Image");
            diagnosticReport.Identifier.Add(new Identifier(SystemCodeLocal, pPACSNOValueRaw));
            diagnosticReport.Status = DiagnosticReport.DiagnosticReportStatus.Final;
            diagnosticReport.Category.Add(new CodeableConcept(SystemCodeLoinc, "LP29684-5", "Radiology", "Radiology"));
            diagnosticReport.Code = new CodeableConcept("https://twcore.mohw.gov.tw/ig/emr/CodeSystem/ICD-10-procedurecode", "BW24ZZZ", "Computerized Tomography (CT Scan) of Chest and Abdomen", "Computerized Tomography (CT Scan) of Chest and Abdomen");
            diagnosticReport.Subject = composition.Subject;
            diagnosticReport.Encounter = encounter.GetReference();
            diagnosticReport.Effective = new FhirDateTime(composition.Date);
            diagnosticReport.Performer.Add(author.GetReference());
            //diagnosticReport.ResultsInterpreter.Add(legalAuthor.GetReference());
            diagnosticReport.Result.Add(observation.GetReference());//診斷結果(Finding) 需要更改
            diagnosticReport.ImagingStudy.Add(imagingStudy.GetReference());//影像檢查
            diagnosticReport.Conclusion = pReport;//報告內容
            diagnosticReport = CreateResource(diagnosticReport);

            var sectionComponent = new Composition.SectionComponent();
            sectionComponent.Code = new CodeableConcept(SystemCodeLoinc, "30954-2", "Relevant diagnostic tests/laboratory data Narrative", "Relevant diagnostic tests/laboratory data Narrative");
            sectionComponent.Entry.Add(author.GetReference());
            sectionComponent.Entry.Add(condition.GetReference());
            sectionComponent.Entry.Add(imagingStudy.GetReference());
            sectionComponent.Entry.Add(observation.GetReference());
            sectionComponent.Entry.Add(diagnosticReport.GetReference());
            // TODO: composition - sections (影像內容)
            composition.Section.Add(sectionComponent);
            // 產生composition
            composition = CreateResource(composition);

            // 組合bundle
            var bundle = new Bundle();
            bundle.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/ImageBundle");
            bundle.Type = Bundle.BundleType.Document;
            bundle.Identifier = new Identifier("https://twcore.mohw.gov.tw/ig/index.html", "Bundle-EMR");
            bundle.Timestamp = DateTimeOffset.Now;

            bundle.AppendEntryResource(composition);
            bundle.AppendEntryResource(patient);
            bundle.AppendEntryResource(organization);

            bundle.AppendEntryResource(author);//Practitioner
            bundle.AppendEntryResource(encounter);
            bundle.AppendEntryResource(condition);

            bundle.AppendEntryResource(observation);
            bundle.AppendEntryResource(diagnosticReport);
            bundle.AppendEntryResource(imagingStudy);

            return bundle;
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
            encounter.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/TWCoreEncounter");
            encounter.Identifier.Add(new Identifier(SystemCodeLocal, opdNo));
            encounter.Class = new Coding("http://terminology.hl7.org/CodeSystem/v3-ActCode", "PRENC");
            encounter.Status = Encounter.EncounterStatus.Finished;
            encounter.Subject = composition.Subject;

            // 開單醫師
            var practitoner = GetPractitionerResource(node, "ns:encounterParticipant", nsMgr, composition);
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

        //private Specimen CreateSpecimenResource(XElement organizer, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        //{
        //    var node = organizer.XPathSelectElement(xpath, nsMgr);

        //    // 注意: 每次都產生新的Specimen
        //    var specimen = new Specimen();
        //    specimen.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckSpecimen");

        //    var specimenCode = node.XPathEvaluateString("ns:specimenRole/ns:specimenPlayingEntity/ns:code/@code", nsMgr);
        //    if (specimenCode == "Blood")
        //        specimenCode = " BLD"; // 轉譯
        //    specimen.Type = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v3-SpecimenType", specimenCode);
        //    specimen.Subject = composition.Subject;

        //    // 檢體來源，使用固定值
        //    specimen.Collection = new Specimen.CollectionComponent();
        //    specimen.Collection.BodySite = new CodeableConcept(SystemCodeSnomed, "408512008", "Posterior carpal region", "Posterior carpal region");

        //    return specimen;
        //}
        //private Observation CreateObservationResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition, Specimen specimen)
        //{
        //    var node = string.IsNullOrEmpty(xpath) ? root : root.XPathSelectElement(xpath, nsMgr);

        //    // 注意: 每次都產生新的Observation
        //    var observation = new Observation();
        //    observation.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/InspectionCheckObservation");
        //    observation.Status = ObservationStatus.Final;
        //    observation.Subject = composition.Subject;
        //    observation.Encounter = composition.Encounter;
        //    observation.Performer.Add(composition.Author[1]);
        //    observation.Specimen = specimen.GetReference();

        //    // 設定檢驗單資訊            
        //    var id = root.Document.Root.XPathEvaluateString("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument/ns:id/@extension", nsMgr);
        //    // 設定OID單號
        //    observation.Identifier.Add(new Identifier(SystemCodeLocal, id));

        //    // TODO: 解析檢驗單號


        //    // 設定檢驗項目            
        //    var obsLoincCode = node.XPathEvaluateString("ns:code/@code", nsMgr);
        //    var obsLoincDisplayName = node.XPathEvaluateString("ns:code/@displayName", nsMgr);
        //    var obsNhiCode = node.XPathEvaluateString("ns:code/ns:translation/@code", nsMgr);
        //    var obsNhiDisplayName = node.XPathEvaluateString("ns:code/ns:translation/@displayName", nsMgr);
        //    observation.Code = new CodeableConcept("http://loinc.org", obsLoincCode, obsLoincDisplayName, obsNhiDisplayName ?? obsLoincDisplayName);

        //    // 採檢時間
        //    var specTime = root.Document.Root.XPathEvaluateString("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:effectiveTime/@value", nsMgr);
        //    // 收件時間
        //    var rcvTime = node.XPathEvaluateString("ns:effectiveTime/@value", nsMgr);
        //    observation.Effective = new Period
        //    {
        //        Start = DateUtility.Convert(specTime, inFormat: "yyyyMMddHHmm"),
        //        End = DateUtility.Convert(rcvTime, inFormat: "yyyyMMddHHmm")
        //    };

        //    // 項目與結果
        //    var interpretation = new CodeableConcept(SystemCodeLoinc, "30954-2", "Relevant diagnostic tests and/or laboratory data");
        //    observation.Interpretation.Add(interpretation);

        //    // 備註            
        //    observation.Note.Add(new Annotation { Text = new Markdown("無") });

        //    // 檢體類別
        //    observation.BodySite = specimen.Collection.BodySite;

        //    // 檢驗內容
        //    var components = node.XPathSelectElements("ns:component", nsMgr);
        //    foreach (var component in components)
        //    {
        //        var obsComponent = new Observation.ComponentComponent();

        //        // 檢驗細項代碼
        //        var loincCode = component.XPathEvaluateString("ns:observation/ns:code/@code", nsMgr);
        //        var loincDisplayName = component.XPathEvaluateString("ns:observation/ns:code/@displayName", nsMgr);
        //        obsComponent.Code = new CodeableConcept(SystemCodeLoinc, loincCode, loincDisplayName);


        //        /**
        //         * 檢驗結果/參考值
        //         *   根據文件(https://emr.mohw.gov.tw/myemr/Html/DownloadStandardFile)
        //         *   結果有以下幾種
        //         *     文字
        //         *       <value xsi:type="ST">Positive</value>
        //         *     文字帶單位
        //         *       <value xsi:type="ST" unit="ppm">Positive</value>
        //         *     數字區間
        //         *       <value xsi:type="IVL_PQ">
        //         *         <low value="3.80" unit="g/dL"/>
        //         *         <high value="10.0" unit="g/dl" />
        //         *       </value>
        //         *     單數字
        //         *       <value xsi:type="PQ" value="3.80 unit="mg/dL" />
        //         */
        //        var valueNode = component.XPathSelectElement("ns:observation/ns:value", nsMgr);
        //        var valueType = valueNode.XPathEvaluateString("@xsi:type", nsMgr);
        //        switch (valueType)
        //        {
        //            case "ST": // 文字結果，只處理數值不處理unit
        //                var stringValue = valueNode.XPathEvaluateString("@value", nsMgr);
        //                obsComponent.Value = new FhirString(stringValue);
        //                break;
        //            case "PQ": // 數字結果，處理數值與單位
        //                var value = decimal.Parse(valueNode.XPathEvaluateString("@value", nsMgr));
        //                var unit = valueNode.XPathEvaluateString("@unit", nsMgr);
        //                obsComponent.Value = CreateQuantity(value, unit);
        //                break;
        //            case "IVL_PQ": // 數字區間                                                
        //                var lowValue = decimal.Parse(valueNode.XPathEvaluateString("ns:low/@value", nsMgr));
        //                var lowUnit = valueNode.XPathEvaluateString("ns:low/@unit", nsMgr);
        //                var highValue = decimal.Parse(valueNode.XPathEvaluateString("ns:high/@value", nsMgr));
        //                var highUnit = valueNode.XPathEvaluateString("ns:high/@unit", nsMgr);
        //                var valueRange = new Range();
        //                valueRange.Low = CreateQuantity(lowValue, lowUnit);
        //                valueRange.High = CreateQuantity(highValue, highUnit);
        //                obsComponent.Value = valueRange;
        //                break;
        //            default:
        //                throw new NotSupportedException("unsupported value type:" + valueType);
        //        }

        //        // 參考值，只參考range
        //        var rangeComponent = new Observation.ReferenceRangeComponent();
        //        var valueRangeNode = component.XPathSelectElement("ns:observation/ns:referenceRange/ns:observationRange/ns:value", nsMgr);
        //        var valueRangeType = valueRangeNode.XPathEvaluateString("@xsi:type", nsMgr);
        //        switch (valueRangeType)
        //        {
        //            case "IVL_PQ": // 數字區間                        
        //                var lowValue = decimal.Parse(valueRangeNode.XPathEvaluateString("ns:low/@value", nsMgr));
        //                var lowUnit = valueRangeNode.XPathEvaluateString("ns:low/@unit", nsMgr);
        //                var highValue = decimal.Parse(valueRangeNode.XPathEvaluateString("ns:high/@value", nsMgr));
        //                var highUnit = valueRangeNode.XPathEvaluateString("ns:high/@unit", nsMgr);
        //                rangeComponent.Low = CreateQuantity(lowValue, lowUnit);
        //                rangeComponent.High = CreateQuantity(highValue, highUnit);
        //                obsComponent.ReferenceRange.Add(rangeComponent);
        //                break;
        //            default:
        //                throw new NotSupportedException("unsupported value type:" + valueType);
        //        }
        //        observation.Component.Add(obsComponent);
        //    }
        //    return observation;
        //}

        /// <summary>
        /// 轉換CDAR2的病人資訊
        /// </summary>        
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
            patient.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/TWCorePatient");
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
        /// <summary>
        /// 轉換CDAR2的組織資訊
        /// </summary>
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
            organization.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/TWCoreOrganization");
            organization.SetTwIdentifier(hospId);

            var hospName = node.XPathEvaluateString("ns:name", nsMgr);
            organization.Name = hospName;

            return CreateResource(organization);
        }
        /// <summary>
        /// 轉換CDAR2的人員資訊
        /// </summary>
        private Practitioner GetPractitionerResource(XElement root, string xpath, XmlNamespaceManager nsMgr, Composition composition)
        {
            Practitioner practitioner = null;

            var node = root.XPathSelectElement(xpath, nsMgr);

            // 抓第一個代碼
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
                for (var i = 1; i < composition.Author.Count; i++)
                {
                    var prac = client.Read<Practitioner>(composition.Author[i].Reference);
                    if (prac != null && prac.GetIdentifier(SystemCodeGlobal) == empId)
                        return prac;
                }
            }

            // 建立新的醫事人員資料
            practitioner = new Practitioner();
            practitioner.SetMetaProfile("https://twcore.mohw.gov.tw/ig/emr/StructureDefinition/TWCorePractitioner");
            practitioner.SetHospitalIdentifier(SystemCodeGlobal, empId);

            // 中文姓名
            var cnm = node.XPathEvaluateString("ns:assignedPerson/ns:name", nsMgr);
            practitioner.SetChineseName(cnm);

            return CreateResource(practitioner);
        }
    }
}
