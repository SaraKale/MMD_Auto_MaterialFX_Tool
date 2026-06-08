namespace PMX_Material_Tools
{
    partial class MaterialPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialPreview));
            this.OutCSVlist = new System.Windows.Forms.Label();
            this.outCSV_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PreviewRM_text = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ENG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TexPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ToonPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SpaPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PreviewList = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutCSVlist
            // 
            this.OutCSVlist.Font = new System.Drawing.Font("宋体", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutCSVlist.Location = new System.Drawing.Point(9, 32);
            this.OutCSVlist.Name = "OutCSVlist";
            this.OutCSVlist.Size = new System.Drawing.Size(281, 62);
            this.OutCSVlist.TabIndex = 2;
            this.OutCSVlist.Text = "导出CSV文件";
            // 
            // outCSV_button
            // 
            this.outCSV_button.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.outCSV_button.Location = new System.Drawing.Point(0, 14);
            this.outCSV_button.Name = "outCSV_button";
            this.outCSV_button.Size = new System.Drawing.Size(219, 72);
            this.outCSV_button.TabIndex = 5;
            this.outCSV_button.Text = "保存";
            this.outCSV_button.UseVisualStyleBackColor = true;
            this.outCSV_button.Click += new System.EventHandler(this.outCSV_button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 478F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.04312F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.03984F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.95335F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(821, 1029);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.PreviewRM_text);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(815, 169);
            this.panel1.TabIndex = 6;
            // 
            // PreviewRM_text
            // 
            this.PreviewRM_text.Font = new System.Drawing.Font("宋体", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PreviewRM_text.Location = new System.Drawing.Point(9, 6);
            this.PreviewRM_text.Name = "PreviewRM_text";
            this.PreviewRM_text.Size = new System.Drawing.Size(797, 141);
            this.PreviewRM_text.TabIndex = 0;
            this.PreviewRM_text.Text = "材质预览窗口\r\n这是可以预览查看材质的相关列表，同时可以导出 .csv 文件。";
            this.PreviewRM_text.Click += new System.EventHandler(this.PreviewRM_text_Click);
            // 
            // panel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.PreviewList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 178);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(815, 724);
            this.panel2.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.OutCSVlist);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 908);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(337, 118);
            this.panel3.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.outCSV_button);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(346, 908);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(472, 118);
            this.panel4.TabIndex = 9;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 77;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "jp Name";
            this.NameColumn.Width = 161;
            // 
            // ENG
            // 
            this.ENG.Text = "Eng Name";
            this.ENG.Width = 155;
            // 
            // TexPath
            // 
            this.TexPath.Text = "Tex";
            this.TexPath.Width = 172;
            // 
            // ToonPath
            // 
            this.ToonPath.Text = "Toon";
            this.ToonPath.Width = 106;
            // 
            // SpaPath
            // 
            this.SpaPath.Text = "Spa";
            this.SpaPath.Width = 103;
            // 
            // PreviewList
            // 
            this.PreviewList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.NameColumn,
            this.ENG,
            this.TexPath,
            this.ToonPath,
            this.SpaPath});
            this.PreviewList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewList.Font = new System.Drawing.Font("宋体", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PreviewList.GridLines = true;
            this.PreviewList.HideSelection = false;
            this.PreviewList.Location = new System.Drawing.Point(0, 0);
            this.PreviewList.Name = "PreviewList";
            this.PreviewList.Size = new System.Drawing.Size(815, 724);
            this.PreviewList.TabIndex = 1;
            this.PreviewList.UseCompatibleStateImageBehavior = false;
            this.PreviewList.View = System.Windows.Forms.View.Details;
            this.PreviewList.SelectedIndexChanged += new System.EventHandler(this.PreviewList_SelectedIndexChanged);
            // 
            // MaterialPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(821, 1029);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MaterialPreview";
            this.Text = "材质预览窗口";
            this.Load += new System.EventHandler(this.MaterialPreview_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label OutCSVlist;
        private System.Windows.Forms.Button outCSV_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label PreviewRM_text;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView PreviewList;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader ENG;
        private System.Windows.Forms.ColumnHeader TexPath;
        private System.Windows.Forms.ColumnHeader ToonPath;
        private System.Windows.Forms.ColumnHeader SpaPath;
    }
}

