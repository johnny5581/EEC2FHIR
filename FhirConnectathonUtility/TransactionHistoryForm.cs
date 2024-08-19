using Newtonsoft.Json.Linq;
using ScintillaNET;
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

namespace FhirConn.Utility
{
    public partial class TransactionHistoryForm : Form
    {
        private BindingSource bindingSource;

        private Scintilla textRequest;
        private Scintilla textResponse;

        private readonly string fixText;
        public TransactionHistoryForm()
        {
            InitializeComponent();
            bindingSource = new BindingSource();
            listHistory.DataSource = bindingSource;

            textRequest = new Scintilla();
            textRequest.Dock = DockStyle.Fill;
            textRequest.Lexer = Lexer.Json;
            InitializeTextArea(textRequest);
            splitContainer.Panel1.Controls.Add(textRequest);

            textResponse = new Scintilla();
            textResponse.Dock = DockStyle.Fill;
            textResponse.Lexer = Lexer.Json;
            InitializeTextArea(textResponse);
            splitContainer.Panel2.Controls.Add(textResponse);

            fixText = Text;
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


        public void LoadData(TransactionHistory model)
        {
            if (model != null)
            {
                bindingSource.DataSource = model.Requests;
                Text = $"{fixText} - {model.Token}";
                listHistory.SelectedIndex = -1;
                if (listHistory.Items.Count > 0)
                    listHistory.SelectedIndex = 0;
            }
        }

        private void listHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = listHistory.SelectedIndex;
            textRequest.Clear();
            textResponse.Clear();
            if (index > -1)
            {
                var item = bindingSource[index] as RequestHistory;

                textRequest.Text = item.Request;
                PrettyJson(textRequest);

                textResponse.Text = item.Response;
                PrettyJson(textResponse);
            }            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                Hide();
            }
        }
        private void PrettyJson(Scintilla scintilla)
        {
            var json = scintilla.Text;
            try
            {
                scintilla.Text = JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch { }
        }
    }
}
