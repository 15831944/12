﻿namespace ControlPlatformLib
{
    partial class FormProjectDataSetting
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
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.buttonAddGroup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonRemoveGroup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridGroup = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxGroupName.Location = new System.Drawing.Point(44, 591);
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(133, 21);
            this.textBoxGroupName.TabIndex = 3;
            // 
            // buttonAddGroup
            // 
            this.buttonAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddGroup.Location = new System.Drawing.Point(183, 574);
            this.buttonAddGroup.Name = "buttonAddGroup";
            this.buttonAddGroup.Size = new System.Drawing.Size(76, 39);
            this.buttonAddGroup.TabIndex = 4;
            this.buttonAddGroup.Text = "Add";
            this.buttonAddGroup.UseVisualStyleBackColor = true;
            this.buttonAddGroup.Click += new System.EventHandler(this.buttonAddGroup_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 574);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name";
            // 
            // buttonRemoveGroup
            // 
            this.buttonRemoveGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveGroup.Location = new System.Drawing.Point(263, 573);
            this.buttonRemoveGroup.Name = "buttonRemoveGroup";
            this.buttonRemoveGroup.Size = new System.Drawing.Size(76, 39);
            this.buttonRemoveGroup.TabIndex = 4;
            this.buttonRemoveGroup.Text = "Remove";
            this.buttonRemoveGroup.UseVisualStyleBackColor = true;
            this.buttonRemoveGroup.Click += new System.EventHandler(this.buttonRemoveGroup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 20F);
            this.label1.Location = new System.Drawing.Point(127, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 27);
            this.label1.TabIndex = 7;
            this.label1.Text = "Group";
            // 
            // dataGridGroup
            // 
            this.dataGridGroup.AllowUserToAddRows = false;
            this.dataGridGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column5});
            this.dataGridGroup.Location = new System.Drawing.Point(24, 55);
            this.dataGridGroup.Name = "dataGridGroup";
            this.dataGridGroup.RowHeadersWidth = 20;
            this.dataGridGroup.RowTemplate.Height = 23;
            this.dataGridGroup.Size = new System.Drawing.Size(315, 508);
            this.dataGridGroup.TabIndex = 14;
            this.dataGridGroup.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridGroup_CellClick);
            this.dataGridGroup.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridGroup_CellValidated);
            this.dataGridGroup.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridGroup_RowHeaderMouseClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Remard";
            this.Column5.Name = "Column5";
            this.Column5.Width = 200;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(362, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(768, 602);
            this.panel1.TabIndex = 15;
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(263, 10);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(76, 39);
            this.buttonExport.TabIndex = 4;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // FormProjectDataSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1151, 631);
            this.Controls.Add(this.dataGridGroup);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRemoveGroup);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonAddGroup);
            this.Controls.Add(this.textBoxGroupName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormProjectDataSetting";
            this.Text = "FormHardSetting";
            this.Load += new System.EventHandler(this.FormProjectDataSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.Button buttonAddGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonRemoveGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonExport;
    }
}