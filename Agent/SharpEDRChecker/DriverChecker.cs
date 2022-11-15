﻿/* original code: https://github.com/PwnDexter/SharpEDRChecker
 * author: PwnDexter
   github: https://github.com/PwnDexter
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpEDRChecker
{
    internal class DriverChecker
    {
        [DllImport("psapi")]
        private static extern bool EnumDeviceDrivers(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)][In][Out] UIntPtr[] ddAddresses,
            uint arraySizeBytes,
            [MarshalAs(UnmanagedType.U4)] out uint bytesNeeded);

        [DllImport("psapi")]
        private static extern int GetDeviceDriverBaseName(
            UIntPtr ddAddress,
            StringBuilder ddBaseName,
            int baseNameStringSizeChars);

        [DllImport("psapi")]
        private static extern int GetDeviceDriverFileName(
            UIntPtr ddAddress,
            StringBuilder ddBaseName,
            int baseNameStringSizeChars);

        internal static string CheckDrivers()
        {
            try
            {
                Console.WriteLine("####################################");
                Console.WriteLine("[!][!][!] Checking drivers [!][!][!]");
                Console.WriteLine("####################################\n");
                uint numberOfDrivers;
                UIntPtr[] driverAddresses;

                uint sizeOfDriverArrayInBytes = GetSizeOfDriversArray();
                if (sizeOfDriverArrayInBytes == 0)
                {
                    Console.WriteLine("[-] Error getting driver array size");
                    return "Driver checks errored";
                }

                uint sizeOfOneDriverAddress = (uint)UIntPtr.Size;
                numberOfDrivers = sizeOfDriverArrayInBytes / sizeOfOneDriverAddress;
                driverAddresses = new UIntPtr[numberOfDrivers];

                bool success = EnumDeviceDrivers(driverAddresses, sizeOfDriverArrayInBytes, out sizeOfDriverArrayInBytes);

                if (!success)
                {
                    Console.WriteLine("[-] Call to EnumDeviceDrivers failed!");
                    int error = Marshal.GetLastWin32Error();
                    Console.WriteLine("[-] The last Win32 Error was: " + error);
                    return "Driver checks errored";
                }

                var summary = IterateOverDrivers(numberOfDrivers, driverAddresses);

                if (string.IsNullOrEmpty(summary))
                {
                    Console.WriteLine("[+] No suspicious drivers found\n");
                    return "No suspicious drivers found";
                }
                return $"{summary}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking drivers: {e.Message}\n{e.StackTrace}");
                return "Driver checks errored";
            }
        }

        private static string IterateOverDrivers(uint arraySize, UIntPtr[] ddAddresses)
        {
            var summary = "";
            for (int i = 0; i < arraySize; i++)
            {
                var driverFileName = GetDriverFileName(ddAddresses[i]);
                var driverBaseName = GetDriverBaseName(ddAddresses[i]);
                summary += CheckDriver(driverFileName, driverBaseName);
            }
            return summary;
        }

        internal static string CheckDriver(string driverFileName, string driverBaseName)
        {
            try
            {
                var fixedDriverPath = driverFileName.ToLower().Replace(@"\systemroot\".ToLower(), @"c:\windows\".ToLower());
                if (fixedDriverPath.StartsWith(@"\windows\"))
                {
                    fixedDriverPath = fixedDriverPath.Replace(@"\windows\".ToLower(), @"c:\windows\".ToLower());
                }
                else if (fixedDriverPath.ToLower().StartsWith(@"\??\"))
                {
                    fixedDriverPath = fixedDriverPath.ToLower().Replace(@"\??\", @"");
                }
                var metadata = $"{FileChecker.GetFileInfo(fixedDriverPath)}";
                var allattribs = $"{driverBaseName} - {metadata}";

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
                    Console.WriteLine("[-] Suspicious driver found:" +
                                $"\n\tSuspicious Module: {driverBaseName}" +
                                $"\n\tFile Metadata: {metadata}" +
                                $"\n[!] Matched on: {string.Join(", ", matches.ToArray())}\n");
                    return $"{driverBaseName} : {string.Join(", ", matches.ToArray())}\n";
                }
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Errored on checking driver {driverBaseName} : {driverFileName}\n{e.Message}\n{e.StackTrace}");
                return $"{driverBaseName} : Failed to perform checks\n";
            }
        }

        private static string GetDriverBaseName(UIntPtr driverAddress)
        {
            StringBuilder driverBaseNamesb = new StringBuilder(1000);
            int result = GetDeviceDriverBaseName(driverAddress, driverBaseNamesb, driverBaseNamesb.Capacity);

            if (result == 0)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("[-] The last Win32 Error was: " + error);
            }
            return driverBaseNamesb.ToString();
        }

        private static string GetDriverFileName(UIntPtr driverAddress)
        {
            StringBuilder driverFileNamesb = new StringBuilder(1000);
            int result = GetDeviceDriverFileName(driverAddress, driverFileNamesb, driverFileNamesb.Capacity);

            if (result == 0)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("[-] The last Win32 Error was: " + error);
            }
            return driverFileNamesb.ToString();
        }

        private static uint GetSizeOfDriversArray()
        {
            uint bytesNeeded;
            bool success = EnumDeviceDrivers(null, 0, out bytesNeeded);

            if (!success)
            {
                Console.WriteLine("[-] Call to EnumDeviceDrivers failed!");
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("[-] The last Win32 Error was: " + error);
                return 0;
            }
            return bytesNeeded;
        }
    }
}