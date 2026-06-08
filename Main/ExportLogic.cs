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
    public partial class Form1 : Form
    {

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
                string renderer = render_list.SelectedItem?.ToString() ?? rm.GetString("Renderer_None"); // 渲染器列表

                if (!directoverwrite_text.Checked)
                {
                    if (renderer == rm.GetString("Renderer_RayMMD") || renderer == rm.GetString("Renderer_RaySJMatcap"))
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); //自动生成时间戳
                        fxFolderPath = Path.Combine(outputFolderPath, "Ray" + $"_FX_{timestamp}");
                        if (!Directory.Exists(fxFolderPath))
                        {
                            Directory.CreateDirectory(fxFolderPath);
                        }
                    }
                    else if (renderer == rm.GetString("Renderer_ikPolishShader"))
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); //自动生成时间戳
                        fxFolderPath = Path.Combine(outputFolderPath, "ik" + $"_FX_{timestamp}");
                        if (!Directory.Exists(fxFolderPath))
                        {
                            Directory.CreateDirectory(fxFolderPath);
                        }
                    }
                    else if (renderer == rm.GetString("Renderer_PowerShader"))
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

                // --------------------------------------
                // 处理 Material.emd 文件
                // --------------------------------------
                // 处理渲染器和语言选择
                bool includeNote = note_button.Checked;
                string materialLang = Meteriallang_button.SelectedItem?.ToString() ?? "无"; // FX文本语言
                string exportOption = Exportoptions_list.SelectedItem?.ToString() ?? rm.GetString("ExportOption_ByImageName"); // 按图片文件名输出FX

                // 根据系统语言设置编码
                Encoding encoding = new UTF8Encoding(false); // 默认UTF-8编码（无BOM）
                var currentCulture = System.Globalization.CultureInfo.CurrentCulture.Name;
                if (currentCulture.StartsWith("zh-CN"))
                {
                    encoding = Encoding.GetEncoding("GB18030"); // 简体中文
                }
                else if (currentCulture.StartsWith("zh-TW"))
                {
                    encoding = Encoding.GetEncoding("BIG5"); // 繁体中文
                }
                else if (currentCulture.StartsWith("ja"))
                {
                    encoding = Encoding.GetEncoding("shift_jis"); // 日语
                }
                else if (currentCulture.StartsWith("ko"))
                {
                    encoding = Encoding.GetEncoding("EUC-KR"); // 韩语
                }
                else if (currentCulture.StartsWith("ru"))
                {
                    encoding = Encoding.GetEncoding("KOI8-R"); // 俄语
                }
                else if (currentCulture.StartsWith("en"))
                {
                    encoding = Encoding.GetEncoding("WINDOWS-1252"); // 英文拉丁语
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
                        //emdWriter.WriteLine("Obj.show = true"); // 开启阴影，不建议开启，ikPolishShader渲染器加载emd文件会报错

                        // 遍历材质列表，输出FX路径
                        for (int i = 0; i < pmx.MaterialList.Length; i++)
                        {
                            var material = pmx.MaterialList.Span[i];
                            string materialTextureFile = material.Texture < pmx.TextureList.Length
                                ? pmx.TextureList.Span[material.Texture]
                                : rm.GetString("TextureFileMissing"); // 纹理丢失提示

                            // 统一都按系统语言编码输出
                            string fxFileName = exportOption == rm.GetString("ExportOption_ByMaterialName") ? material.Name + ".fx" : // 按材质名称:
                                                exportOption == rm.GetString("ExportOption_ByID") ? i.ToString("D2") + ".fx" : // 按ID编号
                                                exportOption == rm.GetString("ExportOption_ByIDAndMaterialName") ? "[" + i.ToString("D2") + "]" + material.Name + ".fx" : // 按ID编号+材质名称
                                                Path.GetFileNameWithoutExtension(materialTextureFile) + ".fx"; // 按图片文件名

                            // 输出路径 - 自定义路径/相对路径
                            string emdPath = Path.Combine(fxFolderPath, fxFileName).Replace('\\', '/'); // 绝对路径转义，如：D:/Project/Albedo.png
                            if (fxFolderPath.StartsWith(Path.GetDirectoryName(inputFilePath)))
                            {
                                Uri baseUri = new Uri(fxFolderPath + "/");
                                Uri fileUri = new Uri(emdPath);
                                emdPath = baseUri.MakeRelativeUri(fileUri).ToString().Replace('\\', '/'); // 相对路径，如：../Project/Albedo.png
                            }
                            // 确保路径输出用正确编码
                            emdWriter.WriteLine($"Obj[{i}] = {emdPath}");
                        }
                    }
                }
                else
                {
                    // 未选择导出选项或渲染器，弹出提示
                    MessageBox.Show(rm.GetString("SelectExportOptionRenderer"), rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                        exportOption == rm.GetString("ExportOption_ByIDAndMaterialName") ? "[" + i.ToString("D2") + "]" + material.Name + ".fx" : // 按ID编号+材质名称输出FX
                                       Path.GetFileNameWithoutExtension(textureFile) + ".fx");
                    string sourceFile1 = "", sourceFile2 = "", sourceFolder = "";
                    string destFile2 = "", destFile3 = "", destFile4 = "", destFile5 = "";
                    string sourceFile3 = "", sourceFile4 = "", sourceFile5 = "";

                    /*****************************
                    Ray-MMD 1.5.2  作者：Rui

                    关联文件：
                    Renderer\Ray-MMD\material_2.0.fx
                    Renderer\Ray-MMD\material_common_2.0.fxsub
                    ***********************/
                    if (renderer == rm.GetString("Renderer_RayMMD"))
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
                        string textureFileName = Path.GetFileName(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 获取 CustomRules.ini 路径
                        string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomRules.ini");
                        var suffixes = LoadAllSuffixes(iniPath);

                        // 提取前缀（不含扩展名）
                        string fileNameNoExt = Path.GetFileNameWithoutExtension(textureFileName);
                        string texturePrefix = ExtractPrefixFromFilename(fileNameNoExt, suffixes);

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        string smoothnessFileName = "";
                        string roughnessFileName = "";
                        string metalnessFileName = "";
                        string OcclusionFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;
                        bool smoothnessMatched = false;
                        bool roughnessMatched = false;
                        bool metalnessMatched = false;
                        bool OcclusionMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 高光 Specular 规则的文件
                            if (!albedoMatched && !string.IsNullOrEmpty(rule.Specular))
                            {
                                var specularFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Specular}.*").ToList();
                                if (specularFiles.Any())
                                {
                                    albedoFileName = Path.GetFileName(specularFiles.First());
                                    albedoMatched = true;
                                }
                            }

                            // 查找符合 法线 Normal 规则的文件
                            if (!normalMatched && !string.IsNullOrEmpty(rule.Normal))
                            {
                                var normalFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Normal}.*").ToList();
                                if (normalFiles.Any())
                                {
                                    normalFileName = Path.GetFileName(normalFiles.First());
                                    normalMatched = true;
                                }
                            }

                            // 查找符合 光滑度 Smoothness 规则的文件
                            if (!smoothnessMatched && !string.IsNullOrEmpty(rule.Smoothness))
                            {
                                var smoothnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Smoothness}.*").ToList();
                                if (smoothnessFiles.Any())
                                {
                                    smoothnessFileName = Path.GetFileName(smoothnessFiles.First());
                                    smoothnessMatched = true;
                                }
                            }

                            // 查找符合 粗糙度 Roughness 规则的文件
                            if (!roughnessMatched && !string.IsNullOrEmpty(rule.Roughness))
                            {
                                var roughnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Roughness}.*").ToList();
                                if (roughnessFiles.Any())
                                {
                                    roughnessFileName = Path.GetFileName(roughnessFiles.First());
                                    roughnessMatched = true;
                                }
                            }

                            // 查找符合 金属度 Metalness 规则的文件
                            if (!metalnessMatched && !string.IsNullOrEmpty(rule.Metalness))
                            {
                                var metalnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Metalness}.*").ToList();
                                if (metalnessFiles.Any())
                                {
                                    metalnessFileName = Path.GetFileName(metalnessFiles.First());
                                    metalnessMatched = true;
                                }
                            }

                            // 查找符合 环境光遮蔽 Occlusion 规则的文件
                            if (!OcclusionMatched && !string.IsNullOrEmpty(rule.Occlusion))
                            {
                                var OcclusionFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Occlusion}.*").ToList();
                                if (OcclusionFiles.Any())
                                {
                                    OcclusionFileName = Path.GetFileName(OcclusionFiles.First());
                                    OcclusionMatched = true;
                                }
                            }

                            // 如果所有贴图都找到了，则可以提前退出循环
                            if (albedoMatched && normalMatched && smoothnessMatched && roughnessMatched && metalnessMatched)
                            {
                                break;
                            }
                        }

                        // 辅助函数：获取相对或绝对路径（始终使用 / 斜杠）
                        string GetRelativeOrAbsolutePath(string fileName)
                        {
                            string combinedPath = Path.Combine(textureDirectory, fileName).Replace('\\', '/');
                            if (fxFolderPath.StartsWith(Path.GetDirectoryName(inputFilePath)))
                            {
                                Uri baseUri = new Uri(fxFolderPath + "/");
                                Uri fileUri = new Uri(combinedPath);
                                return baseUri.MakeRelativeUri(fileUri).ToString().Replace('\\', '/');
                            }
                            return combinedPath;
                        }

                        // 只有匹配时才进行替换，如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            if (albedoMatched) // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FROM\s+\d+", "#define ALBEDO_SUB_MAP_FROM 1"); // 开启高光贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FILE\s+""([^""]*)""", $"#define ALBEDO_SUB_MAP_FILE \"{albedoFileName}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FROM\s+\d+", "#define NORMAL_MAP_FROM 1"); // 开启法线贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FILE\s+""([^""]*)""", $"#define NORMAL_MAP_FILE \"{normalFileName}\""); // 替换法线贴图文件路径
                            }
                            if (smoothnessMatched || roughnessMatched)  // 替换光滑度或粗糙度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FROM\s+\d+", "#define SMOOTHNESS_MAP_FROM 1");  // 开启光滑度贴图
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;  // 优先选择光滑度贴图，如果光滑度smoothnessFileName为空，就用粗糙度roughnessFileName
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+""([^""]*)""", $"#define SMOOTHNESS_MAP_FILE \"{selectedFile}\""); // 替换光滑度贴图文件路径
                            }
                            if (metalnessMatched)  // 替换金属度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FROM\s+\d+", "#define METALNESS_MAP_FROM 1"); // 开启金属度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FILE\s+""([^""]*)""", $"#define METALNESS_MAP_FILE \"{metalnessFileName}\""); // 替换金属度贴图文件路径
                            }
                            if (OcclusionMatched)  // 替换环境光遮蔽贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FROM\s+\d+", "#define OCCLUSION_MAP_FROM 1"); // 开启环境光遮蔽贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FILE\s+""([^""]*)""", $"#define OCCLUSION_MAP_FILE \"{OcclusionFileName}\""); // 替换环境光遮蔽贴图文件路径
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为相对路径或绝对路径
                        else
                        {
                            if (albedoMatched)  // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FROM\s+\d+", "#define ALBEDO_SUB_MAP_FROM 1"); // 开启高光贴图
                                string path = GetRelativeOrAbsolutePath(albedoFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FILE\s+""([^""]*)""", $"#define ALBEDO_SUB_MAP_FILE \"{path}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FROM\s+\d+", "#define NORMAL_MAP_FROM 1"); // 开启法线贴图
                                string path = GetRelativeOrAbsolutePath(normalFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FILE\s+""([^""]*)""", $"#define NORMAL_MAP_FILE \"{path}\""); // 替换法线贴图文件路径
                            }
                            if (smoothnessMatched || roughnessMatched)  // 替换光滑度或粗糙度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FROM\s+\d+", "#define SMOOTHNESS_MAP_FROM 1"); // 开启光滑度贴图
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;  // 优先选择光滑度贴图，如果smoothnessFileName为空，就用roughnessFileName
                                string path = GetRelativeOrAbsolutePath(selectedFile);
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+""([^""]*)""", $"#define SMOOTHNESS_MAP_FILE \"{path}\""); // 替换光滑度贴图文件路径
                            }
                            if (metalnessMatched)  // 替换金属度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FROM\s+\d+", "#define METALNESS_MAP_FROM 1"); // 开启金属度贴图
                                string path = GetRelativeOrAbsolutePath(metalnessFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FILE\s+""([^""]*)""", $"#define METALNESS_MAP_FILE \"{path}\""); // 替换金属度贴图文件路径
                            }
                            if (OcclusionMatched)  // 替换环境光遮蔽贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FROM\s+\d+", "#define OCCLUSION_MAP_FROM 1"); // 开启环境光遮蔽贴图
                                string path = GetRelativeOrAbsolutePath(OcclusionFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FILE\s+""([^""]*)""", $"#define OCCLUSION_MAP_FILE \"{path}\""); // 替换环境光遮蔽贴图文件路径
                            }
                        }

                        // 如果没有匹配到任何贴图则跳过写入 .fx 文件
                        if (!albedoMatched && !normalMatched && !smoothnessMatched && !roughnessMatched && !metalnessMatched)
                        {
                            continue; // 跳过当前材质
                        }

                        // 如果匹配到了贴图，才写入 fx 文件
                        File.WriteAllText(destFile1, fxContent, new UTF8Encoding(false)); // 使用不带 BOM 的 UTF-8 编码
                    }


                    /*****************************
                    Ray-MMD 乘算/加算Sph  作者：三金络合物

                    关联文件：
                    Renderer\SJ_Matcap_1.2(152)\SJ_Matcap.fx
                    Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_1.0(152).fxsub
                    ***********************/
                    else if (renderer == rm.GetString("Renderer_RaySJMatcap"))
                    {
                        if (includeNote)
                        {
                            switch (materialLang)
                            {
                                case "English":
                                case "英语":
                                case "英語":
                                    sourceFile1 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_Note_en.fx";
                                    break;
                                case "Simplified Chinese":
                                case "简体中文":
                                case "簡體中文":
                                case "中国語簡体字":
                                    sourceFile1 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_Note_cn.fx";
                                    break;
                                case "Traditional Chinese":
                                case "繁体中文":
                                case "繁體中文":
                                case "中国語繁体字":
                                    sourceFile1 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_Note_tw.fx";
                                    break;
                                case "Japanese":
                                case "日语":
                                case "日語":
                                case "日本語":
                                    sourceFile1 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_Note_jp.fx";
                                    break;
                                default:
                                    sourceFile1 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_Note_en.fx";
                                    break;
                            }
                        }
                        else
                        {
                            sourceFile1 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap.fx";
                        }

                        sourceFile2 = @"Renderer\SJ_Matcap_1.2(152)\SJ_Matcap_1.0(152).fxsub"; //来源
                        destFile2 = Path.Combine(fxFolderPath, "SJ_Matcap_1.0(152).fxsub"); //输出
                        // 复制文件到目标文件夹
                        File.Copy(sourceFile1, destFile1, true);
                        File.Copy(sourceFile2, destFile2, true);

                        // 读取 .fx 文件内容
                        string fxContent = File.ReadAllText(destFile1);

                        // 获取材质对应的贴图文件
                        string textureFileName = Path.GetFileName(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 获取 CustomRules.ini 路径
                        string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomRules.ini");
                        var suffixes = LoadAllSuffixes(iniPath);

                        // 提取前缀（不含扩展名）
                        string fileNameNoExt = Path.GetFileNameWithoutExtension(textureFileName);
                        string texturePrefix = ExtractPrefixFromFilename(fileNameNoExt, suffixes);

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        string smoothnessFileName = "";
                        string roughnessFileName = "";
                        string metalnessFileName = "";
                        string OcclusionFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;
                        bool smoothnessMatched = false;
                        bool roughnessMatched = false;
                        bool metalnessMatched = false;
                        bool OcclusionMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 高光 Specular 规则的文件
                            if (!albedoMatched && !string.IsNullOrEmpty(rule.Specular))
                            {
                                var specularFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Specular}.*").ToList();
                                if (specularFiles.Any())
                                {
                                    albedoFileName = Path.GetFileName(specularFiles.First());
                                    albedoMatched = true;
                                }
                            }

                            // 查找符合 法线 Normal 规则的文件
                            if (!normalMatched && !string.IsNullOrEmpty(rule.Normal))
                            {
                                var normalFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Normal}.*").ToList();
                                if (normalFiles.Any())
                                {
                                    normalFileName = Path.GetFileName(normalFiles.First());
                                    normalMatched = true;
                                }
                            }

                            // 查找符合 光滑度 Smoothness 规则的文件
                            if (!smoothnessMatched && !string.IsNullOrEmpty(rule.Smoothness))
                            {
                                var smoothnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Smoothness}.*").ToList();
                                if (smoothnessFiles.Any())
                                {
                                    smoothnessFileName = Path.GetFileName(smoothnessFiles.First());
                                    smoothnessMatched = true;
                                }
                            }

                            // 查找符合 粗糙度 Roughness 规则的文件
                            if (!roughnessMatched && !string.IsNullOrEmpty(rule.Roughness))
                            {
                                var roughnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Roughness}.*").ToList();
                                if (roughnessFiles.Any())
                                {
                                    roughnessFileName = Path.GetFileName(roughnessFiles.First());
                                    roughnessMatched = true;
                                }
                            }

                            // 查找符合 金属度 Metalness 规则的文件
                            if (!metalnessMatched && !string.IsNullOrEmpty(rule.Metalness))
                            {
                                var metalnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Metalness}.*").ToList();
                                if (metalnessFiles.Any())
                                {
                                    metalnessFileName = Path.GetFileName(metalnessFiles.First());
                                    metalnessMatched = true;
                                }
                            }

                            // 查找符合 环境光遮蔽 Occlusion 规则的文件
                            if (!OcclusionMatched && !string.IsNullOrEmpty(rule.Occlusion))
                            {
                                var OcclusionFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Occlusion}.*").ToList();
                                if (OcclusionFiles.Any())
                                {
                                    OcclusionFileName = Path.GetFileName(OcclusionFiles.First());
                                    OcclusionMatched = true;
                                }
                            }

                            // 如果所有贴图都找到了，则可以提前退出循环
                            if (albedoMatched && normalMatched && smoothnessMatched && roughnessMatched && metalnessMatched)
                            {
                                break;
                            }
                        }

                        // 辅助函数：获取相对或绝对路径（始终使用 / 斜杠）
                        string GetRelativeOrAbsolutePath(string fileName)
                        {
                            string combinedPath = Path.Combine(textureDirectory, fileName).Replace('\\', '/');
                            if (fxFolderPath.StartsWith(Path.GetDirectoryName(inputFilePath)))
                            {
                                Uri baseUri = new Uri(fxFolderPath + "/");
                                Uri fileUri = new Uri(combinedPath);
                                return baseUri.MakeRelativeUri(fileUri).ToString().Replace('\\', '/');
                            }
                            return combinedPath;
                        }

                        // 获取 Spa 纹理路径
                        string spaFileName = material.SphereTextre < pmx.TextureList.Length ? pmx.TextureList.Span[material.SphereTextre] : "";
                        bool spaMatched = !string.IsNullOrWhiteSpace(spaFileName) && spaFileName != rm.GetString("TextureFileMissing");
                        if (spaMatched)
                        {
                            spaFileName = Path.GetFileName(spaFileName);
                        }

                        // 只有匹配时才进行替换，如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            if (albedoMatched) // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FROM\s+\d+", "#define ALBEDO_SUB_MAP_FROM 1"); // 开启高光贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FILE\s+""([^""]*)""", $"#define ALBEDO_SUB_MAP_FILE \"{albedoFileName}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FROM\s+\d+", "#define NORMAL_MAP_FROM 1"); // 开启法线贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FILE\s+""([^""]*)""", $"#define NORMAL_MAP_FILE \"{normalFileName}\""); // 替换法线贴图文件路径
                            }
                            if (smoothnessMatched || roughnessMatched)  // 替换光滑度或粗糙度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FROM\s+\d+", "#define SMOOTHNESS_MAP_FROM 1");  // 开启光滑度贴图
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;  // 优先选择光滑度贴图，如果光滑度smoothnessFileName为空，就用粗糙度roughnessFileName
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+""([^""]*)""", $"#define SMOOTHNESS_MAP_FILE \"{selectedFile}\""); // 替换光滑度贴图文件路径
                            }
                            if (metalnessMatched)  // 替换金属度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FROM\s+\d+", "#define METALNESS_MAP_FROM 1"); // 开启金属度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FILE\s+""([^""]*)""", $"#define METALNESS_MAP_FILE \"{metalnessFileName}\""); // 替换金属度贴图文件路径
                            }
                            if (OcclusionMatched)  // 替换环境光遮蔽贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FROM\s+\d+", "#define OCCLUSION_MAP_FROM 1"); // 开启环境光遮蔽贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FILE\s+""([^""]*)""", $"#define OCCLUSION_MAP_FILE \"{OcclusionFileName}\""); // 替换环境光遮蔽贴图文件路径
                            }
                            if (spaMatched)  // 替换 Spa 贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MATCAP_ENABLE\s+\d+", "#define ALBEDO_MATCAP_ENABLE 2"); // 开启 Spa 贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MATCAP_MAP_FILE\s+""([^""]*)""", $"#define ALBEDO_MATCAP_MAP_FILE \"{spaFileName}\""); // 替换 Spa 贴图文件路径
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为相对路径或绝对路径
                        else
                        {
                            if (albedoMatched)  // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FROM\s+\d+", "#define ALBEDO_SUB_MAP_FROM 1"); // 开启高光贴图
                                string path = GetRelativeOrAbsolutePath(albedoFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_SUB_MAP_FILE\s+""([^""]*)""", $"#define ALBEDO_SUB_MAP_FILE \"{path}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FROM\s+\d+", "#define NORMAL_MAP_FROM 1"); // 开启法线贴图
                                string path = GetRelativeOrAbsolutePath(normalFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_MAP_FILE\s+""([^""]*)""", $"#define NORMAL_MAP_FILE \"{path}\""); // 替换法线贴图文件路径
                            }
                            if (smoothnessMatched || roughnessMatched)  // 替换光滑度或粗糙度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FROM\s+\d+", "#define SMOOTHNESS_MAP_FROM 1"); // 开启光滑度贴图
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;  // 优先选择光滑度贴图，如果smoothnessFileName为空，就用roughnessFileName
                                string path = GetRelativeOrAbsolutePath(selectedFile);
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+""([^""]*)""", $"#define SMOOTHNESS_MAP_FILE \"{path}\""); // 替换光滑度贴图文件路径
                            }
                            if (metalnessMatched)  // 替换金属度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FROM\s+\d+", "#define METALNESS_MAP_FROM 1"); // 开启金属度贴图
                                string path = GetRelativeOrAbsolutePath(metalnessFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FILE\s+""([^""]*)""", $"#define METALNESS_MAP_FILE \"{path}\""); // 替换金属度贴图文件路径
                            }
                            if (OcclusionMatched)  // 替换环境光遮蔽贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FROM\s+\d+", "#define OCCLUSION_MAP_FROM 1"); // 开启环境光遮蔽贴图
                                string path = GetRelativeOrAbsolutePath(OcclusionFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+OCCLUSION_MAP_FILE\s+""([^""]*)""", $"#define OCCLUSION_MAP_FILE \"{path}\""); // 替换环境光遮蔽贴图文件路径
                            }
                            if (spaMatched)  // 替换 Spa 贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MATCAP_ENABLE\s+\d+", "#define ALBEDO_MATCAP_ENABLE 2"); // 开启 Spa 贴图
                                string path = GetRelativeOrAbsolutePath(spaFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MATCAP_MAP_FILE\s+""([^""]*)""", $"#define ALBEDO_MATCAP_MAP_FILE \"{path}\""); // 替换 Spa 贴图文件路径
                            }
                        }

                        // 如果没有匹配到任何贴图则跳过写入 .fx 文件
                        if (!albedoMatched && !normalMatched && !smoothnessMatched && !roughnessMatched && !metalnessMatched && !spaMatched)
                        {
                            continue; // 跳过当前材质
                        }

                        // 如果匹配到了贴图，才写入 fx 文件
                        File.WriteAllText(destFile1, fxContent, new UTF8Encoding(false)); // 使用不带 BOM 的 UTF-8 编码
                    }

                    /*************************
                    ikPolishShader  作者：ikeno

                    关联文件：
                    Renderer\ikPolishShader\Material.fx
                    Renderer\ikPolishShader\Sources
                    ***********************/
                    else if (renderer == rm.GetString("Renderer_ikPolishShader"))
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
                        destFile2 = Path.Combine(fxFolderPath, "Sources");
                        // 复制文件和文件夹到目标文件夹
                        File.Copy(sourceFile1, destFile1, true);
                        CopyDirectory(sourceFolder, destFile2);

                        // 读取 .fx 文件内容
                        string fxContent = File.ReadAllText(destFile1);

                        // 获取材质对应的贴图文件
                        string textureFileName = Path.GetFileName(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 获取 CustomRules.ini 路径
                        string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomRules.ini");
                        var suffixes = LoadAllSuffixes(iniPath);

                        // 提取前缀（不含扩展名）
                        string fileNameNoExt = Path.GetFileNameWithoutExtension(textureFileName);
                        string texturePrefix = ExtractPrefixFromFilename(fileNameNoExt, suffixes);

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        string smoothnessFileName = "";
                        string roughnessFileName = "";
                        string metalnessFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;
                        bool smoothnessMatched = false;
                        bool roughnessMatched = false;
                        bool metalnessMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 高光 Specular 规则的文件
                            if (!albedoMatched && !string.IsNullOrEmpty(rule.Specular))
                            {
                                var specularFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Specular}.*").ToList();
                                if (specularFiles.Any())
                                {
                                    albedoFileName = Path.GetFileName(specularFiles.First());
                                    albedoMatched = true;
                                }
                            }

                            // 查找符合 法线 Normal 规则的文件
                            if (!normalMatched && !string.IsNullOrEmpty(rule.Normal))
                            {
                                var normalFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Normal}.*").ToList();
                                if (normalFiles.Any())
                                {
                                    normalFileName = Path.GetFileName(normalFiles.First());
                                    normalMatched = true;
                                }
                            }

                            // 查找符合 光滑度 Smoothness 规则的文件
                            if (!smoothnessMatched && !string.IsNullOrEmpty(rule.Smoothness))
                            {
                                var smoothnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Smoothness}.*").ToList();
                                if (smoothnessFiles.Any())
                                {
                                    smoothnessFileName = Path.GetFileName(smoothnessFiles.First());
                                    smoothnessMatched = true;
                                }
                            }

                            // 查找符合 粗糙度 Roughness 规则的文件
                            if (!roughnessMatched && !string.IsNullOrEmpty(rule.Roughness))
                            {
                                var roughnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Roughness}.*").ToList();
                                if (roughnessFiles.Any())
                                {
                                    roughnessFileName = Path.GetFileName(roughnessFiles.First());
                                    roughnessMatched = true;
                                }
                            }

                            // 查找符合 金属度 Metalness 规则的文件
                            if (!metalnessMatched && !string.IsNullOrEmpty(rule.Metalness))
                            {
                                var metalnessFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Metalness}.*").ToList();
                                if (metalnessFiles.Any())
                                {
                                    metalnessFileName = Path.GetFileName(metalnessFiles.First());
                                    metalnessMatched = true;
                                }
                            }

                            // 如果所有贴图都找到了，则可以提前退出循环
                            if (albedoMatched && normalMatched && smoothnessMatched && roughnessMatched && metalnessMatched)
                            {
                                break;
                            }
                        }

                        // 辅助函数：获取相对或绝对路径（始终使用 / 斜杠）
                        string GetRelativeOrAbsolutePath(string fileName)
                        {
                            string combinedPath = Path.Combine(textureDirectory, fileName).Replace('\\', '/');
                            if (fxFolderPath.StartsWith(Path.GetDirectoryName(inputFilePath)))
                            {
                                Uri baseUri = new Uri(fxFolderPath + "/");
                                Uri fileUri = new Uri(combinedPath);
                                return baseUri.MakeRelativeUri(fileUri).ToString().Replace('\\', '/');
                            }
                            return combinedPath;
                        }

                        // 只有匹配时才进行替换，如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            if (albedoMatched) // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MAP_ENABLE\s+\d+", "#define ALBEDO_MAP_ENABLE  1"); // 开启高光贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_1\s+""([^""]*)""", $"#define TEXTURE_FILENAME_1   \"{albedoFileName}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMALMAP_ENABLE\s+\d+", "#define NORMALMAP_ENABLE 1"); // 开启法线贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMALMAP_MAIN_FILENAME\s+""([^""]*)""", $"#define NORMALMAP_MAIN_FILENAME  \"{normalFileName}\""); // 替换法线贴图文件路径
                            }
                            if (metalnessMatched)  // 替换金属度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_ENABLE\s+\d+", "#define METALNESS_MAP_FROM 1"); // 开启金属度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FILE\s+\d+", "#define METALNESS_MAP_FILE 2"); // 金属度贴图文件类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_2\s+""([^""]*)""", $"#define TEXTURE_FILENAME_2\"{metalnessFileName}\""); // 替换金属度贴图文件路径，纹理文件名排序
                            }
                            // 替换光滑度贴图或粗糙度贴图
                            if (smoothnessMatched)
                            {
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_TYPE\s+\d+", "#define SMOOTHNESS_TYPE 1"); // 光滑度类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_ENABLE\s+\d+", "#define SMOOTHNESS_MAP_ENABLE 1"); // 开启光滑度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+\d+", "#define SMOOTHNESS_MAP_FILE 3"); // 光滑度贴图文件类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_3\s+""([^""]*)""", $"#define TEXTURE_FILENAME_3 \"{selectedFile}\""); // 替换光滑度贴图文件路径，纹理文件名排序
                            }
                            else if (roughnessMatched) //替换粗糙度贴图，只有在没有光滑度 smoothness 时才用粗糙度 roughness
                            {
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_TYPE\s+\d+", "#define SMOOTHNESS_TYPE 2"); // 粗糙度类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_ENABLE\s+\d+", "#define SMOOTHNESS_MAP_ENABLE 1"); // 开启粗糙度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+\d+", "#define SMOOTHNESS_MAP_FILE 3"); // 粗糙度贴图文件类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_3\s+""([^""]*)""", $"#define TEXTURE_FILENAME_3 \"{selectedFile}\""); // 替换粗糙度贴图文件路径，纹理文件名排序
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为相对路径或绝对路径
                        else
                        {
                            if (albedoMatched)  // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+ALBEDO_MAP_ENABLE\s+\d+", "#define ALBEDO_MAP_ENABLE  1"); // 开启高光贴图
                                string path = GetRelativeOrAbsolutePath(albedoFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_1\s+""([^""]*)""", $"#define TEXTURE_FILENAME_1   \"{path}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMALMAP_ENABLE\s+\d+", "#define NORMALMAP_ENABLE  1"); // 开启法线贴图
                                string path = GetRelativeOrAbsolutePath(normalFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMALMAP_MAIN_FILENAME\s+""([^""]*)""", $"#define NORMALMAP_MAIN_FILENAME  \"{path}\""); // 替换法线贴图文件路径
                            }
                            if (metalnessMatched)  // 替换金属度贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_ENABLE\s+\d+", "#define METALNESS_MAP_ENABLE  1"); // 开启金属度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+METALNESS_MAP_FILE\s+\d+", "#define METALNESS_MAP_FILE  2"); // 金属度贴图纹理文件名排序
                                string path = GetRelativeOrAbsolutePath(metalnessFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_2\s+""([^""]*)""", $"#define TEXTURE_FILENAME_2  \"{path}\""); // 替换金属度贴图文件路径
                            }
                            // 替换光滑度或粗糙度贴图
                            if (smoothnessMatched)
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_TYPE\s+\d+", "#define SMOOTHNESS_TYPE  1"); // 光滑度类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_ENABLE\s+\d+", "#define SMOOTHNESS_MAP_ENABLE  1"); // 开启光滑度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+\d+", "#define SMOOTHNESS_MAP_FILE  3"); // 光滑度贴图纹理文件名排序
                                string path = GetRelativeOrAbsolutePath(smoothnessFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_3\s+""([^""]*)""", $"#define TEXTURE_FILENAME_3  \"{path}\"");  // 替换光滑度贴图文件路径
                            }
                            else if (roughnessMatched) //替换粗糙度贴图，只有在没有光滑度 smoothness 时才用粗糙度 roughness
                            {
                                string selectedFile = !string.IsNullOrEmpty(smoothnessFileName) ? smoothnessFileName : roughnessFileName;
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_TYPE\s+\d+", "#define SMOOTHNESS_TYPE  2"); // 粗糙度类型
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_ENABLE\s+\d+", "#define SMOOTHNESS_MAP_ENABLE  1"); // 开启粗糙度贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+SMOOTHNESS_MAP_FILE\s+\d+", "#define SMOOTHNESS_MAP_FILE  3"); // 粗糙度贴图纹理文件名排序
                                string path = GetRelativeOrAbsolutePath(roughnessFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+TEXTURE_FILENAME_3\s+""([^""]*)""", $"#define TEXTURE_FILENAME_3  \"{path}\""); // 替换粗糙度贴图文件路径
                            }
                        }

                        // 如果没有匹配到任何贴图则跳过写入 .fx 文件
                        if (!albedoMatched && !normalMatched && !smoothnessMatched && !roughnessMatched && !metalnessMatched)
                        {
                            continue; // 跳过当前材质
                        }

                        // 如果匹配到了贴图，才写入 fx 文件
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
                    else if (renderer == rm.GetString("Renderer_PowerShader"))
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
                        string textureFileName = Path.GetFileName(textureFile);
                        string textureDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(inputFilePath), textureFile));

                        // 获取 CustomRules.ini 路径
                        string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomRules.ini");
                        var suffixes = LoadAllSuffixes(iniPath);

                        // 提取前缀（不含扩展名）
                        string fileNameNoExt = Path.GetFileNameWithoutExtension(textureFileName);
                        string texturePrefix = ExtractPrefixFromFilename(fileNameNoExt, suffixes);

                        // 查找符合 CustomRules.ini 规则的文件名
                        string albedoFileName = "";
                        string normalFileName = "";
                        bool albedoMatched = false;
                        bool normalMatched = false;

                        foreach (var rule in customRules)
                        {
                            // 查找符合 高光 Specular 规则的文件
                            if (!albedoMatched && !string.IsNullOrEmpty(rule.Specular))
                            {
                                var specularFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Specular}.*").ToList();
                                if (specularFiles.Any())
                                {
                                    albedoFileName = Path.GetFileName(specularFiles.First());
                                    albedoMatched = true;
                                }
                            }

                            // 查找符合 法线 Normal 规则的文件
                            if (!normalMatched && !string.IsNullOrEmpty(rule.Normal))
                            {
                                var normalFiles = Directory.GetFiles(textureDirectory, $"{texturePrefix}{rule.Normal}.*").ToList();
                                if (normalFiles.Any())
                                {
                                    normalFileName = Path.GetFileName(normalFiles.First());
                                    normalMatched = true;
                                }
                            }

                            // 如果找到了匹配的高光 Specular 和法线 Normal 贴图，则退出循环
                            if (albedoMatched && normalMatched)
                            {
                                break;
                            }
                        }

                        // 辅助函数：获取相对或绝对路径（始终使用 / 斜杠）
                        string GetRelativeOrAbsolutePath(string fileName)
                        {
                            string combinedPath = Path.Combine(textureDirectory, fileName).Replace('\\', '/');
                            if (fxFolderPath.StartsWith(Path.GetDirectoryName(inputFilePath)))
                            {
                                Uri baseUri = new Uri(fxFolderPath + "/");
                                Uri fileUri = new Uri(combinedPath);
                                return baseUri.MakeRelativeUri(fileUri).ToString().Replace('\\', '/');
                            }
                            return combinedPath;
                        }

                        // 只有匹配时才进行替换，如果勾选了“源文件夹”选项，贴图文件路径为相对路径
                        if (directoverwrite_text.Checked)
                        {
                            if (albedoMatched) // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+EMISSIVE_FROM\s+\d+", "#define EMISSIVE_FROM 2"); // 开启高光贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+EMISSIVE_TEXTURE\s+""([^""]*)""", $"#define EMISSIVE_TEXTURE \"{albedoFileName}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_FROM\s+\d+", "#define NORMAL_FROM 2"); // 开启法线贴图
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_TEXTURE\s+""([^""]*)""", $"#define NORMAL_TEXTURE \"{normalFileName}\""); // 替换法线贴图文件路径
                            }
                        }
                        // 如果选择自定义路径，那么贴图文件路径为相对路径或绝对路径
                        else
                        {
                            if (albedoMatched)  // 替换高光贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+EMISSIVE_FROM\s+\d+", "#define EMISSIVE_FROM 2"); // 开启高光贴图
                                string path = GetRelativeOrAbsolutePath(albedoFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+EMISSIVE_TEXTURE\s+""([^""]*)""", $"#define EMISSIVE_TEXTURE \"{path}\""); // 替换高光贴图文件路径
                            }
                            if (normalMatched)  // 替换法线贴图
                            {
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_FROM\s+\d+", "#define NORMAL_FROM 2"); // 开启法线贴图
                                string path = GetRelativeOrAbsolutePath(normalFileName);
                                fxContent = Regex.Replace(fxContent, @"#define\s+NORMAL_TEXTURE\s+""([^""]*)""", $"#define NORMAL_TEXTURE \"{path}\""); // 替换法线贴图文件路径
                            }
                        }

                        // 如果没有匹配到任何贴图则跳过写入 .fx 文件
                        if (!albedoMatched && !normalMatched)
                        {
                            continue; // 跳过当前材质
                        }

                        // 如果匹配到了贴图，才写入 fx 文件
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

        // ====================================
    }

    // ================ END ==================
}