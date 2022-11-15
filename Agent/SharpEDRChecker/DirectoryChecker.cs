/* original code: https://github.com/PwnDexter/SharpEDRChecker
 * author: PwnDexter
   github: https://github.com/PwnDexter
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEDRChecker
{
    internal class DirectoryChecker
    {
        internal static string CheckDirectories()
        {
            try
            {
                Console.WriteLine("########################################");
                Console.WriteLine("[!][!][!] Checking Directories [!][!][!]");
                Console.WriteLine("########################################\n");
                string summary = "";
                string[] progdirs = {
                    @"C:\Program Files",
                    @"C:\Program Files (x86)",
                    @"C:\ProgramData"};

                foreach (string dir in progdirs)
                {
                    string[] subdirectories = Directory.GetDirectories(dir);
                    summary += CheckDirectory(subdirectories);
                }
                if (string.IsNullOrEmpty(summary))
                {
                    Console.WriteLine("[+] No suspicious directories found\n");
                    return "No suspicious directories found";
                }
                return $"{summary}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking directories: {e.Message}\n{e.StackTrace}");
                return "Errored on checking directories";
            }
        }

        private static string CheckDirectory(string[] subdirectories)
        {
            var summary = "";
            foreach (var subdirectory in subdirectories)
            {
                summary += CheckSubDirectory(subdirectory);  
            }
            return summary;
        }

        private static string CheckSubDirectory(string subdirectory)
        {
            try
            {
                var matches = new List<string>();
                foreach (var edrstring in EDRData.edrlist)
                {
                    if (subdirectory.ToString().ToLower().Contains(edrstring.ToLower()))
                    {
                        matches.Add(edrstring);
                    }
                }
                if (matches.Count > 0)
                {
                    Console.WriteLine($"[-] Suspicious directory found: {subdirectory}");
                    Console.WriteLine($"[!] Matched on: {string.Join(", ", matches.ToArray())}\n");
                    return $"{subdirectory} : {string.Join(", ", matches.ToArray())}\n";
                }
                return "";
            } 
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking sub directory: {subdirectory}\n{e.Message}\n{e.StackTrace}");
                return $"{subdirectory} : Failed to perform checks";
            }
        }
    }
}