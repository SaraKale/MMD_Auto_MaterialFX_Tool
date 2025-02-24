using System;
using System.Collections.Generic;
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

            // 批量绑定控件与提示文本
            BindControlTooltips();

            // 初始化渲染器列表
            Exportoptions_list.Items.Clear(); // 清空列表，避免重复选项
            render_list.Items.AddRange(new string[] {
                "无",
                "Ray-MMD",
                "ikPolishShader",
                "PowerShader"
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

            // 初始化导出选项列表
            Exportoptions_list.Items.Clear(); // 清空列表，避免重复选项
            Exportoptions_list.Items.AddRange(new string[] {
                "无",
                "按图片文件名输出FX",
                "按材质名称输出FX",
                "按ID编号输出FX"
            });
            // Exportoptions_list.SelectedIndex = 0;  // 默认选择第一个选项

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
                    outputbox.Enabled = false;
                    output_button.Enabled = false;
                    Packimages.Enabled = false;
                }
                else
                {
                    outputbox.Text = Path.GetDirectoryName(files[0]);
                    outputbox.Enabled = true;
                    output_button.Enabled = true;
                    Packimages.Enabled = true;
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
                outputbox.Enabled = false;
                output_button.Enabled = false;
                Packimages.Enabled = false;
            }
            else
            {
                outputbox.Enabled = true;
                output_button.Enabled = true;
                Packimages.Enabled = true;
            }
        }

        // 输出后自动打开文件夹 选项
        private void ExportAfterFolder_text_CheckedChanged(object sender, EventArgs e)
        {

        }

        // 复制文件重试机制，解决文件占用问题
        //private void CopyFileWithRetry(string sourceFilePath, string destFilePath, int retryCount = 3, int delayMilliseconds = 1000)
        //{
        //    ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

        //    for (int i = 0; i < retryCount; i++)
        //    {
        //        try
        //        {
        //            // 添加调试信息，尝试复制文件
        //            // 尝试复制文件: {sourceFilePath} 到 {destFilePath} (尝试次数: {i + 1})
        //            Console.WriteLine(string.Format(rm.GetString("CopyFileAttemptMessage"), sourceFilePath, destFilePath, i + 1));
        //            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //            using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
        //            {
        //                sourceStream.CopyTo(destStream);
        //            }
        //            Console.WriteLine(rm.GetString("FileCopySuccessMessage")); // 文件复制成功
        //            return; // 成功复制文件，退出方法
        //        }
        //        catch (FileNotFoundException ex)
        //        {
        //            // 文件未找到，添加调试信息
        //            Console.WriteLine(string.Format(rm.GetString("FileNotFound"), ex.Message));
        //            throw new FileNotFoundException(string.Format(rm.GetString("FileNotFound"), ex.Message), ex);
        //        }
        //        catch (UnauthorizedAccessException ex)
        //        {
        //            // 无权限访问文件，添加调试信息
        //            Console.WriteLine(string.Format(rm.GetString("UnauthorizedAccess"), ex.Message));
        //            throw new UnauthorizedAccessException(string.Format(rm.GetString("UnauthorizedAccess"), ex.Message), ex);
        //        }
        //        catch (IOException ex)
        //        {
        //            if (i == retryCount - 1)
        //            {
        //                // 重试次数用尽，抛出异常
        //                string errorMessage = string.Format(rm.GetString("FileCopyError"), ex.Message); // 复制文件失败 提示
        //                Console.WriteLine(string.Format(rm.GetString("FileCopyError"), errorMessage));
        //                throw new IOException(errorMessage, ex);
        //            }
        //            // 等待一段时间后重试 文件被占用，等待 {delayMilliseconds} 毫秒后重试...
        //            Console.WriteLine(string.Format(rm.GetString("FileInUseConsoleMessage"), delayMilliseconds));
        //            System.Threading.Thread.Sleep(delayMilliseconds);
        //        }
        //        catch (Exception ex)
        //        {
        //            // 其他未知错误，添加调试信息
        //            Console.WriteLine(string.Format(rm.GetString("UnknownError"), ex.Message));
        //            throw new Exception(string.Format(rm.GetString("UnknownError"), ex.Message), ex);
        //        }
        //    }
        //}

        //=======================================
        // 最终输出按钮
        // 提取材质和纹理信息，生成材质文件和纹理文件，并创建 "_FX" 文件夹
        //=======================================
        private void export_button_Click(object sender, EventArgs e)
        {
            string inputFilePath = inputfilebox.Text; // 输入文件路径
            string outputFolderPath = outputbox.Text; // 输出文件夹路径

            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

            if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFolderPath))
            {
                // 未选择输入输出路径 提示：请先选择输入文件和输出文件夹
                MessageBox.Show(rm.GetString("SelectInputOutput"), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFolderPath))
            {
                // 材质导出成功提示
                MessageBox.Show(rm.GetString("ExportSuccess"), rm.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 读取 PMX 文件
                // 使用PMXParser库解析读取文件，PMXParser.Parse方法返回一个PMXObject对象，包含了PMX文件的所有信息
                MMDTools.PMXObject pmx = MMDTools.PMXParser.Parse(inputFilePath);

                // 使用字典存储纹理文件和对应的材质名称列表
                Dictionary<string, List<string>> textureMaterialMap = new Dictionary<string, List<string>>();
                foreach (var material in pmx.MaterialList.Span)
                {
                    // 若缺少材质纹理文件
                    string textureFile = material.Texture < pmx.TextureList.Length ? pmx.TextureList.Span[material.Texture] : rm.GetString("TextureFileMissing");

                    if (!textureMaterialMap.ContainsKey(textureFile))
                    {
                        textureMaterialMap[textureFile] = new List<string>();
                    }
                    textureMaterialMap[textureFile].Add(material.Name);
                }

                // 如果勾选了“源文件夹”选项，输出路径为纹理文件所在目录
                if (directoverwrite_text.Checked)
                {
                    outputFolderPath = GetTextureDirectoryFromPMX(inputFilePath);
                }

                // 创建 "_FX" 文件夹（如果未选择“源文件夹”选项）
                string fxFolderPath = outputFolderPath;
                string renderer = render_list.SelectedItem?.ToString() ?? "无"; // 渲染器列表

                if (!directoverwrite_text.Checked)
                {
                    if (renderer == "Ray-MMD"){
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); //自动生成时间戳
                        fxFolderPath = Path.Combine(outputFolderPath, "Ray" + $"_FX_{timestamp}");
                        if (!Directory.Exists(fxFolderPath))
                        {
                            Directory.CreateDirectory(fxFolderPath);
                        }
                    }
                    else if(renderer == "ikPolishShader")
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); //自动生成时间戳
                        fxFolderPath = Path.Combine(outputFolderPath, "ik" + $"_FX_{timestamp}");
                        if (!Directory.Exists(fxFolderPath))
                        {
                            Directory.CreateDirectory(fxFolderPath);
                        }
                    }
                    else if(renderer == "PowerShader")
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); //自动生成时间戳
                        fxFolderPath = Path.Combine(outputFolderPath, "PS" + $"_FX_{timestamp}");
                        if (!Directory.Exists(fxFolderPath))
                        {
                            Directory.CreateDirectory(fxFolderPath);
                        }
                    }
                    else
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); //自动生成时间戳
                        fxFolderPath = Path.Combine(outputFolderPath, $"_FX_{timestamp}");
                        if (!Directory.Exists(fxFolderPath))
                        {
                            Directory.CreateDirectory(fxFolderPath);
                        }
                    }

                }

                // 将材质和纹理信息写入文件
                string outputFilePath = Path.Combine(fxFolderPath, "MaterialInfo.txt");
                using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                {
                    foreach (var entry in textureMaterialMap)
                    {
                        writer.WriteLine(entry.Key);
                        foreach (var materialName in entry.Value)
                        {
                            writer.WriteLine(materialName);
                        }
                        writer.WriteLine();
                    }
                }

                //----------------------------------------
                // 复制所有纹理文件(包含子文件夹)到输出目录
                //----------------------------------------
                // 如果选择了“复制源图片到路径”，将会把所有图片复制到输出路径
                if (Packimages.Checked)
                {
                    // 支持的图片格式
                    var supportedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ".jpg", "jpeg", ".png", ".bmp", ".gif", ".tga", ".dds", ".svg", ".webp", ".tiff", ".avif"
                    };

                    // 假设纹理文件路径存储在 textureMaterialMap 中（键为纹理文件名，值为相关的材质）
                    foreach (var textureFile in textureMaterialMap.Keys)
                    {
                        // 获取纹理文件所在的完整路径
                        string sourceTexturePath = Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile);

                        // 获取纹理文件所在目录
                        string sourceDirectory = Path.GetDirectoryName(sourceTexturePath);

                        if (Directory.Exists(sourceDirectory))
                        {
                            // 获取源目录及其子目录下的所有文件
                            IEnumerable<string> allFiles = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories);

                            // 遍历所有文件
                            foreach (var file in allFiles)
                            {
                                // 获取文件扩展名
                                string extension = Path.GetExtension(file).ToLower();

                                // 如果文件是支持的图片格式
                                if (supportedExtensions.Contains(extension))
                                {
                                    // 获取文件名
                                    string textureFileName = Path.GetFileName(file);

                                    // 计算目标路径
                                    string relativePath = Path.GetDirectoryName(file).Substring(sourceDirectory.Length);
                                    string destDirectory = Path.Combine(fxFolderPath, relativePath.TrimStart(Path.DirectorySeparatorChar));

                                    // 确保目标文件夹存在
                                    if (!Directory.Exists(destDirectory))
                                    {
                                        Directory.CreateDirectory(destDirectory);
                                    }

                                    string destTexturePath = Path.Combine(destDirectory, textureFileName);

                                    // 给路径加上 \\?\ 前缀，解决路径长度限制
                                    string longSourceTexturePath = @"\\?\" + file;
                                    string longDestTexturePath = @"\\?\" + destTexturePath;

                                    try
                                    {   // 复制图片文件到输出目录，允许覆盖同名文件
                                        File.Copy(longSourceTexturePath, longDestTexturePath, true);
                                        //CopyFileWithRetry(longSourceTexturePath, longDestTexturePath); // 复制文件失败尝试
                                        Console.WriteLine(string.Format(rm.GetString("FileCopySuccess"), textureFileName)); // 文件 {textureFileName} 复制成功
                                    }
                                    catch (IOException)
                                    {   // 文件被占用，弹出提示：文件被占用
                                        MessageBox.Show(rm.GetString("FileInUseError"), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    catch (Exception ex)
                                    {
                                        // 复制图片文件失败，弹出提示：复制图片文件失败
                                        MessageBox.Show(string.Format(rm.GetString("FileCopyError"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {   // 源文件夹不存在，弹出提示：源文件夹不存在
                            MessageBox.Show(string.Format(rm.GetString("SourceDirectoryNotFound"), sourceDirectory), rm.GetString("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                //----------------------------------------
                // 导出材质文件
                //----------------------------------------
                // 处理渲染器和语言选择
                // string renderer = render_list.SelectedItem?.ToString() ?? "无"; // 渲染器列表
                bool includeNote = note_button.Checked;
                string materialLang = Meteriallang_button.SelectedItem?.ToString() ?? "无"; // FX文本语言
                string exportOption = Exportoptions_list.SelectedItem?.ToString() ?? rm.GetString("ExportOption_ByImageName"); // 按图片文件名输出FX

                // --------------------------------------
                // 处理 Material.emd 文件
                // --------------------------------------
                // 根据选择的语言设置编码
                Encoding encoding = new UTF8Encoding(false); // 使用不带 BOM 的 UTF-8 编码
                if (exportOption == rm.GetString("ExportOption_ByMaterialName")) // 按材质名称输出FX
                {
                    // 遍历每个材质，检测材质名称的语言，应对在 MMD 导入 .emd 文件时乱码的问题
                    foreach (var material in pmx.MaterialList.Span)
                    {
                        if (IsSimplifiedChinese(material.Name))
                        {
                            encoding = Encoding.GetEncoding("GB18030"); // GB18030 包含 GBK 和 GB2312 编码
                            break;
                        }
                        else if (IsTraditionalChinese(material.Name))
                        {
                            encoding = Encoding.GetEncoding("BIG5"); // BIG5 是繁体中文编码
                            break;
                        }
                        else if (IsJapanese(material.Name))
                        {
                            encoding = Encoding.GetEncoding("shift_jis"); // Shift-JIS 是日语编码
                            break;
                        }
                        else if (IsKorean(material.Name))
                        {
                            encoding = Encoding.GetEncoding("EUC-KR"); // EUC-KR 是韩语编码
                            break;
                        }
                        else if (IsRussian(material.Name))
                        {
                            encoding = Encoding.GetEncoding("KOI8-R"); // KOI8-R 是俄语编码
                            break;
                        }
                        else if (IsLatin(material.Name))
                        {
                            encoding = Encoding.GetEncoding("WINDOWS-1250"); // WINDOWS-1250 是拉丁语编码
                            break;
                        }
                    }
                }

                // 检查是否选择了导出选项和渲染器
                if (Exportoptions_list.SelectedItem != null && render_list.SelectedItem != null)
                {
                    // 创建 Material.emd 文件，用于在 MME 关联材质文件
                    string emdFilePath = Path.Combine(fxFolderPath, "Material_MME.emd");
                    using (StreamWriter emdWriter = new StreamWriter(emdFilePath, false, encoding))
                    {
                        emdWriter.WriteLine("[Info]");
                        emdWriter.WriteLine("Version = 3");
                        emdWriter.WriteLine();
                        emdWriter.WriteLine("[Effect]");
                        emdWriter.WriteLine("Obj = none"); // Main主栏
                        emdWriter.WriteLine("Obj.show = true"); // 阴影

                        // 如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            for (int i = 0; i < pmx.MaterialList.Length; i++)
                            {
                                var material = pmx.MaterialList.Span[i];
                                string materialTextureFile = material.Texture < pmx.TextureList.Length ? pmx.TextureList.Span[material.Texture] : rm.GetString("TextureFileMissing"); // 按图片文件名输出FX
                                string fxFileName = exportOption == rm.GetString("ExportOption_ByMaterialName") ? material.Name + ".fx" : // 按材质名称输出FX
                                                    exportOption == rm.GetString("ExportOption_ByID") ? i.ToString("D2") + ".fx" : // 按ID编号输出FX
                                                    Path.GetFileNameWithoutExtension(materialTextureFile) + ".fx";
                                emdWriter.WriteLine($"Obj[{i}] = {fxFileName}");
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为绝对路径
                        else
                        {
                            for (int i = 0; i < pmx.MaterialList.Length; i++)
                            {
                                var material = pmx.MaterialList.Span[i];
                                string materialTextureFile = material.Texture < pmx.TextureList.Length ? pmx.TextureList.Span[material.Texture] : rm.GetString("TextureFileMissing"); // 按图片文件名输出FX
                                string fxFileName = exportOption == rm.GetString("ExportOption_ByMaterialName") ? material.Name + ".fx" : // 按材质名称输出FX
                                                    exportOption == rm.GetString("ExportOption_ByID") ? i.ToString("D2") + ".fx" : // 按ID编号输出FX
                                                    Path.GetFileNameWithoutExtension(materialTextureFile) + ".fx";
                                string emdPath = Path.GetFullPath(Path.Combine(fxFolderPath, fxFileName)).Replace('/', '\\'); // 使用绝对路径转义，如：D:\Project\Material.fx
                                emdWriter.WriteLine($"Obj[{i}] = {emdPath}");
                            }
                        }
                    }
                    // 未选择导出选项或渲染器，弹出提示
                    //MessageBox.Show(rm.GetString("SelectExportOptionRenderer"), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //----------------------------------------
                // 处理每个材质文件
                // 根据选择的渲染器和语言，复制对应的材质文件和文件夹
                //----------------------------------------

                // 读取 CustomRules.ini 文件，解析规则
                var customRules = ReadCustomRules("CustomRules.ini");

                // 遍历每个材质文件
                for (int i = 0; i < pmx.MaterialList.Length; i++)
                {
                    var material = pmx.MaterialList.Span[i];
                    string textureFile = material.Texture < pmx.TextureList.Length ? pmx.TextureList.Span[material.Texture] : rm.GetString("TextureFileMissing"); // 按图片文件名输出FX
                    string destFile1 = Path.Combine(fxFolderPath, exportOption == rm.GetString("ExportOption_ByMaterialName") ? material.Name + ".fx" : // 按材质名称输出FX
                                                    exportOption == rm.GetString("ExportOption_ByID") ? i.ToString("D2") + ".fx" : // 按ID编号输出FX
                                                    Path.GetFileNameWithoutExtension(textureFile) + ".fx");
                    string sourceFile1 = "", sourceFile2 = "", sourceFolder = "", sourceFolder2 = "";
                    string destFile2 = "", destFile3 = "", destFile4 = "", destFile5 = "";
                    string sourceFile3 = "", sourceFile4 = "", sourceFile5 = "";

                    /*****************************
                    Ray-MMD 1.5.2  作者：Rui
                    关联文件：
                    main类：
                    Renderer\Ray-MMD\main.fxsub
                    Renderer\Ray-MMD\main.fx

                    材质类：
                    Renderer\Ray-MMD\material_2.0.fx
                    Renderer\Ray-MMD\material_common_2.0.fxsub
                    ***********************/
                    if (renderer == "Ray-MMD")
                    {
                        if (includeNote)
                        {
                            switch (materialLang)
                            {
                                case "English":
                                case "英语":
                                case "英語":
                                    sourceFile1 = @"Renderer\Ray-MMD\material_Note_en.fx";
                                    break;
                                case "Simplified Chinese":
                                case "简体中文":
                                case "簡體中文":
                                case "中国語簡体字":
                                    sourceFile1 = @"Renderer\Ray-MMD\material_Note_cn.fx";
                                    break;
                                case "Traditional Chinese":
                                case "繁体中文":
                                case "繁體中文":
                                case "中国語繁体字":
                                    sourceFile1 = @"Renderer\Ray-MMD\material_Note_tw.fx";
                                    break;
                                case "Japanese":
                                case "日语":
                                case "日語":
                                case "日本語":
                                    sourceFile1 = @"Renderer\Ray-MMD\material_Note_jp.fx";
                                    break;
                                default:
                                    sourceFile1 = @"Renderer\Ray-MMD\material_Note_en.fx";
                                    break;
                            }
                        }
                        else
                        {
                            sourceFile1 = @"Renderer\Ray-MMD\material_2.0.fx";
                        }

                        sourceFile2 = @"Renderer\Ray-MMD\material_common_2.0.fxsub"; //来源
                        destFile2 = Path.Combine(fxFolderPath, "material_common_2.0.fxsub"); //输出
                        // 复制文件到目标文件夹
                        File.Copy(sourceFile1, destFile1, true);
                        File.Copy(sourceFile2, destFile2, true);

                        // 读取 .fx 文件内容
                        string fxContent = File.ReadAllText(destFile1);

                        // 获取材质对应的贴图文件
                        string textureFileName = Path.GetFileNameWithoutExtension(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 使用正则提取文件前缀，支持 "_" 和 "-" 作为分隔符
                        Match match = Regex.Match(textureFileName, @"^(.*)[_-]([^_-]+)$");
                        string texturePrefix = match.Success ? match.Groups[1].Value : textureFileName;

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 Specular 规则的文件
                            var specularFiles = Directory.GetFiles(textureDirectory, $"*{rule.Specular}.*")
                                .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(texturePrefix))
                                .ToList();

                            if (specularFiles.Count > 0)
                            {
                                albedoFileName = Path.GetFileName(specularFiles.First());
                                albedoMatched = true;
                            }

                            // 查找符合 Normal 规则的文件
                            var normalFiles = Directory.GetFiles(textureDirectory, $"*{rule.Normal}.*")
                                .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(texturePrefix))
                                .ToList();

                            if (normalFiles.Count > 0)
                            {
                                normalFileName = Path.GetFileName(normalFiles.First());
                                normalMatched = true;
                            }

                            // 如果找到了匹配的 Albedo 和 Normal 贴图，则退出循环
                            if (albedoMatched && normalMatched)
                            {
                                break;
                            }
                        }

                        // 只有匹配时才进行替换
                        // 如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            // 替换高光贴图
                            if (albedoMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FROM\s+\d+", "#define ALBEDO_SUB_MAP_FROM 1");
                                fxContent = Regex.Replace(fxContent, @"#define\s+(ALBEDO_SUB_MAP_FILE)\s+""([^""]*)""", $"#define ALBEDO_SUB_MAP_FILE \"{albedoFileName}\"");
                            }
                            // 替换法线贴图
                            if (normalMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FROM\s+\d+", "#define NORMAL_MAP_FROM 1");
                                fxContent = Regex.Replace(fxContent, @"#define\s+(NORMAL_MAP_FILE)\s+""([^""]*)""", $"#define NORMAL_MAP_FILE \"{normalFileName}\"");
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为绝对路径
                        else
                        {
                            // 替换高光贴图
                            if (albedoMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FROM\s+\d+", "#define ALBEDO_SUB_MAP_FROM 1");
                                string combinedPath = Path.Combine(textureDirectory, albedoFileName).Replace('\\', '/'); // 路径转义，如：D:/Project/Albedo.png
                                fxContent = Regex.Replace(fxContent, @"#define\s+(ALBEDO_SUB_MAP_FILE)\s+""([^""]*)""", $"#define ALBEDO_SUB_MAP_FILE \"{combinedPath}\"");
                            }
                            // 替换法线贴图
                            if (normalMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FROM\s+\d+", "#define NORMAL_MAP_FROM 1");
                                string combinedPath = Path.Combine(textureDirectory, normalFileName).Replace('\\', '/'); // 路径转义，如：D:/Project/Normal.png
                                fxContent = Regex.Replace(fxContent, @"#define\s+(NORMAL_MAP_FILE)\s+""([^""]*)""", $"#define NORMAL_MAP_FILE \"{combinedPath}\"");
                            }
                        }

                        // 写入修改后的 .fx 文件
                        File.WriteAllText(destFile1, fxContent, new UTF8Encoding(false)); // 使用不带 BOM 的 UTF-8 编码
                    }
                    /*****************************
                    ikPolishShader  作者：ikeno

                    关联文件：
                    main类：
                    Renderer\ikPolishShader\PolishMain.fx

                    材质类：
                    Renderer\ikPolishShader\Material.fx
                    Renderer\ikPolishShader\Sources
                    ***********************/
                    else if (renderer == "ikPolishShader")
                    {
                        if (includeNote)
                        {
                            switch (materialLang)
                            {
                                case "English":
                                case "英语":
                                case "英語":
                                    sourceFile1 = @"Renderer\ikPolishShader\Material_Note_en.fx";
                                    break;
                                case "Simplified Chinese":
                                case "简体中文":
                                case "簡體中文":
                                case "中国語簡体字":
                                    sourceFile1 = @"Renderer\ikPolishShader\Material_Note_cn.fx";
                                    break;
                                case "Traditional Chinese":
                                case "繁体中文":
                                case "繁體中文":
                                case "中国語繁体字":
                                    sourceFile1 = @"Renderer\ikPolishShader\Material_Note_tw.fx";
                                    break;
                                case "Japanese":
                                case "日语":
                                case "日語":
                                case "日本語":
                                    sourceFile1 = @"Renderer\ikPolishShader\Material_Note_jp.fx";
                                    break;
                                default:
                                    sourceFile1 = @"Renderer\ikPolishShader\Material_Note_en.fx";
                                    break;
                            }
                        }
                        else
                        {
                            sourceFile1 = @"Renderer\ikPolishShader\Material.fx";
                        }

                        sourceFolder = @"Renderer\ikPolishShader\Sources";
                        sourceFolder2 = @"Renderer\ikPolishShader\Materials";
                        destFile2 = Path.Combine(fxFolderPath, "Sources");
                        destFile3 = Path.Combine(fxFolderPath, "Materials");
                        // 复制文件和文件夹到目标文件夹
                        File.Copy(sourceFile1, destFile1, true);
                        CopyDirectory(sourceFolder, destFile2);
                        CopyDirectory(sourceFolder2, destFile3);

                        // 读取 .fx 文件内容
                        string fxContent = File.ReadAllText(destFile1);

                        // 获取材质对应的贴图文件
                        string textureFileName = Path.GetFileNameWithoutExtension(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 使用正则提取文件前缀，支持 "_" 和 "-" 作为分隔符
                        Match match = Regex.Match(textureFileName, @"^(.*)[_-]([^_-]+)$");
                        string texturePrefix = match.Success ? match.Groups[1].Value : textureFileName;

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 Specular 规则的文件
                            var specularFiles = Directory.GetFiles(textureDirectory, $"*{rule.Specular}.*")
                                .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(texturePrefix))
                                .ToList();

                            if (specularFiles.Count > 0)
                            {
                                albedoFileName = Path.GetFileName(specularFiles.First());
                                albedoMatched = true;
                            }

                            // 查找符合 Normal 规则的文件
                            var normalFiles = Directory.GetFiles(textureDirectory, $"*{rule.Normal}.*")
                                .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(texturePrefix))
                                .ToList();

                            if (normalFiles.Count > 0)
                            {
                                normalFileName = Path.GetFileName(normalFiles.First());
                                normalMatched = true;
                            }

                            // 如果找到了匹配的 Albedo 和 Normal 贴图，则退出循环
                            if (albedoMatched && normalMatched)
                            {
                                break;
                            }
                        }

                        // 只有匹配时才进行替换
                        // 如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            // 替换高光贴图
                            if (albedoMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MAP_ENABLE\s+\d+", "#define ALBEDO_MAP_ENABLE 1");
                                fxContent = Regex.Replace(fxContent, @"#define\s+(TEXTURE_FILENAME_1)\s+""([^""]*)""", $"#define TEXTURE_FILENAME_1 \"{albedoFileName}\"");
                            }
                            // 替换法线贴图
                            if (normalMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMALMAP_ENABLE\s+\d+", "#define NORMALMAP_ENABLE 1");
                                fxContent = Regex.Replace(fxContent, @"#define\s+(NORMALMAP_MAIN_FILENAME)\s+""([^""]*)""", $"#define NORMALMAP_MAIN_FILENAME \"{normalFileName}\"");
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为绝对路径
                        else 
                        {
                            // 替换高光贴图
                            if (albedoMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MAP_ENABLE\s+\d+", "#define ALBEDO_MAP_ENABLE 1");
                                string combinedPath = Path.Combine(textureDirectory, albedoFileName).Replace('\\', '/'); // 路径转义，如：D:/Project/Albedo.png
                                fxContent = Regex.Replace(fxContent, @"#define\s+(TEXTURE_FILENAME_1)\s+""([^""]*)""", $"#define TEXTURE_FILENAME_1 \"{combinedPath}\"");
                            }
                            // 替换法线贴图
                            if (normalMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMALMAP_ENABLE\s+\d+", "#define NORMALMAP_ENABLE 1");
                                string combinedPath = Path.Combine(textureDirectory, normalFileName).Replace('\\', '/'); // 路径转义，如：D:/Project/Normal.png
                                fxContent = Regex.Replace(fxContent, @"#define\s+(NORMALMAP_MAIN_FILENAME)\s+""([^""]*)""", $"#define NORMALMAP_MAIN_FILENAME \"{combinedPath}\"");
                            }
                        }

                        // 写入修改后的 .fx 文件
                        File.WriteAllText(destFile1, fxContent, new UTF8Encoding(false)); // 使用不带 BOM 的 UTF-8 编码
                    }

                    /*****************************
                    PowerShader  作者：角砂糖

                    关联文件：
                    Renderer\PowerShader\Shader_Main.fx
                    Renderer\PowerShader\Common_Shader.fxsub
                    Renderer\PowerShader\Config.fxsub
                    Renderer\PowerShader\shading_hint_toon.png
                    Renderer\PowerShader\PSController.pmx
                    ***********************/
                    else if (renderer == "PowerShader")
                    {
                        if (includeNote)
                        {
                            switch (materialLang)
                            {
                                case "English":
                                case "英语":
                                case "英語":
                                    sourceFile1 = @"Renderer\PowerShader\Shader_Note_en.fx";
                                    break;
                                case "Simplified Chinese":
                                case "简体中文":
                                case "簡體中文":
                                case "中国語簡体字":
                                    sourceFile1 = @"Renderer\PowerShader\Shader_Note_cn.fx";
                                    break;
                                case "Traditional Chinese":
                                case "繁体中文":
                                case "繁體中文":
                                case "中国語繁体字":
                                    sourceFile1 = @"Renderer\PowerShader\Shader_Note_tw.fx";
                                    break;
                                case "Japanese":
                                case "日语":
                                case "日語":
                                case "日本語":
                                    sourceFile1 = @"Renderer\PowerShader\Shader_Note_jp.fx";
                                    break;
                                default:
                                    sourceFile1 = @"Renderer\PowerShader\Shader_Note_en.fx";
                                    break;
                            }
                        }
                        else
                        {
                            sourceFile1 = @"Renderer\PowerShader\Shader_Main.fx";
                        }

                        sourceFile2 = @"Renderer\PowerShader\Common_Shader.fxsub"; //来源
                        destFile2 = Path.Combine(fxFolderPath, "Common_Shader.fxsub"); //输出
                        sourceFile3 = @"Renderer\PowerShader\Config.fxsub"; //来源
                        destFile3 = Path.Combine(fxFolderPath, "Config.fxsub"); //输出
                        sourceFile4 = @"Renderer\PowerShader\shading_hint_toon.png"; //来源
                        destFile4 = Path.Combine(fxFolderPath, "shading_hint_toon.png"); //输出
                        sourceFile5 = @"Renderer\PowerShader\PSController.pmx"; //来源
                        destFile5 = Path.Combine(fxFolderPath, "PSController.pmx"); //输出
                        // 复制文件到目标文件夹
                        File.Copy(sourceFile1, destFile1, true);
                        File.Copy(sourceFile2, destFile2, true);
                        File.Copy(sourceFile3, destFile3, true);
                        File.Copy(sourceFile4, destFile4, true);
                        File.Copy(sourceFile5, destFile5, true);

                        // 读取 .fx 文件内容
                        string fxContent = File.ReadAllText(destFile1);

                        // 获取材质对应的贴图文件
                        string textureFileName = Path.GetFileNameWithoutExtension(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 使用正则提取文件前缀，支持 "_" 和 "-" 作为分隔符
                        Match match = Regex.Match(textureFileName, @"^(.*)[_-]([^_-]+)$");
                        string texturePrefix = match.Success ? match.Groups[1].Value : textureFileName;

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 Specular 规则的文件
                            var specularFiles = Directory.GetFiles(textureDirectory, $"*{rule.Specular}.*")
                                .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(texturePrefix))
                                .ToList();

                            if (specularFiles.Count > 0)
                            {
                                albedoFileName = Path.GetFileName(specularFiles.First());
                                albedoMatched = true;
                            }

                            // 查找符合 Normal 规则的文件
                            var normalFiles = Directory.GetFiles(textureDirectory, $"*{rule.Normal}.*")
                                .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(texturePrefix))
                                .ToList();

                            if (normalFiles.Count > 0)
                            {
                                normalFileName = Path.GetFileName(normalFiles.First());
                                normalMatched = true;
                            }

                            // 如果找到了匹配的 Albedo 和 Normal 贴图，则退出循环
                            if (albedoMatched && normalMatched)
                            {
                                break;
                            }
                        }

                        // 只有匹配时才进行替换
                        // 如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            if (albedoMatched)
                            {
                                // 高光贴图开启
                                fxContent = Regex.Replace(fxContent, @"#define\s+EMISSIVE_FROM\s+\d+", "#define EMISSIVE_FROM 2");
                                // 替换高光贴图文件名
                                fxContent = Regex.Replace(fxContent, @"#define\s+(EMISSIVE_TEXTURE)\s+""([^""]*)""", $"#define EMISSIVE_TEXTURE \"{albedoFileName}\"");
                            }
                            if (normalMatched)
                            {
                                // 法线贴图开启
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_FROM\s+\d+", "#define NORMAL_FROM 2");
                                // 替换法线贴图文件名
                                fxContent = Regex.Replace(fxContent, @"#define\s+(NORMAL_TEXTURE)\s+""([^""]*)""", $"#define NORMAL_TEXTURE \"{normalFileName}\"");
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为绝对路径
                        else {
                            if (albedoMatched)
                            {
                                // 高光贴图开启
                                fxContent = Regex.Replace(fxContent, @"#define\s+EMISSIVE_FROM\s+\d+", "#define EMISSIVE_FROM 2");
                                // 替换高光贴图文件名
                                string combinedPath = Path.Combine(textureDirectory, albedoFileName).Replace('\\', '/'); // 路径转义，如：D:/Project/Albedo.png
                                fxContent = Regex.Replace(fxContent, @"#define\s+(EMISSIVE_TEXTURE)\s+""([^""]*)""", $"#define EMISSIVE_TEXTURE \"{combinedPath}\"");
                            }
                            if (normalMatched)
                            {
                                // 法线贴图开启
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_FROM\s+\d+", "#define NORMAL_FROM 2");
                                // 替换法线贴图文件名
                                string combinedPath = Path.Combine(textureDirectory, normalFileName).Replace('\\', '/'); // 路径转义，如：D:/Project/Normal.png
                                fxContent = Regex.Replace(fxContent, @"#define\s+(NORMAL_TEXTURE)\s+""([^""]*)""", $"#define NORMAL_TEXTURE \"{combinedPath}\"");
                            }
                        }

                        // 写入修改后的 .fx 文件
                        File.WriteAllText(destFile1, fxContent, new UTF8Encoding(false)); // 使用不带 BOM 的 UTF-8 编码
                    }
                }

                // 导出成功，弹出提示：材质和纹理信息提取成功！
                MessageBox.Show(rm.GetString("ExportSuccess"), rm.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 如果勾选了“输出后自动打开文件夹”选项，自动打开导出路径的文件夹
                if (ExportAfterFolder_text.Checked)
                {
                    System.Diagnostics.Process.Start("explorer.exe", fxFolderPath);
                }

            }
            //================ if end =======================
            catch (FileNotFoundException ex)
            {
                // 文件未找到，弹出提示：文件未找到
                MessageBox.Show(string.Format(rm.GetString("FileNotFound"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                // 无权限访问文件，弹出提示：无权限访问文件
                MessageBox.Show(string.Format(rm.GetString("UnauthorizedAccess"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                // 文件读写错误，弹出提示：文件读写错误
                MessageBox.Show(string.Format(rm.GetString("IOException"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException ex)
            {
                // 文件格式错误，弹出提示：文件格式错误
                MessageBox.Show(string.Format(rm.GetString("FormatException"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                // 索引超出数组界限，弹出提示：索引超出了数组界限
                MessageBox.Show(string.Format(rm.GetString("IndexOutOfRangeException"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // 其他未知错误，弹出提示：发生未知错误
                MessageBox.Show(string.Format(rm.GetString("UnknownError"), ex.Message), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //=======================================
        // 判断.emd文件的编码语言
        //=======================================
        // 判断材质名称是否为简体中文
        private bool IsSimplifiedChinese(string text)
        {
            return text.Any(c => c >= 0x4E00 && c <= 0x9FFF);
        }

        // 判断材质名称是否为繁体中文
        private bool IsTraditionalChinese(string text)
        {
            return text.Any(c => c >= 0x3400 && c <= 0x4DBF);
        }

        // 判断材质名称是否为日语
        private bool IsJapanese(string text)
        {
            return text.Any(c => (c >= 0x3040 && c <= 0x309F) || (c >= 0x30A0 && c <= 0x30FF) || (c >= 0x31F0 && c <= 0x31FF));
        }

        // 判断材质名称是否为韩语
        private bool IsKorean(string text)
        {
            return text.Any(c => (c >= 0x1100 && c <= 0x11FF) || (c >= 0x3130 && c <= 0x318F) || (c >= 0xAC00 && c <= 0xD7AF));
        }

        // 判断材质名称是否为俄语
        private bool IsRussian(string text)
        {
            return text.Any(c => c >= 0x0400 && c <= 0x04FF);
        }

        // 判断材质名称是否为拉丁字符
        private bool IsLatin(string text)
        {
            return text.Any(c => c >= 0x0020 && c <= 0x007F);
        }

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
            public string Diffuse { get; set; }
            public string Specular { get; set; }
            public string Normal { get; set; }
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
                // 解析 PMX 文件时出错 提示 ParsPmxerror
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

        //================ END =======================

    }
}

