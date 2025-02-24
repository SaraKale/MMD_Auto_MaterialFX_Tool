using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using static PMX_Material_Tools.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace PMX_Material_Tools
{
    // 材质预览窗口
    public partial class MaterialPreview : Form
    {
        // 构造函数，初始化窗口并加载材质信息
        public MaterialPreview(List<MaterialInfo> materialInfoList)
        {
            InitializeComponent();
            InitializeListView();
            LoadMaterialInfo(materialInfoList); // 加载材质信息到 ListView
            InitializeContextMenu(); // 初始化上下文菜单

            // -------------------------
            //  自适应布局缩放设置
            // --------------------------
            // TableLayoutPanel 填充整个窗体
            tableLayoutPanel1.Dock = DockStyle.Fill;

            // Panel 的列宽占 30% 的窗体宽度
            tableLayoutPanel1.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 30);
            tableLayoutPanel1.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 70);

            // Panel 控件填充单元格
            panel1.Dock = DockStyle.Fill;
            panel2.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.Fill;
            panel4.Dock = DockStyle.Fill;

            // ListView 控件填充表格单元格
            PreviewList.Dock = DockStyle.Fill;

            // Label 控件宽度自适应
            PreviewRM_text.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // 让按钮始终在右侧
            outCSV_button.Anchor = AnchorStyles.Top | AnchorStyles.Left ;

        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            ResizeListViewColumns(); // 监听 Panel.SizeChanged 事件
        }

        private void ResizeListViewColumns()
        {
            // 调整 PreviewList 控件列宽
            int totalWidth = PreviewList.ClientSize.Width;
            int columnCount = PreviewList.Columns.Count;

            if (columnCount == 0) return;

            for (int i = 0; i < columnCount; i++)
            {
                PreviewList.Columns[i].Width = totalWidth / columnCount; // 每列等分
            }
        }

        // 窗口加载
        private void MaterialPreview_Load(object sender, EventArgs e)
        {
            // 控件自适应缩放
            panel1.SizeChanged += panel1_SizeChanged;
        }

        // 更新语言
        public void UpdateLanguage(ResourceManager rm)
        {
            this.Text = rm.GetString("MaterialPreviewTitle"); // 标题
            PreviewRM_text.Text = rm.GetString("RreviewRMTips"); // 材质预览窗口说明
            OutCSVlist.Text = rm.GetString("OutCSVText"); // 导出CSV文件
            outCSV_button.Text = rm.GetString("CSVtoButton"); // 导出CSV文件 保存按钮
        }

        // 初始化 ListView 控件
        private void InitializeListView()
        {
            PreviewList.OwnerDraw = true; // 启用自定义绘制
            PreviewList.FullRowSelect = true; // 启用行选择
            PreviewList.View = View.Details; // 详细视图模式
            PreviewList.GridLines = true; // 禁用默认网格线（我们手动绘制）

            PreviewList.DrawColumnHeader += PreviewList_DrawColumnHeader;
            PreviewList.DrawItem += PreviewList_DrawItem;
            PreviewList.DrawSubItem += PreviewList_DrawSubItem;
        }

        // 自定义绘制列标题
        private void PreviewList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (Font headerFont = new Font("宋体", 10.125F, FontStyle.Bold))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds); // 标题背景
                e.Graphics.DrawRectangle(new Pen(Color.Black, 2), e.Bounds); // 加粗边框
                e.Graphics.DrawString(e.Header.Text, headerFont, Brushes.Black, e.Bounds, sf);
            }
        }

        // 自定义绘制行
        private void PreviewList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        // 自定义绘制单元格
        // 没有效果，ListView不支持自定义绘制单元格
        private void PreviewList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Rectangle cellBounds = e.Bounds;

            // **检查内容是否发生变化**
            bool isChanged = (e.SubItem.Text.Contains("⚠️") || e.SubItem.Text.Contains("Error"));

            // 设置不同背景色
            Brush backgroundBrush = isChanged ? Brushes.LightPink : Brushes.White;
            e.Graphics.FillRectangle(backgroundBrush, cellBounds);

            // 设置不同字体大小
            Font itemFont = isChanged ? new Font(e.Item.Font.FontFamily, 12, FontStyle.Bold) : new Font(e.Item.Font.FontFamily, 12, FontStyle.Regular);

            // 绘制文本
            using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(e.SubItem.Text, itemFont, Brushes.Black, cellBounds, sf);
            }

            // **手动绘制网格**
            using (Pen gridPen = new Pen(Color.DarkGray, 1)) // 设置网格线颜色和宽度
            {
                e.Graphics.DrawLine(gridPen, cellBounds.Left, cellBounds.Bottom, cellBounds.Right, cellBounds.Bottom);
                e.Graphics.DrawLine(gridPen, cellBounds.Right, cellBounds.Top, cellBounds.Right, cellBounds.Bottom);
            }
        }

        // 加载材质信息到 ListView
        public void LoadMaterialInfo(List<MaterialInfo> materialInfoList)
        {
            PreviewList.Items.Clear(); // 清空现有项

            foreach (var materialInfo in materialInfoList)
            {
                ListViewItem item = new ListViewItem(new[]
                {
                    materialInfo.ID,
                    materialInfo.Name,
                    materialInfo.EnglishName,
                    materialInfo.Texture,
                    materialInfo.Toon,
                    materialInfo.Spa
                });
                PreviewList.Items.Add(item);
            }

            // 设置列宽度自适应内容
            foreach (ColumnHeader column in PreviewList.Columns)
            {
                column.Width = -2;
            }
        }



        // 表格列表
        private void PreviewList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // 导出 CSV 按钮点击事件处理程序
        private void outCSV_button_Click(object sender, EventArgs e)
        {
            ExportToCSV();
        }

        // 导出 CSV 文件
        private void ExportToCSV()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // 链接 ResourceManager 语言文件
                ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = rm.GetString("OutCSVText"); // 设置对话框标题，导出 CSV 文件
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder csvContent = new StringBuilder();

                    // 添加列标题
                    foreach (ColumnHeader column in PreviewList.Columns)
                    {
                        csvContent.Append(column.Text + ",");
                    }
                    csvContent.AppendLine();

                    // 添加行数据
                    foreach (ListViewItem item in PreviewList.Items)
                    {
                        foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                        {
                            csvContent.Append(subItem.Text + ",");
                        }
                        csvContent.AppendLine();
                    }

                    // 写入文件
                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);
                    // 显示成功消息,CSV 文件导出成功！", "成功"
                    MessageBox.Show(rm.GetString("CSVExportSuccessMessage"), rm.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // 初始化上下文菜单
        private void InitializeContextMenu()
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

            ContextMenu contextMenu = new ContextMenu();
            MenuItem copyMenuItem = new MenuItem(rm.GetString("CopyMenuItemText"), CopyMenuItem_Click); // 创建复制菜单项
            contextMenu.MenuItems.Add(copyMenuItem);
            PreviewList.ContextMenu = contextMenu;
        }

        // 复制菜单项点击事件处理程序
        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            // 链接 ResourceManager 语言文件
            ResourceManager rm = new ResourceManager("PMX_Material_Tools.languagelist.Resources", Assembly.GetExecutingAssembly());

            if (PreviewList.SelectedItems.Count > 0)
            {
                StringBuilder selectedText = new StringBuilder();
                foreach (ListViewItem item in PreviewList.SelectedItems)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    {
                        selectedText.Append(subItem.Text + "\t");
                    }
                    selectedText.AppendLine();
                }
                Clipboard.SetText(selectedText.ToString());
                // 显示成功消息  "内容已复制到剪贴板！", "复制成功"
                MessageBox.Show(rm.GetString("CopySuccessMessage"), rm.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 材质预览窗口说明
        private void PreviewRM_text_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // =========== END ============
    }
}
