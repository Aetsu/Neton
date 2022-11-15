using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Neton
{
    static class ExeLoader
    {
        static void SendInfo(Settings settings, string module, string moduleOutput)
        {
            Console.WriteLine(moduleOutput);
            object data = new
            {
                moduleOutput = Helpers.B64e(moduleOutput),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e(module),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };
            Helpers.SendData(settings.url, data);
        }
        static string runCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {command}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
            return output;
        }
        public static string LaunchFile(string binFile)
        {
            string output = "";
            string outputFile = Helpers.GetRandomString() + ".exe";
            Helpers.B64strToFile(binFile, outputFile);
            if (File.Exists(outputFile))
            {
                output = runCommand(outputFile);
            }
            else
            {
                Console.WriteLine("[-] File not found");
            }
            return output;
        }

        public static void LaunchAlkhaser(Settings settings)
        {
            string output = LaunchFile(Resources.GetAlkhaser());
            SendInfo(settings, "alkhaser", output);
        }
        public static void LaunchPafish(Settings settings)
        {
            string output = LaunchFile(Resources.GetPafish());
            SendInfo(settings, "pafish", output); ;
        }
    }
}
