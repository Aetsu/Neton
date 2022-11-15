/* original code: https://github.com/PwnDexter/SharpEDRChecker
 * author: PwnDexter
   github: https://github.com/PwnDexter
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace SharpEDRChecker
{
    internal class ProcessChecker
    {
        internal static string CheckProcesses()
        {
            try
            {
                Console.WriteLine("######################################");
                Console.WriteLine("[!][!][!] Checking processes [!][!][!]");
                Console.WriteLine("######################################\n");
                var processList = new ManagementObjectSearcher("Select * From Win32_Process").Get();
                string summary = "";
                foreach (var process in processList)
                {
                    summary += CheckProcess(process);
                }
                if (string.IsNullOrEmpty(summary))
                {
                    Console.WriteLine("[+] No suspicious processes found\n");
                    return "No suspicious processes found";
                }
                return $"{summary}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking processes: {e.Message}\n{e.StackTrace}");
                return "\n[-] Errored on checking processes\n";
            }
        }

        private static string CheckProcess(ManagementBaseObject process)
        {
            try
            {
                var processName = process["Name"];
                var processPath = process["ExecutablePath"];
                var processDescription = process["Description"];
                var processCaption = process["Caption"];
                var processCmdLine = process["CommandLine"];
                var processPID = process["ProcessId"];
                var processParent = process["ParentProcessId"];
                var metadata = "";

                var allattribs = $"{processName} - " +
                    $"{processPath} - " +
                    $"{processDescription} - " +
                    $"{processCaption} - " +
                    $"{processCmdLine}";

                if (processPath != null)
                {
                    metadata = $"{FileChecker.GetFileInfo(processPath.ToString())}";
                    allattribs = $"{allattribs} - {metadata}";
                }
                
                var matches = new List<string>();
                foreach (var edrstring in EDRData.edrlist)
                {
                    if (allattribs.ToLower().Contains(edrstring.ToLower()))
                    {
                        matches.Add(edrstring);
                    }
                }
                if (matches.Count > 0)
                {
                    Console.WriteLine($"[-] Suspicious process found:" +
                                $"\n\tName: {processName}" +
                                $"\n\tDescription: {processDescription}" +
                                $"\n\tCaption: {processCaption}" +
                                $"\n\tBinary: {processPath}" +
                                $"\n\tProcess ID: {processPID}" +
                                $"\n\tParent Process: {processParent}" +
                                $"\n\tProcess CmdLine: {processCmdLine}" +
                                $"\n\tFile Metadata: {metadata}" +
                                $"\n[!] Matched on: {string.Join(", ", matches.ToArray())}\n");
                    return $"{processName} : {string.Join(", ", matches.ToArray())}\n";
                }
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking individual process: {process["Name"]} : {process["ProcessId"]}\n{e.Message}\n{e.StackTrace}");
                return $"{process["Name"]} : Failed to perform checks\n";
            }
        }

        internal static string CheckCurrentProcessModules()
        {
            try
            {
                Console.WriteLine("###################################################################");
                Console.WriteLine("[!][!][!] Checking modules loaded in your current process [!][!][!]");
                Console.WriteLine("###################################################################\n");
                Process myproc = Process.GetCurrentProcess();
                var summary = "";
                foreach (ProcessModule module in myproc.Modules)
                {
                    summary += CheckModule(module);
                }
                if (string.IsNullOrEmpty(summary))
                {
                    Console.WriteLine("[+] No suspicious modules found in your process\n");
                    return "No suspicious modules found in your process";
                }
                return $"{summary}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking modloads: {e.Message}\n{e.StackTrace}");
                return "Errored on checking modloads";
            }
        }

        private static string CheckModule(ProcessModule module)
        {
            try
            {
                var metadata = $"{FileChecker.GetFileInfo(module.FileName)}";
                var allattribs = $"{module.FileName} - {metadata}";

                var matches = new List<string>();
                foreach (var edrstring in EDRData.edrlist)
                {
                    if (allattribs.ToString().ToLower().Contains(edrstring.ToLower()))
                    {
                        matches.Add(edrstring);
                    }
                }
                if (matches.Count > 0)
                {
                    Console.WriteLine("[-] Suspicious modload found in your process:" +
                                $"\n\tSuspicious Module: {module.FileName}" +
                                $"\n\tFile Metadata: {metadata}" +
                                $"\n[!] Matched on: {string.Join(", ", matches.ToArray())}\n");
                    return $"{module.FileName} : {string.Join(", ", matches.ToArray())}\n";
                }
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking individual module: {module.FileName}\n{e.Message}\n{e.StackTrace}");
                return $"{module.FileName} : Failed to perform checks\n";
            }
        }
    }
}