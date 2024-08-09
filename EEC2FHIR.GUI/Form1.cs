using EEC2FHIR.GUI.Forms;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace EEC2FHIR.GUI
{
    public partial class Form1 : CgForm
    {
        private Scintilla textAreaXml;
        private Scintilla textAreaJson;
        private int maxLineNumberCharLengthXml;
        private int maxLineNumberCharLengthJson;
        private FhirClient client;
        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponent();
            client = new FhirClient(ConfigurationManager.AppSettings["fhir.server"]);
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

            textArea.Margins[0].Width = 10;
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

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var xml = textAreaXml.Text;                
                var parser = new Laboratory.Parser(client);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var bundle = parser.Parse(xmlDoc);
                var json = new FhirJsonSerializer().SerializeToString(bundle);
                textAreaJson.Text = json;
                PrettyJson();
            });
        }
    }
}
