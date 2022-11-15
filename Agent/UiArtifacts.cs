using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Neton
{
    class UiArtifacts
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Check if windows with certain class names are present in the OS
        public string checkWindowTitle()
        {
            List<string> lRes = new List<string>();

            IntPtr hWnd = FindWindow("VBoxTrayToolWndClass", null);
            if (hWnd.ToInt32() != 0)
            {
                string info = string.Format("{0} | {1}", "VirtualBox", "VBoxTrayToolWndClass");
                lRes.Add(info);
            }
            IntPtr hWnd2 = FindWindow(null, "VBoxTrayToolWnd");
            if (hWnd2.ToInt32() != 0)
            {
                string info = string.Format("{0} | {1}", "VirtualBox", "VBoxTrayToolWnd");
                lRes.Add(info);
            }
            string response = string.Join("\n", lRes.ToArray());
            return response;
        }

        //Check if top level windows' number is too small
        public string checkNWindows()
        {
            Process[] processlist = Process.GetProcesses();
            int count = 0;
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    count++;
                }
            }
            string info = string.Format("{0} | {1}", "Number of top level windows", count.ToString());          
            return info;
        }
    }
}
