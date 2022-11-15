/* original code: https://github.com/matterpreter/OffensiveCSharp/tree/master/DriverQuery
 * author: Matt Hand
   github: https://github.com/matterpreter
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;

namespace Neton
{
    static class DriveQuery
    {
        public static string GetSignatures(bool isNotMicrosoftSigned)
        {
            bool debugOutput = false;
            #if DEBUG
                debugOutput = true;
            #endif
            string res = "";
            if (debugOutput)
            {
                Console.WriteLine("[+] Enumerating driver services...");
            }
            Dictionary<string, string> drivers = EnumAllKernelDriverServices();
            FileVersionInfo fileInfo;
            X509Certificate cert;

            if (debugOutput)
            {
                Console.WriteLine("[+] Checking file signatures...");
            }
            foreach (KeyValuePair<string, string> kvp in drivers)
            {
                string serviceName = kvp.Key;
                string driverFile = kvp.Value;
                FileInfo finfo = new FileInfo(driverFile);
                X509Certificate2 cert2;

                try
                {
                    fileInfo = FileVersionInfo.GetVersionInfo(driverFile);

                    cert = X509Certificate.CreateFromSignedFile(driverFile);
                    cert2 = new X509Certificate2(cert.Handle);
                }
                catch (CryptographicException)
                {
                    if (debugOutput)
                    {
                        Console.WriteLine("[-] Invalid certificate handle on {0}. Skipping...", driverFile);
                    }
                    continue;
                }
                catch (FileNotFoundException)
                {
                    if (debugOutput)
                    {
                        Console.WriteLine("[-] Couldn't find the file {0}. Skipping...", driverFile);

                    }
                    continue;
                }

                if (isNotMicrosoftSigned)
                {
                    if (Convert.ToString(cert2.Subject).Contains("Microsoft Corporation"))
                    {
                        continue;
                    }
                    else
                    {
                        //Driver details
                        if (debugOutput)
                        {
                            Console.WriteLine("{0}\n" +
                            "    Service Name: {1}\n" +
                            "    Path: {2}\n" +
                            "    Version: {3}\n" +
                            "    Creation Time (UTC): {4}\n" +
                            "    Cert Issuer: {5}\n" +
                            "    Signer: {6}\n",
                            fileInfo.FileDescription, serviceName, driverFile,
                            fileInfo.FileVersion, finfo.CreationTimeUtc,
                            cert2.Issuer.ToString(), cert2.Subject.ToString());
                        }
                        res += String.Format("{0}\n" +
                            "    Service Name: {1}\n" +
                            "    Path: {2}\n" +
                            "    Version: {3}\n" +
                            "    Creation Time (UTC): {4}\n" +
                            "    Cert Issuer: {5}\n" +
                            "    Signer: {6}\n*********\n",
                            fileInfo.FileDescription, serviceName, driverFile,
                            fileInfo.FileVersion, finfo.CreationTimeUtc,
                            cert2.Issuer.ToString(), cert2.Subject.ToString());
                    }
                }
                else
                {
                    //Driver details
                    if (debugOutput)
                    {
                        Console.WriteLine("{0}\n" +
                            "    Service Name: {1}\n" +
                            "    Path: {2}\n" +
                            "    Version: {3}\n" +
                            "    Creation Time (UTC): {4}\n" +
                            "    Cert Issuer: {5}\n" +
                            "    Signer: {6}\n",
                            fileInfo.FileDescription, serviceName, driverFile,
                            fileInfo.FileVersion, finfo.CreationTimeUtc,
                            cert2.Issuer.ToString(), cert2.Subject.ToString());
                    }
                }
            }
            return res;
        }

        static Dictionary<string, string> EnumAllKernelDriverServices()
        {
            Dictionary<string, string> drivers = new Dictionary<string, string>();

            ServiceController[] services = ServiceController.GetDevices();
            foreach (ServiceController service in services)
            {
                if ((service.ServiceType & ServiceType.KernelDriver) != 0)
                {
                    using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + service.ServiceName + "'"))
                    {
                        try
                        {
                            wmiService.Get();
                            string currentserviceExePath = Environment.ExpandEnvironmentVariables(wmiService["PathName"].ToString());

                            if (currentserviceExePath != string.Empty)
                            {
                                drivers.Add(service.ServiceName, FixPath(currentserviceExePath));
                            }
                        }
                        catch (Exception e)
                        {
                            #if DEBUG
                                Console.WriteLine("[/] Error: " + e);
                            #endif
                            continue;
                        }
                    }
                }
            }

            return drivers;
        }
        //Hack to resolve file location issue due to NT paths
        static string FixPath(string oldPath)
        {
            string newPath = oldPath;

            if (oldPath.StartsWith(@"\SystemRoot\"))
            {
                newPath = oldPath.Replace(@"\SystemRoot\", @"C:\Windows\");

            }
            else if (oldPath.StartsWith(@"system32\"))
            {
                newPath = oldPath.Replace(@"system32\", @"C:\Windows\System32\");
            }
            else if (oldPath.StartsWith(@"System32\"))
            {
                newPath = oldPath.Replace(@"System32\", @"C:\Windows\System32\");
            }
            else if (oldPath.StartsWith(@"\??\"))
            {
                newPath = oldPath.Replace(@"\??\", "");
            }
            return newPath;
        }
    }
}
