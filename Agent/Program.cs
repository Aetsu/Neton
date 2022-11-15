using System;
using System.Threading;

namespace Neton
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Helpers.CheckInternetConnection())
            {
                Console.WriteLine("[-] Error: No Internet connection");
                return;
            }
            Settings settings = new Settings();
            settings.url = "https://xxxxxxxxxxxx:8443/collect_data";
            settings.executionId = Helpers.GetRandomString();
            settings.sandboxId = "sandboxName";
            settings.wave = "1";
            settings.version = "1.3";
            settings.md5Hash = Helpers.getMd5(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            settings.sha256Hash = Helpers.getSha256(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);



            Console.WriteLine("  ================= Start execution =================\n\n\n");
            Thread thr = new Thread(() => CheckSandbox.Launch(settings));
            Thread thr2 = new Thread(() => SystemInfo.Launch(settings));
            Thread thr3 = new Thread(() => HookDetector.Launch(settings));
            Thread thr4 = new Thread(() => FileCrawler.Launch(settings));
            Thread thr5 = new Thread(() => SharpEDRCheckerHelper.Launch(settings));
            Thread thr6 = new Thread(() => ExeLoader.LaunchPafish(settings));
            Thread thr7 = new Thread(() => ExeLoader.LaunchAlkhaser(settings));
            thr.Start();
            thr3.Start();
            thr2.Start();
            thr4.Start();
            thr5.Start();
            thr6.Start();
            thr7.Start();
            thr.Join();
            thr2.Join();
            thr3.Join();
            thr4.Join();
            thr5.Join();
            thr6.Join();
            thr7.Join();
            Console.WriteLine("\n\n\n  ================= End execution  ================= ");
            //Console.ReadLine();
        }
    }
}
