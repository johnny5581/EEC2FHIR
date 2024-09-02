using FhirConn.Utility;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EECViewer
{
    public partial class SearchFormBase : FormBase, IHttpMessageHandlerCallback
    {
        protected readonly FhirClient client;
        protected TransactionHistory history;
        protected TransactionHistoryForm subForm;
        public SearchFormBase()
        {
            InitializeComponent();
            var server = ConfigurationManager.AppSettings["fhir.server"];
            var tokenServer = ConfigurationManager.AppSettings["fhir.token.server"];
            var clientId = ConfigurationManager.AppSettings["fhir.token.client.id"];
            var clientSecret = ConfigurationManager.AppSettings["fhir.token.client.secret"];
            client = new FhirClient(server, new FhirClientSettings { PreferredFormat = ResourceFormat.Json }, messageHandler: new FhirConn.Utility.HttpBearerTokenHandler(this, tokenServer, clientId, clientSecret));


            dgvData.AutoGenerateColumns = true;
        }
        public virtual string DocumentType
        {
            get
            {
                if (DocumentTypeValue == null)
                    throw new NullReferenceException("缺少要查詢的文件類別");
                return DocumentTypeValue;
            }
        }
        protected string DocumentTypeValue { get; set; }

        public void SaveRequest(string guid, string method, string uri, string requestText)
        {
            history.Requests.Add(new RequestHistory(guid, method, uri)
            {
                Request = requestText
            });
        }

        public void SaveResponse(string guid, string responseText)
        {
            history.Requests[guid].Response = responseText;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                bindingSource.DataSource = null;

                // 準備查詢參數
                var querier = new FhirResourceQuerier<Bundle>(client);
                var searchParams = new SearchParams();

                // 基礎: 類別為檢驗報告 (11503-0)
                searchParams.Add("type", DocumentType);

                // 日期，當日或區間
                if (!string.IsNullOrEmpty(textDocumentTime.Text))
                {
                    searchParams.AddDate("date", textDocumentTime.Text, "文件產生日期");
                }
                else if (!string.IsNullOrEmpty(textDocumentTimeBegin.Text) || !string.IsNullOrEmpty(textDocumentTimeEnd.Text))
                {
                    searchParams.AddDateRange("date", textDocumentTimeBegin.Text, textDocumentTimeEnd.Text, "文件產生日期(起迄)");
                }

                // 病歷號
                if (!string.IsNullOrEmpty(textPatientChtno.Text))
                {
                    var chtno = textPatientChtno.Text;
                    var system = textPatientChtnoSystem.Text;
                    if (string.IsNullOrEmpty(system))
                        throw new Exception("病歷號查詢必須要搭配醫療院所系統代碼(SystemCode)");

                    searchParams.Add("patient.identifier", $"{system}|{chtno}");
                }

                // 身分證號
                if (!string.IsNullOrEmpty(textPatientId.Text))
                {
                    searchParams.Add("patient.identifier", $"{TWPatient.CodeSystemTwIdentifier}|{textPatientId.Text}");
                }

                // 姓名
                if (!string.IsNullOrEmpty(textPatientName.Text))
                {
                    searchParams.Add("patient.name", textPatientName.Text);
                }

                // 組織
                if (!string.IsNullOrEmpty(textOrganizationId.Text))
                {
                    // 查不出來，先直接用代碼查詢
                    //searchParams.Add("patient.organization.identifier", $"{TWOrganization.CodeSystemTwIdentifier}|{textOrganizationId.Text}");
                    searchParams.Add("patient.organization.identifier", textOrganizationId.Text);
                }


                if (searchParams.Parameters.Count <= 1)
                    throw new Exception("至少要有一個查詢條件");


                // 查詢資料
                history = new TransactionHistory(Guid.NewGuid().ToString());
                try
                {
                    var bundle = client.Search<Composition>(searchParams);
                    if (bundle == null || bundle.Entry == null || bundle.Entry.Count == 0)
                    {
                        MessageBox.Show("查無資料");
                        return;
                    }

                    var models = new List<ViewModel>();
                    foreach (var entry in bundle.Entry)
                    {
                        var composition = entry.Resource as Composition;
                        var model = ConvertToViewModel(composition);
                        models.Add(model);
                    }
                    bindingSource.DataSource = models;
                    dgvData.AutoResizeColumns();
                }
                finally
                {
                    subForm?.LoadData(history);
                }
            });
        }

        protected virtual ViewModel ConvertToViewModel(Composition composition)
        {
            var model = new ViewModel();
            model.Data.Add("root", composition);

            model.Id = composition.Id;

            // 讀取病人資料
            var pat = client.Read<Patient>(composition.Subject.Reference);
            model.PatId = pat.GetTwIdentifier();
            model.PatChtNo = pat.GetIdentifier(codeSystemLocal, true);
            model.PatName = pat.Name.ToText();
            model.PatGender = Convert.ToString(pat.Gender);
            model.Data.Add(composition.Subject.Reference, pat);

            // 讀取機構資料
            if (composition.Custodian != null)
            {
                var org = client.Read<Organization>(composition.Custodian.Reference);
                model.Org = $"{org.Name} ({org.GetTwIdentifier()})";
                model.Data.Add(composition.Custodian.Reference, org);
            }

            // 報告作者
            if (composition.Author.Count > 1)
            {
                var author = client.Read<Practitioner>(composition.Author[1].Reference);
                model.Author = author.Name.ToText();
                model.Data.Add(composition.Author[1].Reference, author);
            }

            // 讀取開單資料
            var encounter = client.Read<Encounter>(composition.Encounter.Reference);
            model.OpdNo = encounter.GetIdentifier(codeSystemLocal, true);
            model.Data.Add(composition.Encounter.Reference, encounter);

            // 開單醫師
            var encPratitioner = client.Read<Practitioner>(encounter.Participant[0].Individual.Reference);
            model.OdrDr = encPratitioner.Name.ToText();
            model.Data.Add(encounter.Participant[0].Individual.Reference, encPratitioner);

            model.Title = composition.Title;
            model.Date = DateTimeOffset.Parse(composition.Date).ToString("yyyy-MM-dd HH:mm");
            model.Status = Convert.ToString(composition.Status);

            return model;
        }
        private void buttonLog_Click(object sender, EventArgs e)
        {
            if (subForm == null)
                subForm = new TransactionHistoryForm();

            subForm.LoadData(history);
            subForm.Show();
            subForm.BringToFront();
        }

        private void dgvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = e.RowIndex;
            if (index > -1)
            {
                var item = dgvData.Rows[index].DataBoundItem as ViewModel;
                if (item != null)
                {
                    ShowDetailView(item);                    
                }
            }
        }

        protected virtual void ShowDetailView(ViewModel model)
        {
            throw new NotImplementedException();
        }

        public class ViewModel
        {
            [DisplayName("#")]
            public string Id { get; set; }
            [DisplayName("身分證號")]
            public string PatId { get; set; }
            [DisplayName("病歷號")]
            public string PatChtNo { get; set; }
            [DisplayName("姓名")]
            public string PatName { get; set; }
            [DisplayName("性別")]
            public string PatGender { get; set; }
            [DisplayName("機構")]
            public string Org { get; set; }
            [DisplayName("報告標題")]
            public string Title { get; set; }
            [DisplayName("報告作者")]
            public string Author { get; set; }
            [DisplayName("報告時間")]
            public string Date { get; set; }
            [DisplayName("報告狀態")]
            public string Status { get; set; }
            [DisplayName("就診號")]
            public string OpdNo { get; set; }
            [DisplayName("開單醫師")]
            public string OdrDr { get; set; }

            [Browsable(false)]
            public Dictionary<string, object> Data { get; } = new Dictionary<string, object>();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSearch.PerformClick();
        }
    }

    public interface IDetailView
    {
        void LoadData(object data);
        DialogResult ShowDialog();
    }

    
}
