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

namespace EECViewer
{
    public partial class MainForm : FormBase
    {
        private readonly string fixText;
        public MainForm()
        {
            InitializeComponent();
            fixText = Text;
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuQryLab_Click(object sender, EventArgs e)
        {
            ShowMdiChild(typeof(Laboratory.SearchForm));
        }

        private void menuQryImage_Click(object sender, EventArgs e)
        {
            ShowMdiChild(typeof(ImageReport.SearchForm));
        }


        private void ShowMdiChild(Type formType)
        {
            foreach (var child in MdiChildren)
            {
                if (child.GetType() == formType)
                {
                    child.Activate();
                    return;
                }
            }

            var form = (Form)Activator.CreateInstance(formType);
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }
    }
}
