/* original code: https://github.com/PwnDexter/SharpEDRChecker
 * author: PwnDexter
   github: https://github.com/PwnDexter
 */
using System;

namespace SharpEDRChecker
{
    public class Program
    {
       public static string LaunchProcesses()
        {
            return ProcessChecker.CheckProcesses();
        }
        
        public static string LaunchCurrentProcessModules()
        {
            return ProcessChecker.CheckCurrentProcessModules();
        }
        
        public static string LaunchCheckDirectories()
        {
            return DirectoryChecker.CheckDirectories();
        }
        
        public static string LaunchServiceChecker()
        {
            return ServiceChecker.CheckServices();
        }
        
        public static string LaunchCheckDrivers()
        {
            return DriverChecker.CheckDrivers();
        }
        private static void PrintIntro(bool isAdm)
        {
            if (isAdm)
            {
                Console.WriteLine($"\n##################################################################");
                Console.WriteLine("   [!][!][!] Welcome to SharpEDRChecker by @PwnDexter [!][!][!]");
                Console.WriteLine("[+][+][+] Running as admin, all checks will be performed [+][+][+]");
                Console.WriteLine($"##################################################################\n");
            }
            else
            {
                Console.WriteLine($"\n###################################################################################################");
                Console.WriteLine("                    [!][!][!] Welcome to SharpEDRChecker by @PwnDexter [!][!][!]");
                Console.WriteLine("[-][-][-] Not running as admin, some privileged metadata and processes may not be checked [-][-][-]");
                Console.WriteLine($"###################################################################################################\n");
            }
        }

        private static void PrintOutro(string summary)
        {
            Console.WriteLine($"################################");
            Console.WriteLine($"[!][!][!] TLDR Summary [!][!][!]");
            Console.WriteLine($"################################");
            Console.WriteLine($"{summary}");
            Console.WriteLine($"#######################################");
            Console.WriteLine("[!][!][!] EDR Checks Complete [!][!][!]");
            Console.WriteLine($"#######################################\n");
        }
    }
}
