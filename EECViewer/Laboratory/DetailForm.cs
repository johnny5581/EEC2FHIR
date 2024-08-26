using EEC2FHIR.Utility;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EECViewer.Laboratory
{
    public partial class DetailForm : FormBase
    {
        private readonly FhirClient client;
        public DetailForm(FhirClient client)
        {
            InitializeComponent();
            this.client = client;
            dgvData.AutoGenerateColumns
                = dgvDetail.AutoGenerateColumns
                = true;
        }

        public void LoadData(ViewData model)
        {
            // 填寫資訊
            groupDocument.Text = model.Composition.Title;
            textDocId.Text = model.Composition.GetIdentifier(codeSystemLocal + "/lab") ?? model.Composition.GetIdentifier(codeSystemLocal);
            textDocDate.Text = DateTime.Parse(model.Composition.Date).ToString("yyyy/MM/dd");
            textDocAuthor.Text = model.Author.Name.ToText();
            textPatId.Text = model.Patient.GetIdentifier(TWPatient.CodeSystemTwIdentifier);
            textPatChtno.Text = $"{model.Patient.GetIdentifier(codeSystemLocal)} ({codeSystemLocal})";
            textPatName.Text = model.Patient.Name.ToText();
            textPatGender.Text = Convert.ToString(model.Patient.Gender);
            textPatBirthDate.Text = model.Patient.BirthDate;
            if (model.Organization != null)
            {
                textOrgId.Text = model.Organization.GetIdentifier(TWOrganization.CodeSystemTwIdentifier);
                textOrgName.Text = model.Organization.Name;
            }
            textOdrId.Text = model.Encounter.GetIdentifier(codeSystemLocal);
            textOdrDate.Text = model.Encounter.Period.Start;
            textOdrDr.Text = model.EncounterPractitioner.Name.ToText();



            var viewModels = model.Composition.Section.Select(section =>
            {
                var m = new ViewModel();
                var observationRef = section.Entry.FirstOrDefault(r => r.Type == "Observation");
                var observation = GetResource<Observation>(model.Composition, observationRef.Reference);

                var specimenRef = section.Entry.FirstOrDefault(r => r.Type == "Specimen");
                var specimen = GetResource<Specimen>(model.Composition, specimenRef.Reference);

                m.SpecType = specimen.Type.ToText();
                m.SpecBodySite = specimen.Collection?.BodySite?.ToText();
                m.Name = observation.Code.ToText();
                m.Status = Convert.ToString(observation.Status);
                m.Items = observation.Component.Select(component =>
                {
                    var item = new ItemViewModel();
                    item.Name = component.Code.ToText();
                    if (component.Value is Quantity)
                    {
                        var quantity = component.Value as Quantity;
                        item.Value = $"{quantity.Value} {quantity.Unit}";
                    }
                    else if (component.Value is FhirString)
                    {
                        var fhirString = component.Value as FhirString;
                        item.Value = fhirString.Value;
                    }
                    else if (component.Value is Range)
                    {
                        var range = component.Value as Range;
                        item.Value = $"{range.Low.Value} {range.Low.Unit} ~ {range.High.Value} {range.Low.Unit}";
                    }

                    if (component.ReferenceRange.Count > 0)
                    {
                        var refRange = component.ReferenceRange[0];
                        item.Reference = $"{refRange.Low.Value} {refRange.Low.Unit} ~ {refRange.High.Value} {refRange.Low.Unit}";
                    }
                    return item;
                }).ToArray();

                return m;
            });

            bindingSource.DataSource = viewModels;
            dgvData.AutoResizeColumns();
        }
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            var rowIndex = dgvData.SelectedCells.Count > 0 ? dgvData.SelectedCells[0].RowIndex : -1;
            bindingSourceDetail.DataSource = null;
            if (rowIndex > -1)
            {
                var model = dgvData.Rows[rowIndex].DataBoundItem as ViewModel;
                bindingSourceDetail.DataSource = model.Items;
                dgvDetail.AutoResizeColumns();
            }
        }

        private TResource GetResource<TResource>(Composition composition, string reference)
            where TResource : Resource
        {
            if (reference.StartsWith("#"))
            {
                var refId = reference.Substring(1);
                var resource = composition.Contained.FirstOrDefault(r => r.Id == refId);
                return (TResource)resource;
            }
            return client.Read<TResource>(reference);
        }


        public class ViewData
        {
            public Composition Composition { get; set; }
            public Patient Patient { get; set; }
            public Organization Organization { get; set; }
            public Encounter Encounter { get; set; }
            public Practitioner EncounterPractitioner { get; set; }
            public Practitioner Author { get; set; }
        }
        public class ViewModel
        {
            [DisplayName("採檢方式")]
            public string SpecType { get; set; }
            [DisplayName("採檢部位")]
            public string SpecBodySite { get; set; }
            [DisplayName("檢驗名稱")]
            public string Name { get; set; }
            [DisplayName("狀態")]
            public string Status { get; set; }
            [Browsable(false)]
            public ItemViewModel[] Items { get; set; }
        }

        public class ItemViewModel
        {
            [DisplayName("檢驗項目")]
            public string Name { get; set; }
            [DisplayName("結果")]
            public string Value { get; set; }
            [DisplayName("參考值")]
            public string Reference { get; set; }
        }


    }
}
