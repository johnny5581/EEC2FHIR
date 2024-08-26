namespace EECViewer.Laboratory
{
    partial class DetailForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupEncounter = new System.Windows.Forms.GroupBox();
            this.textOdrDate = new System.Windows.Forms.TextBox();
            this.labelOdrDate = new System.Windows.Forms.Label();
            this.textOdrDr = new System.Windows.Forms.TextBox();
            this.labelOdrDr = new System.Windows.Forms.Label();
            this.textOdrId = new System.Windows.Forms.TextBox();
            this.labelOdrId = new System.Windows.Forms.Label();
            this.groupOrganization = new System.Windows.Forms.GroupBox();
            this.textOrgName = new System.Windows.Forms.TextBox();
            this.labelOrgName = new System.Windows.Forms.Label();
            this.textOrgId = new System.Windows.Forms.TextBox();
            this.labelOrgId = new System.Windows.Forms.Label();
            this.groupPatient = new System.Windows.Forms.GroupBox();
            this.textPatGender = new System.Windows.Forms.TextBox();
            this.labelPatGender = new System.Windows.Forms.Label();
            this.textPatBirthDate = new System.Windows.Forms.TextBox();
            this.labelPatBirthDate = new System.Windows.Forms.Label();
            this.textPatName = new System.Windows.Forms.TextBox();
            this.labelPatName = new System.Windows.Forms.Label();
            this.textPatId = new System.Windows.Forms.TextBox();
            this.labelPatId = new System.Windows.Forms.Label();
            this.textPatChtno = new System.Windows.Forms.TextBox();
            this.labelPatChtno = new System.Windows.Forms.Label();
            this.groupDocument = new System.Windows.Forms.GroupBox();
            this.textDocDate = new System.Windows.Forms.TextBox();
            this.labelDocDate = new System.Windows.Forms.Label();
            this.textDocId = new System.Windows.Forms.TextBox();
            this.labelDocId = new System.Windows.Forms.Label();
            this.groupLabResult = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.bindingSourceDetail = new System.Windows.Forms.BindingSource(this.components);
            this.textDocAuthor = new System.Windows.Forms.TextBox();
            this.labelDocAuthor = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupEncounter.SuspendLayout();
            this.groupOrganization.SuspendLayout();
            this.groupPatient.SuspendLayout();
            this.groupDocument.SuspendLayout();
            this.groupLabResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(826, 437);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.groupEncounter);
            this.splitContainer1.Panel1.Controls.Add(this.groupOrganization);
            this.splitContainer1.Panel1.Controls.Add(this.groupPatient);
            this.splitContainer1.Panel1.Controls.Add(this.groupDocument);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupLabResult);
            this.splitContainer1.Size = new System.Drawing.Size(826, 437);
            this.splitContainer1.SplitterDistance = 275;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupEncounter
            // 
            this.groupEncounter.Controls.Add(this.textOdrDate);
            this.groupEncounter.Controls.Add(this.labelOdrDate);
            this.groupEncounter.Controls.Add(this.textOdrDr);
            this.groupEncounter.Controls.Add(this.labelOdrDr);
            this.groupEncounter.Controls.Add(this.textOdrId);
            this.groupEncounter.Controls.Add(this.labelOdrId);
            this.groupEncounter.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupEncounter.Location = new System.Drawing.Point(0, 455);
            this.groupEncounter.Margin = new System.Windows.Forms.Padding(5);
            this.groupEncounter.Name = "groupEncounter";
            this.groupEncounter.Padding = new System.Windows.Forms.Padding(5);
            this.groupEncounter.Size = new System.Drawing.Size(258, 136);
            this.groupEncounter.TabIndex = 3;
            this.groupEncounter.TabStop = false;
            this.groupEncounter.Text = "就診資訊";
            // 
            // textOdrDate
            // 
            this.textOdrDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOdrDate.BackColor = System.Drawing.SystemColors.Window;
            this.textOdrDate.Location = new System.Drawing.Point(103, 96);
            this.textOdrDate.Name = "textOdrDate";
            this.textOdrDate.ReadOnly = true;
            this.textOdrDate.Size = new System.Drawing.Size(147, 29);
            this.textOdrDate.TabIndex = 5;
            // 
            // labelOdrDate
            // 
            this.labelOdrDate.Location = new System.Drawing.Point(8, 97);
            this.labelOdrDate.Name = "labelOdrDate";
            this.labelOdrDate.Size = new System.Drawing.Size(89, 26);
            this.labelOdrDate.TabIndex = 4;
            this.labelOdrDate.Text = "開單時間：";
            this.labelOdrDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textOdrDr
            // 
            this.textOdrDr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOdrDr.BackColor = System.Drawing.SystemColors.Window;
            this.textOdrDr.Location = new System.Drawing.Point(103, 61);
            this.textOdrDr.Name = "textOdrDr";
            this.textOdrDr.ReadOnly = true;
            this.textOdrDr.Size = new System.Drawing.Size(147, 29);
            this.textOdrDr.TabIndex = 3;
            // 
            // labelOdrDr
            // 
            this.labelOdrDr.Location = new System.Drawing.Point(8, 62);
            this.labelOdrDr.Name = "labelOdrDr";
            this.labelOdrDr.Size = new System.Drawing.Size(89, 26);
            this.labelOdrDr.TabIndex = 2;
            this.labelOdrDr.Text = "開單醫師：";
            this.labelOdrDr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textOdrId
            // 
            this.textOdrId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOdrId.BackColor = System.Drawing.SystemColors.Window;
            this.textOdrId.Location = new System.Drawing.Point(103, 26);
            this.textOdrId.Name = "textOdrId";
            this.textOdrId.ReadOnly = true;
            this.textOdrId.Size = new System.Drawing.Size(147, 29);
            this.textOdrId.TabIndex = 1;
            // 
            // labelOdrId
            // 
            this.labelOdrId.Location = new System.Drawing.Point(8, 27);
            this.labelOdrId.Name = "labelOdrId";
            this.labelOdrId.Size = new System.Drawing.Size(89, 26);
            this.labelOdrId.TabIndex = 0;
            this.labelOdrId.Text = "就診號：";
            this.labelOdrId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupOrganization
            // 
            this.groupOrganization.Controls.Add(this.textOrgName);
            this.groupOrganization.Controls.Add(this.labelOrgName);
            this.groupOrganization.Controls.Add(this.textOrgId);
            this.groupOrganization.Controls.Add(this.labelOrgId);
            this.groupOrganization.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupOrganization.Location = new System.Drawing.Point(0, 357);
            this.groupOrganization.Margin = new System.Windows.Forms.Padding(5);
            this.groupOrganization.Name = "groupOrganization";
            this.groupOrganization.Padding = new System.Windows.Forms.Padding(5);
            this.groupOrganization.Size = new System.Drawing.Size(258, 98);
            this.groupOrganization.TabIndex = 2;
            this.groupOrganization.TabStop = false;
            this.groupOrganization.Text = "機構";
            // 
            // textOrgName
            // 
            this.textOrgName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOrgName.BackColor = System.Drawing.SystemColors.Window;
            this.textOrgName.Location = new System.Drawing.Point(103, 61);
            this.textOrgName.Name = "textOrgName";
            this.textOrgName.ReadOnly = true;
            this.textOrgName.Size = new System.Drawing.Size(147, 29);
            this.textOrgName.TabIndex = 3;
            // 
            // labelOrgName
            // 
            this.labelOrgName.Location = new System.Drawing.Point(8, 62);
            this.labelOrgName.Name = "labelOrgName";
            this.labelOrgName.Size = new System.Drawing.Size(89, 26);
            this.labelOrgName.TabIndex = 2;
            this.labelOrgName.Text = "名稱：";
            this.labelOrgName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textOrgId
            // 
            this.textOrgId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOrgId.BackColor = System.Drawing.SystemColors.Window;
            this.textOrgId.Location = new System.Drawing.Point(103, 26);
            this.textOrgId.Name = "textOrgId";
            this.textOrgId.ReadOnly = true;
            this.textOrgId.Size = new System.Drawing.Size(147, 29);
            this.textOrgId.TabIndex = 1;
            // 
            // labelOrgId
            // 
            this.labelOrgId.Location = new System.Drawing.Point(8, 27);
            this.labelOrgId.Name = "labelOrgId";
            this.labelOrgId.Size = new System.Drawing.Size(89, 26);
            this.labelOrgId.TabIndex = 0;
            this.labelOrgId.Text = "代號：";
            this.labelOrgId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupPatient
            // 
            this.groupPatient.Controls.Add(this.textPatGender);
            this.groupPatient.Controls.Add(this.labelPatGender);
            this.groupPatient.Controls.Add(this.textPatBirthDate);
            this.groupPatient.Controls.Add(this.labelPatBirthDate);
            this.groupPatient.Controls.Add(this.textPatName);
            this.groupPatient.Controls.Add(this.labelPatName);
            this.groupPatient.Controls.Add(this.textPatId);
            this.groupPatient.Controls.Add(this.labelPatId);
            this.groupPatient.Controls.Add(this.textPatChtno);
            this.groupPatient.Controls.Add(this.labelPatChtno);
            this.groupPatient.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPatient.Location = new System.Drawing.Point(0, 143);
            this.groupPatient.Margin = new System.Windows.Forms.Padding(5);
            this.groupPatient.Name = "groupPatient";
            this.groupPatient.Padding = new System.Windows.Forms.Padding(5);
            this.groupPatient.Size = new System.Drawing.Size(258, 214);
            this.groupPatient.TabIndex = 1;
            this.groupPatient.TabStop = false;
            this.groupPatient.Text = "病人";
            // 
            // textPatGender
            // 
            this.textPatGender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatGender.BackColor = System.Drawing.SystemColors.Window;
            this.textPatGender.Location = new System.Drawing.Point(103, 166);
            this.textPatGender.Name = "textPatGender";
            this.textPatGender.ReadOnly = true;
            this.textPatGender.Size = new System.Drawing.Size(147, 29);
            this.textPatGender.TabIndex = 9;
            // 
            // labelPatGender
            // 
            this.labelPatGender.Location = new System.Drawing.Point(8, 167);
            this.labelPatGender.Name = "labelPatGender";
            this.labelPatGender.Size = new System.Drawing.Size(89, 26);
            this.labelPatGender.TabIndex = 8;
            this.labelPatGender.Text = "性別：";
            this.labelPatGender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPatBirthDate
            // 
            this.textPatBirthDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatBirthDate.BackColor = System.Drawing.SystemColors.Window;
            this.textPatBirthDate.Location = new System.Drawing.Point(103, 131);
            this.textPatBirthDate.Name = "textPatBirthDate";
            this.textPatBirthDate.ReadOnly = true;
            this.textPatBirthDate.Size = new System.Drawing.Size(147, 29);
            this.textPatBirthDate.TabIndex = 7;
            // 
            // labelPatBirthDate
            // 
            this.labelPatBirthDate.Location = new System.Drawing.Point(8, 132);
            this.labelPatBirthDate.Name = "labelPatBirthDate";
            this.labelPatBirthDate.Size = new System.Drawing.Size(89, 26);
            this.labelPatBirthDate.TabIndex = 6;
            this.labelPatBirthDate.Text = "生日：";
            this.labelPatBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPatName
            // 
            this.textPatName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatName.BackColor = System.Drawing.SystemColors.Window;
            this.textPatName.Location = new System.Drawing.Point(103, 96);
            this.textPatName.Name = "textPatName";
            this.textPatName.ReadOnly = true;
            this.textPatName.Size = new System.Drawing.Size(147, 29);
            this.textPatName.TabIndex = 5;
            // 
            // labelPatName
            // 
            this.labelPatName.Location = new System.Drawing.Point(8, 97);
            this.labelPatName.Name = "labelPatName";
            this.labelPatName.Size = new System.Drawing.Size(89, 26);
            this.labelPatName.TabIndex = 4;
            this.labelPatName.Text = "姓名：";
            this.labelPatName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPatId
            // 
            this.textPatId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatId.BackColor = System.Drawing.SystemColors.Window;
            this.textPatId.Location = new System.Drawing.Point(103, 61);
            this.textPatId.Name = "textPatId";
            this.textPatId.ReadOnly = true;
            this.textPatId.Size = new System.Drawing.Size(147, 29);
            this.textPatId.TabIndex = 3;
            // 
            // labelPatId
            // 
            this.labelPatId.Location = new System.Drawing.Point(8, 62);
            this.labelPatId.Name = "labelPatId";
            this.labelPatId.Size = new System.Drawing.Size(89, 26);
            this.labelPatId.TabIndex = 2;
            this.labelPatId.Text = "身分證號：";
            this.labelPatId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPatChtno
            // 
            this.textPatChtno.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPatChtno.BackColor = System.Drawing.SystemColors.Window;
            this.textPatChtno.Location = new System.Drawing.Point(103, 26);
            this.textPatChtno.Name = "textPatChtno";
            this.textPatChtno.ReadOnly = true;
            this.textPatChtno.Size = new System.Drawing.Size(147, 29);
            this.textPatChtno.TabIndex = 1;
            // 
            // labelPatChtno
            // 
            this.labelPatChtno.Location = new System.Drawing.Point(8, 27);
            this.labelPatChtno.Name = "labelPatChtno";
            this.labelPatChtno.Size = new System.Drawing.Size(89, 26);
            this.labelPatChtno.TabIndex = 0;
            this.labelPatChtno.Text = "病歷號：";
            this.labelPatChtno.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupDocument
            // 
            this.groupDocument.Controls.Add(this.textDocAuthor);
            this.groupDocument.Controls.Add(this.labelDocAuthor);
            this.groupDocument.Controls.Add(this.textDocDate);
            this.groupDocument.Controls.Add(this.labelDocDate);
            this.groupDocument.Controls.Add(this.textDocId);
            this.groupDocument.Controls.Add(this.labelDocId);
            this.groupDocument.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDocument.Location = new System.Drawing.Point(0, 0);
            this.groupDocument.Margin = new System.Windows.Forms.Padding(5);
            this.groupDocument.Name = "groupDocument";
            this.groupDocument.Padding = new System.Windows.Forms.Padding(5);
            this.groupDocument.Size = new System.Drawing.Size(258, 143);
            this.groupDocument.TabIndex = 0;
            this.groupDocument.TabStop = false;
            this.groupDocument.Text = "報告";
            // 
            // textDocDate
            // 
            this.textDocDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDocDate.BackColor = System.Drawing.SystemColors.Window;
            this.textDocDate.Location = new System.Drawing.Point(103, 61);
            this.textDocDate.Name = "textDocDate";
            this.textDocDate.ReadOnly = true;
            this.textDocDate.Size = new System.Drawing.Size(147, 29);
            this.textDocDate.TabIndex = 3;
            // 
            // labelDocDate
            // 
            this.labelDocDate.Location = new System.Drawing.Point(8, 62);
            this.labelDocDate.Name = "labelDocDate";
            this.labelDocDate.Size = new System.Drawing.Size(89, 26);
            this.labelDocDate.TabIndex = 2;
            this.labelDocDate.Text = "日期：";
            this.labelDocDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textDocId
            // 
            this.textDocId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDocId.BackColor = System.Drawing.SystemColors.Window;
            this.textDocId.Location = new System.Drawing.Point(103, 26);
            this.textDocId.Name = "textDocId";
            this.textDocId.ReadOnly = true;
            this.textDocId.Size = new System.Drawing.Size(147, 29);
            this.textDocId.TabIndex = 1;
            // 
            // labelDocId
            // 
            this.labelDocId.Location = new System.Drawing.Point(8, 27);
            this.labelDocId.Name = "labelDocId";
            this.labelDocId.Size = new System.Drawing.Size(89, 26);
            this.labelDocId.TabIndex = 0;
            this.labelDocId.Text = "單號：";
            this.labelDocId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupLabResult
            // 
            this.groupLabResult.Controls.Add(this.splitContainer2);
            this.groupLabResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupLabResult.Location = new System.Drawing.Point(0, 0);
            this.groupLabResult.Name = "groupLabResult";
            this.groupLabResult.Size = new System.Drawing.Size(547, 437);
            this.groupLabResult.TabIndex = 0;
            this.groupLabResult.TabStop = false;
            this.groupLabResult.Text = "檢驗結果";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvData);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvDetail);
            this.splitContainer2.Size = new System.Drawing.Size(541, 409);
            this.splitContainer2.SplitterDistance = 127;
            this.splitContainer2.TabIndex = 0;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AutoGenerateColumns = false;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.DataSource = this.bindingSource;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvData.Size = new System.Drawing.Size(541, 127);
            this.dgvData.TabIndex = 1;
            this.dgvData.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AutoGenerateColumns = false;
            this.dgvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetail.DataSource = this.bindingSourceDetail;
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.Location = new System.Drawing.Point(0, 0);
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowTemplate.Height = 24;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDetail.Size = new System.Drawing.Size(541, 278);
            this.dgvDetail.TabIndex = 1;
            // 
            // textDocAuthor
            // 
            this.textDocAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDocAuthor.BackColor = System.Drawing.SystemColors.Window;
            this.textDocAuthor.Location = new System.Drawing.Point(103, 96);
            this.textDocAuthor.Name = "textDocAuthor";
            this.textDocAuthor.ReadOnly = true;
            this.textDocAuthor.Size = new System.Drawing.Size(147, 29);
            this.textDocAuthor.TabIndex = 5;
            // 
            // labelDocAuthor
            // 
            this.labelDocAuthor.Location = new System.Drawing.Point(8, 97);
            this.labelDocAuthor.Name = "labelDocAuthor";
            this.labelDocAuthor.Size = new System.Drawing.Size(89, 26);
            this.labelDocAuthor.TabIndex = 4;
            this.labelDocAuthor.Text = "報告人員：";
            this.labelDocAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(826, 437);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "DetailForm";
            this.Text = "檢驗檢查報告明細";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupEncounter.ResumeLayout(false);
            this.groupEncounter.PerformLayout();
            this.groupOrganization.ResumeLayout(false);
            this.groupOrganization.PerformLayout();
            this.groupPatient.ResumeLayout(false);
            this.groupPatient.PerformLayout();
            this.groupDocument.ResumeLayout(false);
            this.groupDocument.PerformLayout();
            this.groupLabResult.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupDocument;
        private System.Windows.Forms.TextBox textDocId;
        private System.Windows.Forms.Label labelDocId;
        private System.Windows.Forms.TextBox textDocDate;
        private System.Windows.Forms.Label labelDocDate;
        private System.Windows.Forms.GroupBox groupPatient;
        private System.Windows.Forms.TextBox textPatId;
        private System.Windows.Forms.Label labelPatId;
        private System.Windows.Forms.TextBox textPatChtno;
        private System.Windows.Forms.Label labelPatChtno;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textPatGender;
        private System.Windows.Forms.Label labelPatGender;
        private System.Windows.Forms.TextBox textPatBirthDate;
        private System.Windows.Forms.Label labelPatBirthDate;
        private System.Windows.Forms.TextBox textPatName;
        private System.Windows.Forms.Label labelPatName;
        private System.Windows.Forms.GroupBox groupOrganization;
        private System.Windows.Forms.TextBox textOrgName;
        private System.Windows.Forms.Label labelOrgName;
        private System.Windows.Forms.TextBox textOrgId;
        private System.Windows.Forms.Label labelOrgId;
        private System.Windows.Forms.GroupBox groupEncounter;
        private System.Windows.Forms.TextBox textOdrDr;
        private System.Windows.Forms.Label labelOdrDr;
        private System.Windows.Forms.TextBox textOdrId;
        private System.Windows.Forms.Label labelOdrId;
        private System.Windows.Forms.TextBox textOdrDate;
        private System.Windows.Forms.Label labelOdrDate;
        private System.Windows.Forms.GroupBox groupLabResult;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.BindingSource bindingSourceDetail;
        private System.Windows.Forms.TextBox textDocAuthor;
        private System.Windows.Forms.Label labelDocAuthor;
    }
}