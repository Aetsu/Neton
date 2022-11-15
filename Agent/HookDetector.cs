/* original code: https://github.com/matterpreter/OffensiveCSharp/tree/master/HookDetector
 * author: Matt Hand
   github: https://github.com/matterpreter
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Neton
{
    class HookDetector
    {
        static string[] functions =
        {
            "NtClose",
            "NtAllocateVirtualMemory",
            "NtAllocateVirtualMemoryEx",
            "NtCreateThread",
            "NtCreateThreadEx",
            "NtCreateUserProcess",
            "NtFreeVirtualMemory",
            "NtLoadDriver",
            "NtMapViewOfSection",
            "NtOpenProcess",
            "NtProtectVirtualMemory",
            "NtQueueApcThread",
            "NtQueueApcThreadEx",
            "NtResumeThread",
            "NtSetContextThread",
            "NtSetInformationProcess",
            "NtSuspendThread",
            "NtUnloadDriver",
            "NtWriteVirtualMemory",
        };
        static byte[] safeBytes = {
            0x4c, 0x8b, 0xd1, // mov r10, rcx
            0xb8              // mov eax, ??
        };

        static string CheckHook()
        {
            string res = "";
            if (!GetProcessArch())
            {
                Console.WriteLine("[-] It looks like you're not running x64.");
                return res;
            }
            // Get the base address of ntdll.dll in our own process
            IntPtr ntdllBase = GetNTDLLBase();
            if (ntdllBase == IntPtr.Zero)
            {
                Console.WriteLine("[-] Couldn't get find ntdll.dll");
                return res;

            }
            else
            {
                //Console.WriteLine("NTDLL Base Address: 0x{0:X}", ntdllBase.ToInt64());
                res += String.Format("NTDLL Base Address: 0x{0:X}\n", ntdllBase.ToInt64());
            }

            // Get the address of each of the target functions in ntdll.dll
            IDictionary<string, IntPtr> funcAddresses = GetFuncAddress(ntdllBase, functions);

            // Check the first DWORD at each function's address for proper SYSCALL setup
            int i = 0; // Used for populating the results array
            bool safe;
            foreach (KeyValuePair<string, IntPtr> func in funcAddresses)
            {
                byte[] instructions = new byte[4];
                Marshal.Copy(func.Value, instructions, 0, 4);

                string fmtFunc = string.Format("    {0,-25} 0x{1:X} ", func.Key, func.Value.ToInt64());
                safe = instructions.SequenceEqual(safeBytes);

                if (safe)
                {
                    //Console.WriteLine(fmtFunc + "- SAFE");
                    res += fmtFunc + "- SAFE\n";
                }
                else
                {
                    byte[] hookInstructions = new byte[32];
                    Marshal.Copy(func.Value, hookInstructions, 0, 32);
                    //Console.WriteLine(fmtFunc + " - HOOK DETECTED");
                    //Console.WriteLine("    {0,-25} {1}", "Instructions: ", BitConverter.ToString(hookInstructions).Replace("-", " "));
                    res += fmtFunc + " - HOOK DETECTED\n";
                    res += String.Format("    {0,-25} {1}\n", "Instructions: ", BitConverter.ToString(hookInstructions).Replace("-", " "));
                }

                i++;
            }
            return res;
        }

        static IntPtr GetNTDLLBase()
        {
            Process hProc = Process.GetCurrentProcess();
            ProcessModule module = hProc.Modules.Cast<ProcessModule>().SingleOrDefault(m => string.Equals(m.ModuleName, "ntdll.dll", StringComparison.OrdinalIgnoreCase));
            return module?.BaseAddress ?? IntPtr.Zero;
        }

        static IDictionary<string, IntPtr> GetFuncAddress(IntPtr hModule, string[] functions)
        {
            IDictionary<string, IntPtr> funcAddresses = new Dictionary<string, IntPtr>();
            foreach (string function in functions)
            {
                IntPtr funcPtr = Win32.GetProcAddress(hModule, function);
                if (funcPtr != IntPtr.Zero)
                {
                    funcAddresses.Add(function, funcPtr);
                }
                else
                {
                    Console.WriteLine("[-] Couldn't locate the address for {0}! (Error: {1})", function, Marshal.GetLastWin32Error());
                }
            }

            return funcAddresses;
        }

        static bool GetProcessArch()
        {
            // Make sure that we're running x64 on x64
            bool wow64;
            Win32.IsWow64Process(Process.GetCurrentProcess().Handle, out wow64);

            if (Environment.Is64BitProcess && !wow64)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Launch(Settings settings)
        {
            SendInfo(settings);
        }

        static void SendInfo(Settings settings)
        {
            string hooks = CheckHook();
            Console.WriteLine(hooks);
            object data = new
            {
                hooklist = Helpers.B64e(hooks),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e("checkhooks"),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };
            Helpers.SendData(settings.url, data);
        }

    }

    class Win32
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);
    }

}
