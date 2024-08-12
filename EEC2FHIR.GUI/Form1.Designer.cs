namespace EEC2FHIR.GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonTransfer = new System.Windows.Forms.Button();
            this.buttonPrettyXml = new System.Windows.Forms.Button();
            this.buttonPrettyJson = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Location = new System.Drawing.Point(12, 27);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Size = new System.Drawing.Size(873, 690);
            this.splitContainer.SplitterDistance = 436;
            this.splitContainer.TabIndex = 3;
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.Location = new System.Drawing.Point(891, 64);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(105, 31);
            this.buttonUpload.TabIndex = 3;
            this.buttonUpload.Text = "上傳";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // buttonTransfer
            // 
            this.buttonTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTransfer.Location = new System.Drawing.Point(891, 27);
            this.buttonTransfer.Name = "buttonTransfer";
            this.buttonTransfer.Size = new System.Drawing.Size(105, 31);
            this.buttonTransfer.TabIndex = 2;
            this.buttonTransfer.Text = "轉檔";
            this.buttonTransfer.UseVisualStyleBackColor = true;
            this.buttonTransfer.Visible = false;
            this.buttonTransfer.Click += new System.EventHandler(this.buttonTransfer_Click);
            // 
            // buttonPrettyXml
            // 
            this.buttonPrettyXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrettyXml.Location = new System.Drawing.Point(891, 686);
            this.buttonPrettyXml.Name = "buttonPrettyXml";
            this.buttonPrettyXml.Size = new System.Drawing.Size(105, 31);
            this.buttonPrettyXml.TabIndex = 4;
            this.buttonPrettyXml.Text = "PrettyXml";
            this.buttonPrettyXml.UseVisualStyleBackColor = true;
            this.buttonPrettyXml.Click += new System.EventHandler(this.buttonPrettyXml_Click);
            // 
            // buttonPrettyJson
            // 
            this.buttonPrettyJson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrettyJson.Location = new System.Drawing.Point(891, 649);
            this.buttonPrettyJson.Name = "buttonPrettyJson";
            this.buttonPrettyJson.Size = new System.Drawing.Size(105, 31);
            this.buttonPrettyJson.TabIndex = 5;
            this.buttonPrettyJson.Text = "PrettyJson";
            this.buttonPrettyJson.UseVisualStyleBackColor = true;
            this.buttonPrettyJson.Click += new System.EventHandler(this.buttonPrettyJson_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(43, 20);
            this.menuFile.Text = "檔案";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.Size = new System.Drawing.Size(122, 22);
            this.menuFileNew.Text = "新增空白";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size(122, 22);
            this.menuFileOpen.Text = "開啟...";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML|*.xml";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.buttonPrettyJson);
            this.Controls.Add(this.buttonPrettyXml);
            this.Controls.Add(this.buttonTransfer);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "CDAR2轉FHIR";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonTransfer;
        private System.Windows.Forms.Button buttonPrettyXml;
        private System.Windows.Forms.Button buttonPrettyJson;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
