using System;

namespace PMX_Material_Tools
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.note_button = new System.Windows.Forms.CheckBox();
            this.input_button = new System.Windows.Forms.Button();
            this.inport_text = new System.Windows.Forms.Label();
            this.inputfilebox = new System.Windows.Forms.TextBox();
            this.render_text = new System.Windows.Forms.Label();
            this.render_list = new System.Windows.Forms.ComboBox();
            this.outputbox = new System.Windows.Forms.TextBox();
            this.output_text = new System.Windows.Forms.Label();
            this.output_button = new System.Windows.Forms.Button();
            this.export_button = new System.Windows.Forms.Button();
            this.languagetext = new System.Windows.Forms.Label();
            this.languagelist = new System.Windows.Forms.ComboBox();
            this.Material_language = new System.Windows.Forms.Label();
            this.Meteriallang_button = new System.Windows.Forms.ComboBox();
            this.versionText = new System.Windows.Forms.Label();
            this.exportoption_text = new System.Windows.Forms.Label();
            this.Exportoptions_list = new System.Windows.Forms.ComboBox();
            this.directoverwrite_text = new System.Windows.Forms.CheckBox();
            this.ExportAfterFolder_text = new System.Windows.Forms.CheckBox();
            this.Packimages = new System.Windows.Forms.CheckBox();
            this.Preview = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // note_button
            // 
            this.note_button.AutoSize = true;
            this.note_button.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.note_button.Location = new System.Drawing.Point(487, 272);
            this.note_button.Margin = new System.Windows.Forms.Padding(6);
            this.note_button.Name = "note_button";
            this.note_button.Size = new System.Drawing.Size(143, 37);
            this.note_button.TabIndex = 0;
            this.note_button.Text = "含注释";
            this.note_button.UseVisualStyleBackColor = true;
            this.note_button.CheckedChanged += new System.EventHandler(this.note_button_CheckedChanged);
            // 
            // input_button
            // 
            this.input_button.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.input_button.Location = new System.Drawing.Point(788, 137);
            this.input_button.Margin = new System.Windows.Forms.Padding(6);
            this.input_button.Name = "input_button";
            this.input_button.Size = new System.Drawing.Size(168, 56);
            this.input_button.TabIndex = 1;
            this.input_button.Text = "浏览";
            this.input_button.UseVisualStyleBackColor = true;
            this.input_button.Click += new System.EventHandler(this.Input_button_Click);
            // 
            // inport_text
            // 
            this.inport_text.AutoSize = true;
            this.inport_text.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inport_text.Location = new System.Drawing.Point(52, 88);
            this.inport_text.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.inport_text.Name = "inport_text";
            this.inport_text.Size = new System.Drawing.Size(191, 33);
            this.inport_text.TabIndex = 2;
            this.inport_text.Text = "导入PMX文件";
            this.inport_text.Click += new System.EventHandler(this.inport_text_Click);
            // 
            // inputfilebox
            // 
            this.inputfilebox.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inputfilebox.Location = new System.Drawing.Point(58, 141);
            this.inputfilebox.Margin = new System.Windows.Forms.Padding(6);
            this.inputfilebox.Name = "inputfilebox";
            this.inputfilebox.Size = new System.Drawing.Size(692, 42);
            this.inputfilebox.TabIndex = 3;
            this.inputfilebox.TextChanged += new System.EventHandler(this.inputfilebox_TextChanged);
            // 
            // render_text
            // 
            this.render_text.AutoSize = true;
            this.render_text.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.render_text.Location = new System.Drawing.Point(55, 217);
            this.render_text.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.render_text.Name = "render_text";
            this.render_text.Size = new System.Drawing.Size(175, 33);
            this.render_text.TabIndex = 4;
            this.render_text.Text = "选择渲染器";
            this.render_text.Click += new System.EventHandler(this.render_text_Click);
            // 
            // render_list
            // 
            this.render_list.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.render_list.FormattingEnabled = true;
            this.render_list.Location = new System.Drawing.Point(61, 268);
            this.render_list.Margin = new System.Windows.Forms.Padding(6);
            this.render_list.Name = "render_list";
            this.render_list.Size = new System.Drawing.Size(390, 41);
            this.render_list.TabIndex = 5;
            this.render_list.SelectedIndexChanged += new System.EventHandler(this.render_list_SelectedIndexChanged);
            // 
            // outputbox
            // 
            this.outputbox.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.outputbox.Location = new System.Drawing.Point(61, 403);
            this.outputbox.Margin = new System.Windows.Forms.Padding(6);
            this.outputbox.Name = "outputbox";
            this.outputbox.Size = new System.Drawing.Size(689, 42);
            this.outputbox.TabIndex = 8;
            this.outputbox.TextChanged += new System.EventHandler(this.outputbox_TextChanged);
            // 
            // output_text
            // 
            this.output_text.AutoSize = true;
            this.output_text.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.output_text.Location = new System.Drawing.Point(52, 349);
            this.output_text.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.output_text.Name = "output_text";
            this.output_text.Size = new System.Drawing.Size(143, 33);
            this.output_text.TabIndex = 7;
            this.output_text.Text = "导出路径";
            this.output_text.Click += new System.EventHandler(this.output_text_Click);
            // 
            // output_button
            // 
            this.output_button.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.output_button.Location = new System.Drawing.Point(788, 399);
            this.output_button.Margin = new System.Windows.Forms.Padding(6);
            this.output_button.Name = "output_button";
            this.output_button.Size = new System.Drawing.Size(168, 58);
            this.output_button.TabIndex = 6;
            this.output_button.Text = "浏览";
            this.output_button.UseVisualStyleBackColor = true;
            this.output_button.Click += new System.EventHandler(this.output_button_Click);
            // 
            // export_button
            // 
            this.export_button.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.export_button.Location = new System.Drawing.Point(395, 578);
            this.export_button.Margin = new System.Windows.Forms.Padding(6);
            this.export_button.Name = "export_button";
            this.export_button.Size = new System.Drawing.Size(263, 63);
            this.export_button.TabIndex = 9;
            this.export_button.Text = "输出";
            this.export_button.UseVisualStyleBackColor = true;
            this.export_button.Click += new System.EventHandler(this.export_button_Click);
            // 
            // languagetext
            // 
            this.languagetext.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.languagetext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.languagetext.Location = new System.Drawing.Point(565, 31);
            this.languagetext.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.languagetext.Name = "languagetext";
            this.languagetext.Size = new System.Drawing.Size(207, 36);
            this.languagetext.TabIndex = 10;
            this.languagetext.Text = "界面语言";
            this.languagetext.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.languagetext.Click += new System.EventHandler(this.languagetext_Click);
            // 
            // languagelist
            // 
            this.languagelist.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.languagelist.FormattingEnabled = true;
            this.languagelist.Location = new System.Drawing.Point(792, 26);
            this.languagelist.Margin = new System.Windows.Forms.Padding(6);
            this.languagelist.Name = "languagelist";
            this.languagelist.Size = new System.Drawing.Size(243, 41);
            this.languagelist.TabIndex = 11;
            this.languagelist.SelectedIndexChanged += new System.EventHandler(this.languagelist_SelectedIndexChanged);
            // 
            // Material_language
            // 
            this.Material_language.AutoSize = true;
            this.Material_language.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Material_language.Location = new System.Drawing.Point(707, 220);
            this.Material_language.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Material_language.Name = "Material_language";
            this.Material_language.Size = new System.Drawing.Size(175, 33);
            this.Material_language.TabIndex = 12;
            this.Material_language.Text = "FX文本语言";
            this.Material_language.Click += new System.EventHandler(this.Material_language_Click);
            // 
            // Meteriallang_button
            // 
            this.Meteriallang_button.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Meteriallang_button.FormattingEnabled = true;
            this.Meteriallang_button.Location = new System.Drawing.Point(713, 271);
            this.Meteriallang_button.Margin = new System.Windows.Forms.Padding(6);
            this.Meteriallang_button.Name = "Meteriallang_button";
            this.Meteriallang_button.Size = new System.Drawing.Size(257, 41);
            this.Meteriallang_button.TabIndex = 13;
            this.Meteriallang_button.SelectedIndexChanged += new System.EventHandler(this.Meteriallang_button_SelectedIndexChanged);
            // 
            // versionText
            // 
            this.versionText.AutoSize = true;
            this.versionText.Font = new System.Drawing.Font("宋体", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.versionText.Location = new System.Drawing.Point(934, 614);
            this.versionText.Name = "versionText";
            this.versionText.Size = new System.Drawing.Size(96, 27);
            this.versionText.TabIndex = 14;
            this.versionText.Text = "v0.0.7";
            this.versionText.Click += new System.EventHandler(this.versionText_Click);
            // 
            // exportoption_text
            // 
            this.exportoption_text.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exportoption_text.Location = new System.Drawing.Point(55, 495);
            this.exportoption_text.Name = "exportoption_text";
            this.exportoption_text.Size = new System.Drawing.Size(163, 102);
            this.exportoption_text.TabIndex = 15;
            this.exportoption_text.Text = "导出选项";
            this.exportoption_text.Click += new System.EventHandler(this.exportoption_text_Click);
            // 
            // Exportoptions_list
            // 
            this.Exportoptions_list.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Exportoptions_list.FormattingEnabled = true;
            this.Exportoptions_list.Location = new System.Drawing.Point(234, 486);
            this.Exportoptions_list.Name = "Exportoptions_list";
            this.Exportoptions_list.Size = new System.Drawing.Size(360, 41);
            this.Exportoptions_list.TabIndex = 16;
            this.Exportoptions_list.SelectedIndexChanged += new System.EventHandler(this.Exportoptions_list_SelectedIndexChanged);
            // 
            // directoverwrite_text
            // 
            this.directoverwrite_text.AutoSize = true;
            this.directoverwrite_text.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.directoverwrite_text.Location = new System.Drawing.Point(363, 347);
            this.directoverwrite_text.Name = "directoverwrite_text";
            this.directoverwrite_text.Size = new System.Drawing.Size(175, 37);
            this.directoverwrite_text.TabIndex = 18;
            this.directoverwrite_text.Text = "源文件夹";
            this.directoverwrite_text.UseVisualStyleBackColor = true;
            this.directoverwrite_text.CheckedChanged += new System.EventHandler(this.directoverwrite_text_CheckedChanged);
            // 
            // ExportAfterFolder_text
            // 
            this.ExportAfterFolder_text.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ExportAfterFolder_text.Font = new System.Drawing.Font("宋体", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExportAfterFolder_text.Location = new System.Drawing.Point(667, 495);
            this.ExportAfterFolder_text.Name = "ExportAfterFolder_text";
            this.ExportAfterFolder_text.Size = new System.Drawing.Size(363, 97);
            this.ExportAfterFolder_text.TabIndex = 19;
            this.ExportAfterFolder_text.Text = "输出后自动打开文件夹";
            this.ExportAfterFolder_text.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ExportAfterFolder_text.UseVisualStyleBackColor = true;
            this.ExportAfterFolder_text.CheckedChanged += new System.EventHandler(this.ExportAfterFolder_text_CheckedChanged);
            // 
            // Packimages
            // 
            this.Packimages.AutoSize = true;
            this.Packimages.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Packimages.Location = new System.Drawing.Point(667, 347);
            this.Packimages.Name = "Packimages";
            this.Packimages.Size = new System.Drawing.Size(367, 37);
            this.Packimages.TabIndex = 20;
            this.Packimages.Text = "复制源图片到输出路径";
            this.Packimages.UseVisualStyleBackColor = true;
            // 
            // Preview
            // 
            this.Preview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Preview.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Preview.Location = new System.Drawing.Point(987, 136);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(58, 60);
            this.Preview.TabIndex = 21;
            this.Preview.Text = ">";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.Click += new System.EventHandler(this.Preview_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 684);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.Packimages);
            this.Controls.Add(this.ExportAfterFolder_text);
            this.Controls.Add(this.directoverwrite_text);
            this.Controls.Add(this.Exportoptions_list);
            this.Controls.Add(this.exportoption_text);
            this.Controls.Add(this.versionText);
            this.Controls.Add(this.Meteriallang_button);
            this.Controls.Add(this.Material_language);
            this.Controls.Add(this.languagelist);
            this.Controls.Add(this.languagetext);
            this.Controls.Add(this.export_button);
            this.Controls.Add(this.outputbox);
            this.Controls.Add(this.output_text);
            this.Controls.Add(this.output_button);
            this.Controls.Add(this.render_list);
            this.Controls.Add(this.render_text);
            this.Controls.Add(this.inputfilebox);
            this.Controls.Add(this.inport_text);
            this.Controls.Add(this.input_button);
            this.Controls.Add(this.note_button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MMD一键自动生成材质FX工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.CheckBox note_button;
        private System.Windows.Forms.Button input_button;
        private System.Windows.Forms.Label inport_text;
        private System.Windows.Forms.TextBox inputfilebox;
        private System.Windows.Forms.Label render_text;
        private System.Windows.Forms.ComboBox render_list;
        private System.Windows.Forms.TextBox outputbox;
        private System.Windows.Forms.Label output_text;
        private System.Windows.Forms.Button output_button;
        private System.Windows.Forms.Button export_button;
        private System.Windows.Forms.Label languagetext;
        private System.Windows.Forms.ComboBox languagelist;
        private System.Windows.Forms.Label Material_language;
        private System.Windows.Forms.ComboBox Meteriallang_button;
        private System.Windows.Forms.Label versionText;
        private System.Windows.Forms.Label exportoption_text;
        private System.Windows.Forms.ComboBox Exportoptions_list;
        private System.Windows.Forms.CheckBox directoverwrite_text;
        private System.Windows.Forms.CheckBox ExportAfterFolder_text;
        private System.Windows.Forms.CheckBox Packimages;
        private System.Windows.Forms.Button Preview;
    }
}