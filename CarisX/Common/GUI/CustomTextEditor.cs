using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Infragistics.Win.UltraWinEditors;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Oelco.Common.GUI
{
    public partial class CustomTextEditor : UltraTextEditor
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        public CustomTextEditor()
        {
            InitializeComponent();

            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                this.Leave += new EventHandler(this.CustomTextEditor_OnLeave);
            }
        }
        protected override void WndProc(ref Message m)
        {
            // 右クリック
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_NCLBUTTONDOWN = 0xA1;

            // クリックイベントを取得
            if (((Int32)m.WParam == WM_LBUTTONDOWN) ||
                ((Int32)m.Msg == WM_LBUTTONDOWN) ||
                ((Int32)m.WParam == WM_NCLBUTTONDOWN) ||
                ((Int32)m.Msg == WM_NCLBUTTONDOWN))
            {
                // Windows10の場合のみ処理を行う
                System.OperatingSystem os = System.Environment.OSVersion;
                if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
                {
                    // ソフトウェアキーボードを開く
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(startInfo);
                }
            }

            base.WndProc(ref m);
        }
        private void CustomTextEditor_OnLeave(object sender, EventArgs e)
        {
            // Windows10の場合のみ処理を行う
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                // ソフトウェアキーボードを閉じる
                uint WM_SYSCOMMAND = 274;
                uint SC_CLOSE = 61536;
                IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
                PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
            }
        }
    }
}
