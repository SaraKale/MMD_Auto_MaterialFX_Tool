using PMX_Material_Tools;
using System;
using System.Windows.Forms;

namespace PMXMaterialExporter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // MainForm 是你的主窗体类
        }
    }
}