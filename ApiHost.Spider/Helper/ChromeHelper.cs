using ApiHost.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UIAutomationClient;

namespace ApiHost.Spider.Helper
{
    public sealed class ChromeHelper
    {
        #region WINAPI
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint Flags);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWnd);

        private const short SWP_NOMOVE = 0X2;
        private const short SWP_NOSIZE = 1;
        private const short SWP_NOZORDER = 0X4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;
        private const int WM_GETTEXT = 0x000D;
        private const int WM_GETTEXTLENGTH = 0x000E;
        #endregion
        CUIAutomation cUIAutomation = new CUIAutomation();
        private static readonly Lazy<ChromeHelper> lazy = new Lazy<ChromeHelper>(() => new ChromeHelper());
        public static ChromeHelper Instance { get { return lazy.Value; } }
        public ChromeHelper()
        {

        }

        public void SetProcessForeground(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }
            UnMinimizeWindow(handle);
            SetForegroundWindow(handle);
        }

        public void UnMinimizeWindow(IntPtr handle)
        {
            if (IsIconic(handle))
            {
                ShowWindowAsync(handle, SW_RESTORE);
            }
        }

        public void SetWindowRect(Process process, int x = -1, int y = -1, int width = -1, int height = -1, bool isForce = false)
        {
            GetWindowRect(process.MainWindowHandle, out Rectangle rect);
            if (isForce || (rect.Width - rect.X) < 1200)
            {
                Debug.WriteLine($"{process.MainWindowTitle} [{rect}]");

                SetWindowPos(process.MainWindowHandle, new IntPtr(-1),
                    x > 0 ? x : rect.X,
                    y > 0 ? y : rect.Y,
                    width > 0 ? width : rect.Width,
                    height > 0 ? height : rect.Height,
                    SWP_NOZORDER | SWP_SHOWWINDOW);
            }
        }

        public string GetChromeUrl(Process process)
        {
            var _automation = new CUIAutomation();
            IUIAutomationElement elm = _automation.ElementFromHandle(process.MainWindowHandle);
            IUIAutomationCondition Cond = _automation.CreatePropertyCondition(30003, 50004);
            IUIAutomationElementArray elm2 = elm.FindAll(TreeScope.TreeScope_Descendants, Cond);
            for (int i = 0; i < elm2.Length; i++)
            {
                IUIAutomationElement elm3 = elm2.GetElement(i);
                IUIAutomationValuePattern val = (IUIAutomationValuePattern)elm3.GetCurrentPattern(10002);
                if (val?.CurrentValue != "")
                {
                    return val.CurrentValue;
                }
            }
            return "";
        }

        public bool WinPostMessage(IntPtr hWnd, uint msg, int wParam, int lParam)
        {
            return PostMessage(hWnd, msg, wParam, lParam);
        }
    }
}
