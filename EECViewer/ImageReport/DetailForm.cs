using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EECViewer.Laboratory.DetailForm;

namespace EECViewer.ImageReport
{
    public partial class DetailForm : FormBase
    {
        private readonly FhirClient client;
        public DetailForm(FhirClient client)
        {
            InitializeComponent();
            this.client = client;
            dgvSeries.AutoGenerateColumns
                = dgvInstance.AutoGenerateColumns
                = true;
        }
        public void LoadData(ViewData model)
        {
            // 填寫資訊
            groupDocument.Text = model.Composition.Title;
            textDocId.Text = model.Composition.GetIdentifier(codeSystemLocal + "/image") ?? model.Composition.GetIdentifier(codeSystemLocal);
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

            Text = model.Composition.Title;

            var diagnosticReport = model.Composition.Contained.OfType<DiagnosticReport>().FirstOrDefault();
            textImgType.Text = diagnosticReport.Code.ToText();
            textConclusion.Text = diagnosticReport.Conclusion;

            var imagingStudy = model.Composition.Contained.OfType<ImagingStudy>().FirstOrDefault();
            textAccessionNo.Text = imagingStudy.GetIdentifier(codeSystemLocal);

            var condition = model.Composition.Contained.OfType<Condition>().FirstOrDefault();
            textCondition.Text = condition.Code.ToText();


            var models = new List<SeriesViewModel>();
            foreach (var series in imagingStudy.Series)
            {
                var seriesModel = new SeriesViewModel();
                seriesModel.Uid = series.Uid;
                seriesModel.BodySite = series.BodySite.ToText();
                seriesModel.Modality = series.Modality.ToText();
                seriesModel.Count = series.Instance.Count;
                seriesModel.Instances = series.Instance.Select(instance =>
                {
                    var instanceModel = new InstanceViewModel();
                    instanceModel.Uid = instance.Uid;
                    instanceModel.Text = instance.SopClass.ToText();
                    return instanceModel;
                }).ToArray();

                models.Add(seriesModel);
            }

            bindingSourceSeries.DataSource = models;
        }
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            var rowIndex = dgvSeries.SelectedCells.Count > 0 ? dgvSeries.SelectedCells[0].RowIndex : -1;
            bindingSourceInstance.DataSource = null;
            if (rowIndex > -1)
            {
                var model = dgvSeries.Rows[rowIndex].DataBoundItem as SeriesViewModel;
                bindingSourceInstance.DataSource = model.Instances;
                dgvInstance.AutoResizeColumns();
            }
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

        public class SeriesViewModel
        {
            [DisplayName("UID")]
            public string Uid { get; set; }
            [DisplayName("位置")]
            public string BodySite { get; set; }
            [DisplayName("Modality")]
            public string Modality { get; set; }
            [DisplayName("張數")]
            public int Count { get; set; }
            [Browsable(false)]
            public InstanceViewModel[] Instances { get; set; }
        }
        public class InstanceViewModel
        {
            [DisplayName("UID")]
            public string Uid { get; set; }
            [DisplayName("說明")]
            public string Text { get; set; }
        }
    }
}
