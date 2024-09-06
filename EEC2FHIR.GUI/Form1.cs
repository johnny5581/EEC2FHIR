using EEC2FHIR.Utility;
using FhirConn.Utility;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EEC2FHIR.GUI
{
    public partial class Form1 : Form, FhirConn.Utility.IHttpMessageHandlerCallback
    {
        private Scintilla textAreaXml;
        private Scintilla textAreaJson;
        private int maxLineNumberCharLengthXml;
        private int maxLineNumberCharLengthJson;
        private FhirClient client;
        private readonly List<TransactionHistory> Histories
            = new List<TransactionHistory>();
        private TransactionHistory currentScope;
        private BindingSource bindingSource;
        private TransactionHistoryForm subForm;
        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponent();
            bindingSource = new BindingSource();
            bindingSource.DataSource = Histories;
            listHistory.DataSource = bindingSource;
            var server = ConfigurationManager.AppSettings["fhir.server"];
            var tokenServer = ConfigurationManager.AppSettings["fhir.token.server"];
            var clientId = ConfigurationManager.AppSettings["fhir.token.client.id"];
            var clientSecret = ConfigurationManager.AppSettings["fhir.token.client.secret"];

            var tokenType = ConfigurationManager.AppSettings["fhir.token.type"];
            HttpMessageHandler msgHandler = null;
            if (tokenType == "oauth")
                msgHandler = new FhirConn.Utility.OAuth2BearerTokenHandler(this, tokenServer, clientId, clientSecret);
            else
                msgHandler = new FhirConn.Utility.HttpBearerTokenHandler(this, tokenServer, clientId, clientSecret);
            client = new FhirClient(server, new FhirClientSettings { PreferredFormat = ResourceFormat.Json }, messageHandler: msgHandler);

            subForm = new TransactionHistoryForm();
        }

        private void InitializeCustomComponent()
        {
            textAreaXml = new Scintilla();
            InitializeTextArea(textAreaXml);
            textAreaXml.Lexer = Lexer.Xml;
            textAreaXml.Dock = DockStyle.Fill;
            splitContainer.Panel1.Controls.Add(textAreaXml);

            textAreaJson = new Scintilla();
            InitializeTextArea(textAreaJson);
            textAreaJson.Lexer = Lexer.Json;
            textAreaJson.Dock = DockStyle.Fill;
            splitContainer.Panel2.Controls.Add(textAreaJson);
        }
        private void InitializeTextArea(Scintilla textArea)
        {
            textArea.Styles[Style.Default].Font = "Consolas";
            textArea.Styles[Style.Default].Size = 10;

            textArea.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
            textArea.Styles[Style.Xml.Attribute].ForeColor = Color.FromArgb(-39366);
            textArea.Styles[Style.Xml.DoubleString].ForeColor = Color.Purple;
            textArea.Styles[Style.Xml.SingleString].ForeColor = Color.Purple;
            textArea.Styles[Style.Xml.Comment].ForeColor = Color.Green;

            textArea.Styles[Style.Json.Number].ForeColor = Color.Blue;
            textArea.Styles[Style.Json.String].ForeColor = Color.Brown;
            textArea.Styles[Style.Json.StringEol].ForeColor = Color.Purple;
            textArea.Styles[Style.Json.LineComment].ForeColor = Color.Green;
            textArea.Styles[Style.Json.BlockComment].ForeColor = Color.Green;

            textArea.Margins[0].Width = 20;
        }

        private void PrettyXml()
        {
            var xml = textAreaXml.Text;
            try
            {
                textAreaXml.Text = XDocument.Parse(xml).ToString();
            }
            catch { }
        }
        private void PrettyJson()
        {
            var json = textAreaJson.Text;
            try
            {
                textAreaJson.Text = JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch { }
        }


        private void buttonPrettyXml_Click(object sender, EventArgs e)
        {
            PrettyXml();
        }

        private void buttonPrettyJson_Click(object sender, EventArgs e)
        {
            PrettyJson();
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {

            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                Execute(() =>
                {
                    using (var fs = File.OpenText(openFileDialog1.FileName))
                    {
                        var doc = XDocument.Load(fs);
                        textAreaXml.Text = doc.ToString();
                    }
                });

            }
        }
        private void menuUpload_Click(object sender, EventArgs e)
        {
            currentScope = new TransactionHistory(Guid.NewGuid().ToString());
            bindingSource.Add(currentScope);
            //Histories.Add(currentScope);
            Execute(() =>
            {
                var xml = textAreaXml.Text;
                ParserBase parser = CreateParserBase(xml);
                parser.SystemCodeLocal = ConfigurationManager.AppSettings["fhir.codesystem.local"];
                parser.SystemCodeGlobal = ConfigurationManager.AppSettings["fhir.codesystem.global"];
                var bundle = parser.Parse(xml);
                var json = new FhirJsonSerializer().SerializeToString(bundle);
                textAreaJson.Text = json;
                PrettyJson();
                client.Create(bundle);
            });
            currentScope = null;
        }

        private ParserBase CreateParserBase(string xml)
        {
            // 決定xml類型要用哪種Parser
            var doc = XDocument.Parse(xml);

            var docType = GetCdaR2DocumentType(doc);
            switch(docType)
            {
                case "18782-3": // 影像報告
                    return new ImageReport.Parser(client);
                case "11502-2": // 檢驗檢查報告
                    return new Laboratory.Parser(client);
                default:
                    throw new NotSupportedException("不支援的CDAR2文件類型: " + docType);
            }
        }

        private string GetCdaR2DocumentType(XDocument doc)
        {
            var nsMgr = doc.CreateCdaR2NamespaceManager();
            var code = doc.Root.XPathEvaluateString("/cdp:ContentPackage/cdp:ContentContainer/cdp:StructuredContent/ns:ClinicalDocument/ns:code/@code", nsMgr);
            return code;
        }


        private void Execute(Action action, Action<Exception> errorHandler = null)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException ex)
            {
                errorHandler?.Invoke(ex.InnerException);
                MessageBox.Show(ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveRequest(string guid, string url, string method, string request)
        {
            if (currentScope == null) throw new NullReferenceException("missing Upload history");
            var history = new RequestHistory(guid, method, url)
            {
                Request = request
            };
            currentScope.Requests.Add(history);
        }
        public void SaveResponse(string guid, string response)
        {
            if (currentScope == null) throw new NullReferenceException("missing Upload history");
            currentScope.Requests[guid].Response = response;
        }
        private void listHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Execute(() =>
            {
                var item = listHistory.SelectedItem as TransactionHistory;
                if (item == null)
                    throw new Exception("沒有選取紀錄");
                subForm.LoadData(item);
                if (subForm.Visible)
                    subForm.BringToFront();
                else
                    subForm.Show();
            });
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            bindingSource.Clear();
        }
        

    }

    


}
