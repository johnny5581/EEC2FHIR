﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EECViewer
{
    public class FormBase : Form
    {
        public FormBase()
        {
            Font = new Font("微軟正黑體", 12f);
        }

        protected void Execute(Action action)
        {
            try
            {
                action();
            }
            catch(TargetInvocationException ex)
            {
                MessageBox.Show(ex.InnerException.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
