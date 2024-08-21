namespace EECViewer
{
    partial class SearchFormBase
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupOrganization = new System.Windows.Forms.GroupBox();
            this.textOrganizationId = new System.Windows.Forms.TextBox();
            this.labelOrganizationId = new System.Windows.Forms.Label();
            this.groupPatient = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textPatientName = new System.Windows.Forms.TextBox();
            this.labelPatientName = new System.Windows.Forms.Label();
            this.textPatientId = new System.Windows.Forms.TextBox();
            this.labelPatientId = new System.Windows.Forms.Label();
            this.textPatientChtno = new System.Windows.Forms.TextBox();
            this.textPatientChtnoSystem = new System.Windows.Forms.TextBox();
            this.labelPatientChtno = new System.Windows.Forms.Label();
            this.groupDocument = new System.Windows.Forms.GroupBox();
            this.textDocumentTimeEnd = new System.Windows.Forms.TextBox();
            this.textDocumentTimeBegin = new System.Windows.Forms.TextBox();
            this.labelDocumentTimeRange = new System.Windows.Forms.Label();
            this.textDocumentTime = new System.Windows.Forms.TextBox();
            this.labelDocumentTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLog = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupOrganization.SuspendLayout();
            this.groupPatient.SuspendLayout();
            this.groupDocument.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvData);
            this.splitContainer1.Size = new System.Drawing.Size(801, 450);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupOrganization);
            this.panel1.Controls.Add(this.groupPatient);
            this.panel1.Controls.Add(this.groupDocument);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 398);
            this.panel1.TabIndex = 5;
            // 
            // groupOrganization
            // 
            this.groupOrganization.Controls.Add(this.textOrganizationId);
            this.groupOrganization.Controls.Add(this.labelOrganizationId);
            this.groupOrganization.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupOrganization.Location = new System.Drawing.Point(0, 383);
            this.groupOrganization.Name = "groupOrganization";
            this.groupOrganization.Size = new System.Drawing.Size(225, 89);
            this.groupOrganization.TabIndex = 1;
            this.groupOrganization.TabStop = false;
            this.groupOrganization.Text = "醫療機構";
            // 
            // textOrganizationId
            // 
            this.textOrganizationId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOrganizationId.Location = new System.Drawing.Point(13, 48);
            this.textOrganizationId.Name = "textOrganizationId";
            this.textOrganizationId.Size = new System.Drawing.Size(187, 29);
            this.textOrganizationId.TabIndex = 1;
            this.textOrganizationId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelOrganizationId
            // 
            this.labelOrganizationId.AutoSize = true;
            this.labelOrganizationId.Location = new System.Drawing.Point(9, 25);
            this.labelOrganizationId.Name = "labelOrganizationId";
            this.labelOrganizationId.Size = new System.Drawing.Size(73, 20);
            this.labelOrganizationId.TabIndex = 0;
            this.labelOrganizationId.Text = "機構代號";
            // 
            // groupPatient
            // 
            this.groupPatient.Controls.Add(this.label1);
            this.groupPatient.Controls.Add(this.textPatientName);
            this.groupPatient.Controls.Add(this.labelPatientName);
            this.groupPatient.Controls.Add(this.textPatientId);
            this.groupPatient.Controls.Add(this.labelPatientId);
            this.groupPatient.Controls.Add(this.textPatientChtno);
            this.groupPatient.Controls.Add(this.textPatientChtnoSystem);
            this.groupPatient.Controls.Add(this.labelPatientChtno);
            this.groupPatient.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPatient.Location = new System.Drawing.Point(0, 186);
            this.groupPatient.Name = "groupPatient";
            this.groupPatient.Size = new System.Drawing.Size(225, 197);
            this.groupPatient.TabIndex = 0;
            this.groupPatient.TabStop = false;
            this.groupPatient.Text = "病患";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "／";
            // 
            // textPatientName
            // 
            this.textPatientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatientName.Location = new System.Drawing.Point(13, 158);
            this.textPatientName.Name = "textPatientName";
            this.textPatientName.Size = new System.Drawing.Size(187, 29);
            this.textPatientName.TabIndex = 7;
            this.textPatientName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelPatientName
            // 
            this.labelPatientName.AutoSize = true;
            this.labelPatientName.Location = new System.Drawing.Point(9, 135);
            this.labelPatientName.Name = "labelPatientName";
            this.labelPatientName.Size = new System.Drawing.Size(41, 20);
            this.labelPatientName.TabIndex = 6;
            this.labelPatientName.Text = "姓名";
            // 
            // textPatientId
            // 
            this.textPatientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatientId.Location = new System.Drawing.Point(13, 103);
            this.textPatientId.Name = "textPatientId";
            this.textPatientId.Size = new System.Drawing.Size(187, 29);
            this.textPatientId.TabIndex = 5;
            this.textPatientId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelPatientId
            // 
            this.labelPatientId.AutoSize = true;
            this.labelPatientId.Location = new System.Drawing.Point(9, 80);
            this.labelPatientId.Name = "labelPatientId";
            this.labelPatientId.Size = new System.Drawing.Size(73, 20);
            this.labelPatientId.TabIndex = 4;
            this.labelPatientId.Text = "身分證號";
            // 
            // textPatientChtno
            // 
            this.textPatientChtno.Location = new System.Drawing.Point(13, 48);
            this.textPatientChtno.Name = "textPatientChtno";
            this.textPatientChtno.Size = new System.Drawing.Size(89, 29);
            this.textPatientChtno.TabIndex = 3;
            this.textPatientChtno.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // textPatientChtnoSystem
            // 
            this.textPatientChtnoSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatientChtnoSystem.Location = new System.Drawing.Point(139, 48);
            this.textPatientChtnoSystem.Name = "textPatientChtnoSystem";
            this.textPatientChtnoSystem.Size = new System.Drawing.Size(60, 29);
            this.textPatientChtnoSystem.TabIndex = 1;
            this.textPatientChtnoSystem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelPatientChtno
            // 
            this.labelPatientChtno.AutoSize = true;
            this.labelPatientChtno.Location = new System.Drawing.Point(9, 25);
            this.labelPatientChtno.Name = "labelPatientChtno";
            this.labelPatientChtno.Size = new System.Drawing.Size(105, 20);
            this.labelPatientChtno.TabIndex = 0;
            this.labelPatientChtno.Text = "病歷號／機構";
            // 
            // groupDocument
            // 
            this.groupDocument.Controls.Add(this.textDocumentTimeEnd);
            this.groupDocument.Controls.Add(this.textDocumentTimeBegin);
            this.groupDocument.Controls.Add(this.labelDocumentTimeRange);
            this.groupDocument.Controls.Add(this.textDocumentTime);
            this.groupDocument.Controls.Add(this.labelDocumentTime);
            this.groupDocument.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDocument.Location = new System.Drawing.Point(0, 0);
            this.groupDocument.Name = "groupDocument";
            this.groupDocument.Size = new System.Drawing.Size(225, 186);
            this.groupDocument.TabIndex = 2;
            this.groupDocument.TabStop = false;
            this.groupDocument.Text = "文件摘要";
            // 
            // textDocumentTimeEnd
            // 
            this.textDocumentTimeEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDocumentTimeEnd.Location = new System.Drawing.Point(12, 147);
            this.textDocumentTimeEnd.Name = "textDocumentTimeEnd";
            this.textDocumentTimeEnd.Size = new System.Drawing.Size(187, 29);
            this.textDocumentTimeEnd.TabIndex = 4;
            this.textDocumentTimeEnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // textDocumentTimeBegin
            // 
            this.textDocumentTimeBegin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDocumentTimeBegin.Location = new System.Drawing.Point(12, 112);
            this.textDocumentTimeBegin.Name = "textDocumentTimeBegin";
            this.textDocumentTimeBegin.Size = new System.Drawing.Size(187, 29);
            this.textDocumentTimeBegin.TabIndex = 3;
            this.textDocumentTimeBegin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelDocumentTimeRange
            // 
            this.labelDocumentTimeRange.AutoSize = true;
            this.labelDocumentTimeRange.Location = new System.Drawing.Point(8, 89);
            this.labelDocumentTimeRange.Name = "labelDocumentTimeRange";
            this.labelDocumentTimeRange.Size = new System.Drawing.Size(105, 20);
            this.labelDocumentTimeRange.TabIndex = 2;
            this.labelDocumentTimeRange.Text = "產生日期起訖";
            // 
            // textDocumentTime
            // 
            this.textDocumentTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDocumentTime.Location = new System.Drawing.Point(13, 48);
            this.textDocumentTime.Name = "textDocumentTime";
            this.textDocumentTime.Size = new System.Drawing.Size(187, 29);
            this.textDocumentTime.TabIndex = 1;
            this.textDocumentTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // labelDocumentTime
            // 
            this.labelDocumentTime.AutoSize = true;
            this.labelDocumentTime.Location = new System.Drawing.Point(9, 25);
            this.labelDocumentTime.Name = "labelDocumentTime";
            this.labelDocumentTime.Size = new System.Drawing.Size(73, 20);
            this.labelDocumentTime.TabIndex = 0;
            this.labelDocumentTime.Text = "產生日期";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.buttonLog, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSearch, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(242, 52);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // buttonLog
            // 
            this.buttonLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLog.Location = new System.Drawing.Point(172, 3);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(67, 46);
            this.buttonLog.TabIndex = 4;
            this.buttonLog.Text = "紀錄";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.Location = new System.Drawing.Point(3, 3);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(163, 46);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "查詢";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AutoGenerateColumns = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.DataSource = this.bindingSource;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvData.Size = new System.Drawing.Size(555, 450);
            this.dgvData.TabIndex = 0;
            this.dgvData.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentDoubleClick);
            // 
            // SearchFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SearchFormBase";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupOrganization.ResumeLayout(false);
            this.groupOrganization.PerformLayout();
            this.groupPatient.ResumeLayout(false);
            this.groupPatient.PerformLayout();
            this.groupDocument.ResumeLayout(false);
            this.groupDocument.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupOrganization;
        private System.Windows.Forms.TextBox textOrganizationId;
        private System.Windows.Forms.Label labelOrganizationId;
        private System.Windows.Forms.GroupBox groupPatient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPatientName;
        private System.Windows.Forms.Label labelPatientName;
        private System.Windows.Forms.TextBox textPatientId;
        private System.Windows.Forms.Label labelPatientId;
        private System.Windows.Forms.TextBox textPatientChtno;
        private System.Windows.Forms.TextBox textPatientChtnoSystem;
        private System.Windows.Forms.Label labelPatientChtno;
        private System.Windows.Forms.GroupBox groupDocument;
        private System.Windows.Forms.TextBox textDocumentTimeEnd;
        private System.Windows.Forms.TextBox textDocumentTimeBegin;
        private System.Windows.Forms.Label labelDocumentTimeRange;
        private System.Windows.Forms.TextBox textDocumentTime;
        private System.Windows.Forms.Label labelDocumentTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}