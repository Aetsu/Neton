using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Neton
{
    class OsFeatures
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            QueryLimitedInformation = 0x1000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
                                         bool bInheritHandle, int dwProcessId);


        //Checking debug privileges
        public string checkDebugPrivs()
        {
            string info = string.Format("{0} | {1}", "Debug privileges", "Disabled");
            try
            {
                Process[] target = Process.GetProcessesByName("csrss");
                if (target.Length > 0)
                {
                    IntPtr hprocess = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, target[0].Id);
                    if (hprocess != IntPtr.Zero)
                    {
                        info = string.Format("{0} | {1}", "Debug privileges", "Enabled");
                    }
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            return info;
        }
    }
}
