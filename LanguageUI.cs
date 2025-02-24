using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace PMX_Material_Tools
{
    // 界面语言和气泡控件 分类
    public partial class Form1 : Form
    {
        private ToolTipBubble tooltipBubble;  // 使用自定义的气泡控件
        private MaterialPreview materialPreview; // MaterialPreview 实例

        // tooltipBubble 气泡控件提示
        public class ToolTipBubble : Label
        {
            public ToolTipBubble()
            {
                this.BackColor = System.Drawing.Color.LightYellow; // 设置气泡背景颜色
                this.ForeColor = System.Drawing.Color.Black; // 设置气泡文字颜色
                this.AutoSize = true; // 自适应内容
                this.Padding = new Padding(5); // 设置内边距，使文字与边框之间有空间
                this.Visible = false; // 默认不可见
                this.BorderStyle = BorderStyle.FixedSingle; // 设置边框样式                 
                this.Font = new Font(this.Font.FontFamily, 10); //设置默认字体大小
            }
        }

        // -------------------------
        // 批量绑定控件与提示文本
        // -------------------------
        private void BindControlTooltips()
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

            // 创建一个字典，包含控件和对应的资源键
            var controlsAndTooltips = new Dictionary<Control, string>
                {
                    { inport_text, "inputfilebox_tips" }, // 导入PMX文件 提示
                    { render_text, "Rendererlist_tips" }, // 选择渲染器 提示
                    { note_button, "notebutton_tips" }, // 注释复选框 提示
                    { Material_language, "FXlanguage_tips" }, // FX文本语言 提示
                    { directoverwrite_text, "directoverwrite_tips" }, // 源文件夹 提示
                    { Packimages, "Packimages_tips" }, // 复制图片到输出路径 提示
                    { exportoption_text, "exportoption_tips" } // 导出选项 提示
                };

            // 为每个控件绑定鼠标悬停事件
            foreach (var controlAndTooltip in controlsAndTooltips)
            {
                string tooltipKey = controlAndTooltip.Value;

                controlAndTooltip.Key.MouseHover += (sender, e) =>
                {
                    // 从资源文件加载对应的提示文本
                    string tooltipText = rm.GetString(tooltipKey);

                    // 设置气泡的文本
                    tooltipBubble.Text = tooltipText;

                    // 获取控件的位置，设置气泡的位置
                    Control control = (Control)sender;
                    int xOffset = 10; // 气泡与控件之间的水平间距
                    int yOffset = 10; // 气泡与控件之间的垂直间距

                    int xLocation = control.Location.X + control.Width + xOffset;
                    int yLocation = control.Location.Y + yOffset;

                    // 如果气泡位置超出窗体右边界，调整气泡位置
                    if (xLocation + tooltipBubble.Width > this.ClientSize.Width)
                    {
                        xLocation = this.ClientSize.Width - tooltipBubble.Width - 5;
                    }

                    tooltipBubble.Location = new Point(xLocation, yLocation);
                    tooltipBubble.Visible = true;
                    tooltipBubble.BringToFront();
                };

                controlAndTooltip.Key.MouseLeave += OnControlLeave;
            }
        }

        // -------------------------
        // 鼠标离开时隐藏气泡
        // -------------------------
        private void OnControlLeave(object sender, EventArgs e)
        {
            tooltipBubble.Visible = false; // 隐藏气泡
        }

        // --------------------------
        // 更新界面语言
        // --------------------------
        private void SetLanguage(string language)
        {
            try
            {
                // 将显示值转换为区域性名称
                if (languageMap.TryGetValue(language, out string cultureName))
                {
                    // 设置当前线程的文化信息
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(cultureName);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

                    // 链接 ResourceManager 语言文件
                    ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

                    // 更新界面文本
                    this.Text = rm.GetString("FormTitle"); // 标题
                    input_button.Text = rm.GetString("ImportPMXFile"); // 导入PMX文件
                    output_button.Text = rm.GetString("ExportFolder");// 导出路径 按钮
                    export_button.Text = rm.GetString("Export"); // 导出
                    languagetext.Text = rm.GetString("languagetext"); // 界面语言
                    inport_text.Text = rm.GetString("inporttext"); // 导入PMX文件
                    render_text.Text = rm.GetString("rendertext"); // 选择渲染器
                    note_button.Text = rm.GetString("Annotationnotes"); // 含注释
                    Material_language.Text = rm.GetString("Materiallanguage"); // 材质文本语言
                    output_text.Text = rm.GetString("outputtext"); // 导出路径 文本
                    exportoption_text.Text = rm.GetString("exportoptiontext"); // 导出选项文本
                    Packimages.Text = rm.GetString("Packagingimages"); // 复制图片到输出路径
                    ExportAfterFolder_text.Text = rm.GetString("ExportOpenAfterFolder"); // 输出后自动打开文件夹
                    directoverwrite_text.Text = rm.GetString("DirectOverWrite"); // 源文件夹

                    // 更新材质语言列表
                    Meteriallang_button.Items.Clear();
                    Meteriallang_button.Items.AddRange(new string[] {
                        rm.GetString("MaterialLanguage_None"), // 无
                        rm.GetString("MaterialLanguage_English"), // 英语
                        rm.GetString("MaterialLanguage_SimplifiedChinese"), // 简体中文
                        rm.GetString("MaterialLanguage_TraditionalChinese"), // 繁体中文
                        rm.GetString("MaterialLanguage_Japanese") // 日语
                    });

                    // 更新渲染器列表
                    render_list.Items.Clear();
                    render_list.Items.AddRange(new string[] {
                        rm.GetString("Renderer_None"), // 无
                        rm.GetString("Renderer_RayMMD"),
                        rm.GetString("Renderer_ikPolishShader"),
                        rm.GetString("Renderer_PowerShader")
                    });

                    // 更新导出选项列表
                    Exportoptions_list.Items.Clear();
                    Exportoptions_list.Items.AddRange(new string[] {
                        rm.GetString("ExportOption_None"), // 无
                        rm.GetString("ExportOption_ByImageName"), // 按图片文件名输出FX
                        rm.GetString("ExportOption_ByMaterialName"), // 按材质名称输出FX
                        rm.GetString("ExportOption_ByID") // 按ID编号输出FX
                    });

                    // 更新气泡提示文本
                    BindControlTooltips();

                    // 更新 MaterialPreview 的语言
                    if (materialPreview != null)
                    {
                        materialPreview.UpdateLanguage(rm);
                    }
                }
                else
                {
                    throw new CultureNotFoundException();
                }
            }
            catch (CultureNotFoundException)
            {
                // 链接 ResourceManager 语言文件
                ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
                // 不支持的语言设置
                string message = string.Format(rm.GetString("UnsupportedLanguageMessage"), language);
                string title = rm.GetString("ErrorTitle"); //错误
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 选择渲染器文本
        private void render_text_Click(object sender, EventArgs e)
        {

        }

        // 渲染器列表
        private void render_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // 渲染器含注释复选框
        private void note_button_CheckedChanged(object sender, EventArgs e)
        {

        }

        // 材质文本语言
        private void Material_language_Click(object sender, EventArgs e)
        {

        }
        // 材质文本语言选择框
        private void Meteriallang_button_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //=======================
        // UI界面语言切换
        //=======================

        // 界面语言文本
        private void languagetext_Click(object sender, EventArgs e)
        {

        }

        // 界面语言选择框
        private void languagelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLanguage = languagelist.SelectedItem.ToString();
            iniFile.Write("Settings", "Language", selectedLanguage);
            SetLanguage(selectedLanguage);

            // 刷新其他列表选项
            render_list.SelectedIndex = 0;
            Meteriallang_button.SelectedIndex = 0;
            Exportoptions_list.SelectedIndex = 0;
        }

        // 总窗体
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // ================ END ========================

    }
}