namespace EECViewer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuQry = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQryLab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQryImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuQry,
            this.menuExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuQry
            // 
            this.menuQry.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuQryLab,
            this.menuQryImage});
            this.menuQry.Name = "menuQry";
            this.menuQry.Size = new System.Drawing.Size(43, 20);
            this.menuQry.Text = "調閱";
            // 
            // menuQryLab
            // 
            this.menuQryLab.Name = "menuQryLab";
            this.menuQryLab.Size = new System.Drawing.Size(180, 22);
            this.menuQryLab.Text = "檢查檢驗報告";
            this.menuQryLab.Click += new System.EventHandler(this.menuQryLab_Click);
            // 
            // menuQryImage
            // 
            this.menuQryImage.Name = "menuQryImage";
            this.menuQryImage.Size = new System.Drawing.Size(180, 22);
            this.menuQryImage.Text = "影像報告";
            this.menuQryImage.Click += new System.EventHandler(this.menuQryImage_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(43, 20);
            this.menuExit.Text = "離開";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "電子病歷調閱 (FHIR)";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuQry;
        private System.Windows.Forms.ToolStripMenuItem menuQryLab;
        private System.Windows.Forms.ToolStripMenuItem menuQryImage;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
    }
}