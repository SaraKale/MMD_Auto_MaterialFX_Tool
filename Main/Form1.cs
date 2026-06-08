using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace PMX_Material_Tools
{
    // 主入口
    public partial class Form1 : Form
    {
        private IniFile iniFile;
        private Dictionary<string, string> languageMap;

        // 读取和写入INI文件，语言设置
        public class IniFile
        {
            public string Path { get; }
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);
            public IniFile(string path)
            {
                Path = path;
            }
            public void Write(string section, string key, string value)
            {
                WritePrivateProfileString(section, key, value, Path);
            }
            public string Read(string section, string key, string defaultValue = "")
            {
                var retVal = new StringBuilder(255);
                GetPrivateProfileString(section, key, defaultValue, retVal, 255, Path);
                return retVal.ToString();
            }
        }

        public Form1()
        {
            InitializeComponent();
            // 导入文件拖放判定
            inputfilebox.AllowDrop = true;
            inputfilebox.DragEnter += InputFileBox_DragEnter;
            inputfilebox.DragDrop += InputFileBox_DragDrop;

            // 创建 ToolTipBubble 控件用于显示提示信息
            tooltipBubble = new ToolTipBubble();
            tooltipBubble.AutoSize = true;  // 设置为自动调整大小
            tooltipBubble.Visible = false; // 初始状态下，提示框是隐藏的，只有当鼠标悬停时才会显示
            this.Controls.Add(tooltipBubble); // 将 ToolTipBubble 控件添加到窗体中

            this.StartPosition = FormStartPosition.CenterScreen; // 主窗体居中显示

            // 批量绑定控件与提示文本
            BindControlTooltips();

            // 初始化渲染器列表
            ResourceManager initRm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
            Exportoptions_list.Items.Clear(); // 清空列表，避免重复选项
            render_list.Items.AddRange(new string[] {
                initRm.GetString("Renderer_None"),
                initRm.GetString("Renderer_RayMMD"),
                initRm.GetString("Renderer_RaySJMatcap"),
                initRm.GetString("Renderer_ikPolishShader"),
                initRm.GetString("Renderer_PowerShader")
            });
            // render_list.SelectedIndex = 0;  // 默认选择"无"

            // 初始化材质语言列表
            Exportoptions_list.Items.Clear(); // 清空列表，避免重复选项
            Meteriallang_button.Items.AddRange(new string[] {
                "无",
                "English",
                "简体中文",
                "繁體中文",
                "日本語"
            });
            // Meteriallang_button.SelectedIndex = 0;  // 默认选择"无"

            // 初始化导出选项列表
            Exportoptions_list.Items.Clear(); // 清空列表，避免重复选项
            Exportoptions_list.Items.AddRange(new string[] {
                initRm.GetString("ExportOption_None"), // 无
                initRm.GetString("ExportOption_ByImageName"), // 图片文件名输出FX
                initRm.GetString("ExportOption_ByMaterialName"), // 材质名称输出FX
                initRm.GetString("ExportOption_ByID"), // ID编号输出FX
                initRm.GetString("ExportOption_ByIDAndMaterialName") // ID编号+材质名称输出FX
            });
            // Exportoptions_list.SelectedIndex = 0;  // 默认选择第一个选项

            // 初始化语言映射字典
            languageMap = new Dictionary<string, string> {
                { "English", "en-US" },
                { "简体中文", "zh-CN" },
                { "繁體中文", "zh-TW" },
                { "日本語", "ja-JP" }
            };

            // 初始化界面语言列表
            languagelist.Items.AddRange(new string[] {
                "English",
                "简体中文",
                "繁體中文",
                "日本語"
            });

            // 初始化 IniFile
            iniFile = new IniFile("settings.ini");

            // 读取并设置上次选择的语言
            string savedLanguage = iniFile.Read("Settings", "Language", "English");
            languagelist.SelectedItem = savedLanguage;
            SetLanguage(savedLanguage);

            // 绑定事件处理程序
            languagelist.SelectedIndexChanged += languagelist_SelectedIndexChanged;

            // 源文件夹选项，绑定directoverwrite_text复选框的CheckedChanged事件
            directoverwrite_text.CheckedChanged += directoverwrite_text_CheckedChanged;
        }

        //=======================================
        // 导入文件处理逻辑
        //=======================================

        // 导入PMX文件按钮
        private void Input_button_Click(object sender, EventArgs e)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = rm.GetString("OpenFileDialogFilter"); // PMX 文件|*.pmx
            openFileDialog.Title = rm.GetString("OpenFileDialogTitle"); // 选择PMX文件

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 将路径完整展示在文本框
                inputfilebox.Text = openFileDialog.FileName;

                // 将源文件路径自动填入outputbox控件地址栏
                outputbox.Text = Path.GetDirectoryName(openFileDialog.FileName);

                // 如果勾选了“源文件夹”选项，自动填入纹理文件所在目录
                // 并禁用outputbox控件、output_button按钮、Packimages复选框
                if (directoverwrite_text.Checked)
                {
                    string textureDirectory = GetTextureDirectoryFromPMX(openFileDialog.FileName);
                    outputbox.Text = textureDirectory;
                    outputbox.Enabled = false;
                    output_button.Enabled = false;
                    Packimages.Enabled = false;
                }
                else
                {
                    outputbox.Text = Path.GetDirectoryName(openFileDialog.FileName);
                    outputbox.Enabled = true;
                    output_button.Enabled = true;
                    Packimages.Enabled = true;
                }

                // 获取材质信息
                List<MaterialInfo> materialInfoList = GetMaterialInfoFromPMX(openFileDialog.FileName);

                // 如果 MaterialPreview 材质预览窗口已经打开，则更新其内容
                if (materialPreview != null && !materialPreview.IsDisposed)
                {
                    materialPreview.LoadMaterialInfo(materialInfoList);
                }
            }
        }
        // 从 PMX 获取纹理目录
        private string GetTextureDirectoryFromPMX(string pmxFilePath)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
            try
            {
                MMDTools.PMXObject pmx = MMDTools.PMXParser.Parse(pmxFilePath);
                foreach (var material in pmx.MaterialList.Span)
                {
                    if (material.Texture < pmx.TextureList.Length)
                    {
                        string textureFile = pmx.TextureList.Span[material.Texture];
                        if (!string.IsNullOrWhiteSpace(textureFile))
                        {
                            return Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(pmxFilePath), textureFile));
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                // 弹出提示：文件未找到
                MessageBox.Show(string.Format(rm.GetString("FileNotFound"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // 解析 PMX 文件错误
                MessageBox.Show(string.Format(rm.GetString("ErrorParsingPMXMessage"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return Path.GetDirectoryName(pmxFilePath);
        }

        // 导入文件拖入事件前的处理逻辑
        private void InputFileBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && files[0].ToLower().EndsWith(".pmx"))
                {
                    e.Effect = DragDropEffects.Copy; // 设置为Copy，表示支持拖放
                }
                else
                {
                    e.Effect = DragDropEffects.None; // 不支持的文件类型
                }
            }
            else
            {
                e.Effect = DragDropEffects.None; // 非文件拖放
            }
        }

        // 导入文件拖放事件后的处理逻辑
        private void InputFileBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1 && files[0].ToLower().EndsWith(".pmx"))
            {
                inputfilebox.Text = files[0]; // 设置拖入的文件路径

                // 将源文件路径自动填入outputbox控件地址栏
                outputbox.Text = Path.GetDirectoryName(files[0]);

                // 如果勾选了“源文件夹”选项，自动填入纹理文件所在目录
                // 并禁用outputbox控件、output_button按钮、Packimages复选框
                if (directoverwrite_text.Checked)
                {
                    string textureDirectory = GetTextureDirectoryFromPMX(files[0]);
                    outputbox.Text = textureDirectory;
                    outputbox.Enabled = false; // 导出路径文本框禁用
                    output_button.Enabled = false; // 导出路径按钮禁用
                    Packimages.Enabled = false; // 复制图片到输出路径复选框禁用
                }
                else
                {
                    outputbox.Text = Path.GetDirectoryName(files[0]);
                    outputbox.Enabled = true; // 导出路径文本框启用
                    output_button.Enabled = true; // 导出路径按钮启用
                    Packimages.Enabled = true; // 复制图片到输出路径复选框启用
                }

                // 获取材质信息
                List<MaterialInfo> materialInfoList = GetMaterialInfoFromPMX(files[0]);

                // 如果 MaterialPreview 材质预览窗口已经打开，则更新其内容
                if (materialPreview != null && !materialPreview.IsDisposed)
                {
                    materialPreview.LoadMaterialInfo(materialInfoList);
                }
            }
        }

        // 导入PMX文本
        private void inport_text_Click(object sender, EventArgs e)
        {
        }

        // 导入路径文本框
        private void inputfilebox_TextChanged(object sender, EventArgs e)
        {
        }

        //=======================================
        // 导出目标路径处理逻辑
        //=======================================

        // 导出路径文本
        private void output_text_Click(object sender, EventArgs e)
        {
        }

        // 导出路径文本框
        private void outputbox_TextChanged(object sender, EventArgs e)
        {
        }

        // 导出选项文本
        private void exportoption_text_Click(object sender, EventArgs e)
        {
        }

        // 导出选项列表
        private void Exportoptions_list_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // 复制图片到输出路径
        private void Packimages_CheckedChanged(object sender, EventArgs e)
        {
        }

        // 导出目标文件夹按钮
        private void output_button_Click(object sender, EventArgs e)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = rm.GetString("FolderBrowserDialogDescription"); // 选择导出目标文件夹

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // 将选择的文件夹路径显示在输出文本框中
                outputbox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        // 导出路径 源文件夹 选项
        private void directoverwrite_text_CheckedChanged(object sender, EventArgs e)
        {
            // 如果勾选了“源文件夹”选项，禁用outputbox控件、output_button按钮、Packimages复选框
            if (directoverwrite_text.Checked)
            {
                outputbox.Enabled = false; // 导出路径文本框禁用
                output_button.Enabled = false; // 导出路径按钮禁用
                Packimages.Enabled = false; // 复制图片到输出路径复选框禁用
            }
            else
            {
                outputbox.Enabled = true; // 导出路径文本框启用
                output_button.Enabled = true; // 导出路径按钮启用
                Packimages.Enabled = true; // 复制图片到输出路径复选框启用
            }
        }

        // 输出后自动打开文件夹 选项
        private void ExportAfterFolder_text_CheckedChanged(object sender, EventArgs e)
        {
        }


        //=======================================
        // 判断.emd文件的编码语言
        //=======================================
        // 判断是否为简体中文
        //private bool IsSimplifiedChinese(string text)
        //{
        //    return text.Any(c => (c >= 0x4E00 && c <= 0x9FFF) || (c >= 0x3400 && c <= 0x4DBF));
        //}
        //// 判断是否为繁体中文
        //private bool IsTraditionalChinese(string text)
        //{
        //    return text.Any(c => (c >= 0xF900 && c <= 0xFAFF) || (c >= 0x4E00 && c <= 0x9FFF));
        //}
        //// 判断是否为日文
        //private bool IsJapanese(string text)
        //{
        //    return text.Any(c =>
        //        (c >= 0x3040 && c <= 0x309F) || // 平假名
        //        (c >= 0x30A0 && c <= 0x30FF) || // 片假名
        //        (c >= 0xFF66 && c <= 0xFF9F) || // 半角片假名
        //        (c >= 0x4E00 && c <= 0x9FFF) // 常见汉字区
        //    );
        //}
        //// 判断材质名称是否为韩语
        //private bool IsKorean(string text)
        //{
        //    return text.Any(c => (c >= 0x1100 && c <= 0x11FF) || (c >= 0x3130 && c <= 0x318F) || (c >= 0xAC00 && c <= 0xD7AF));
        //}
        //// 判断材质名称是否为俄语
        //private bool IsRussian(string text)
        //{
        //    return text.Any(c => c >= 0x0400 && c <= 0x04FF);
        //}
        //// 判断材质名称是否为拉丁字符
        //private bool IsLatin(string text)
        //{
        //    return text.Any(c => c >= 0x0020 && c <= 0x007F);
        //}

        //=======================================
        // CustomRules.ini 自定义重命名规则
        //=======================================
        // 读取 CustomRules.ini 文件的规则
        private List<CustomRule> ReadCustomRules(string iniFilePath)
        {
            var customRules = new List<CustomRule>();
            var lines = File.ReadAllLines(iniFilePath);
            CustomRule currentRule = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (currentRule != null)
                    {
                        customRules.Add(currentRule);
                    }
                    currentRule = new CustomRule();
                }
                else if (currentRule != null)
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        switch (key)
                        {
                            case "Diffuse":
                                currentRule.Diffuse = value;
                                break;
                            case "Specular":
                                currentRule.Specular = value;
                                break;
                            case "Normal":
                                currentRule.Normal = value;
                                break;
                            case "Smoothness":
                                currentRule.Smoothness = value;
                                break;
                            case "Roughness":
                                currentRule.Roughness = value;
                                break;
                            case "Metalness":
                                currentRule.Metalness = value;
                                break;
                            case "Occlusion":
                                currentRule.Occlusion = value;
                                break;
                        }
                    }
                }
            }
            if (currentRule != null)
            {
                customRules.Add(currentRule);
            }
            return customRules;
        }
        private class CustomRule
        {
            public string Diffuse { get; set; } // 漫反射
            public string Specular { get; set; } // 高光
            public string Normal { get; set; } // 法线
            public string Smoothness { get; set; } // 光滑度
            public string Roughness { get; set; } // 粗糙度
            public string Metalness { get; set; } // 金属度
            public string Occlusion { get; set; } // 环境光遮蔽
        }

        // 读取 ini 文件中所有材质后缀（例如 _D, _N 等）
        private HashSet<string> LoadAllSuffixes(string iniFilePath)
        {
            var suffixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(iniFilePath)) return suffixes;

            foreach (var line in File.ReadAllLines(iniFilePath))
            {
                var trimmed = line.Trim();
                if (trimmed.Contains("="))
                {
                    var parts = trimmed.Split('=');
                    if (parts.Length == 2)
                    {
                        string suffix = parts[1].Trim();
                        if (!string.IsNullOrWhiteSpace(suffix))
                            suffixes.Add(suffix);
                    }
                }
            }
            return suffixes;
        }

        // 根据后缀提取前缀，例如 body_N → body
        private string ExtractPrefixFromFilename(string filenameWithoutExtension, HashSet<string> suffixes)
        {
            foreach (var suffix in suffixes.OrderByDescending(s => s.Length))
            {
                if (filenameWithoutExtension.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return filenameWithoutExtension.Substring(0, filenameWithoutExtension.Length - suffix.Length);
                }
            }
            return filenameWithoutExtension;
        }

        //=======================================
        // 文件夹复制总逻辑
        //=======================================
        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }
            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(directory));
                CopyDirectory(directory, destSubDir);
            }
        }

        // 版本号文本
        private void versionText_Click(object sender, EventArgs e)
        {
        }

        //=======================================
        // 预览材质窗口
        //=======================================
        private MaterialPreview previewWindow; // 添加一个字段来跟踪 MaterialPreview 窗口
        private List<MaterialInfo> GetMaterialInfoFromPMX(string pmxFilePath)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
            var materialInfoList = new List<MaterialInfo>();

            try
            {   // 解析 PMX 文件
                MMDTools.PMXObject pmx = MMDTools.PMXParser.Parse(pmxFilePath);

                for (int i = 0; i < pmx.MaterialList.Length; i++)
                {
                    var material = pmx.MaterialList.Span[i];
                    string id = i.ToString();
                    string name = material.Name; //日文名称
                    string eng = material.NameEnglish; // 英文名称
                    string tex = material.Texture < pmx.TextureList.Length ? pmx.TextureList.Span[material.Texture] : " "; //Tex路径
                    string toon = material.ToonTexture < pmx.TextureList.Length ? pmx.TextureList.Span[material.ToonTexture] : " "; //Toon路径
                    string spa = material.SphereTextre < pmx.TextureList.Length ? pmx.TextureList.Span[material.SphereTextre] : " "; // Spa路径
                    // 添加到材质信息列表
                    materialInfoList.Add(new MaterialInfo
                    {
                        ID = id,
                        Name = name,
                        EnglishName = eng,
                        Texture = tex,
                        Toon = toon,
                        Spa = spa
                    });
                }
            }
            catch (Exception ex)
            {
                // 解析 PMX 文件时出错 提示
                MessageBox.Show(string.Format(rm.GetString("ErrorParsingPMXMessage"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return materialInfoList;
        }
        public class MaterialInfo
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string EnglishName { get; set; }
            public string Texture { get; set; }
            public string Toon { get; set; }
            public string Spa { get; set; }
        }
        //----------------------------------------
        // 预览材质窗口点击事件
        //----------------------------------------
        private void Preview_Click(object sender, EventArgs e)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
            if (materialPreview != null && !materialPreview.IsDisposed)
            {
                // 如果窗口已经打开，则关闭它
                materialPreview.Close();
                materialPreview = null;
            }
            else
            {
                string pmxFilePath = inputfilebox.Text; // 获取输入文件路径
                if (string.IsNullOrEmpty(pmxFilePath) || !File.Exists(pmxFilePath))
                {
                    // 如果 PMX 文件路径为空或文件不存在，弹出提示：请先选择一个有效的 PMX 文件
                    MessageBox.Show(rm.GetString("SelectValidPMXFileMessage"), rm.GetString("notice"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取材质信息
                List<MaterialInfo> materialInfoList = GetMaterialInfoFromPMX(pmxFilePath);

                // 创建并显示 MaterialPreview 窗口
                materialPreview = new MaterialPreview(materialInfoList);

                // 设置窗口位置在 Form1 窗口的右侧
                materialPreview.StartPosition = FormStartPosition.Manual;
                materialPreview.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                materialPreview.Show();

                // 更新 MaterialPreview 的语言
                materialPreview.UpdateLanguage(rm);
            }
        }

        // CustomRules.ini 文件按钮点击事件
        private void OpenCustomINIFILE_Click(object sender, EventArgs e)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

            // CustomRules.ini 路径
            string iniFilePath = Path.Combine(Application.StartupPath, "CustomRules.ini");

            try
            {
                // 检查文件是否存在
                if (!File.Exists(iniFilePath))
                {
                    // MessageBox.Show($"未找到文件：{iniFilePath}\n请确认文件路径是否正确", "提示",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(rm.GetString("CustominiPathError"), rm.GetString("notice"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 调用记事本程序打开指定的INI文件
                // notepad.exe 是系统记事本的固定名称，/A 参数表示以ANSI编码打开（可选）
                Process.Start(new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = $"\"{iniFilePath}\"", // 路径加双引号，避免路径含空格时出错
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // 文件打开失败，弹出提示：文件未找到
                MessageBox.Show(string.Format(rm.GetString("FileNotFailure"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

        //================ END =======================
}


