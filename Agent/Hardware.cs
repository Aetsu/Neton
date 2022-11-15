using System;
using System.Collections.Generic;
using System.Management;


namespace Neton
{
    class Hardware
    {
        //Check if HDD has specific name
        public string checkHdName()
        {
            string response = "";
            List<string> lRes = new List<string>();

            ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in moSearcher.Get())
            {
                try
                {
                    string pInfo = string.Format("{0} | {1}", wmi_HD["Model"].ToString(), wmi_HD["PNPDeviceID"].ToString());
                    lRes.Add(pInfo);
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            response = string.Join("\n", lRes.ToArray());
            return response;
        }

        //Check if CPU temperature information is available
        public string checkTemp()
        {
            string response = "";
            List<string> lRes = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            try
            {
                searcher.Get();
                foreach (ManagementObject queryObj in searcher.Get())
                {

                    double temp = Convert.ToDouble(queryObj["CurrentTemperature"].ToString());
                    double temp_cel = (temp / 10 - 273.15);
                    string pInfo = string.Format("CPU Temperature | {0}", temp_cel.ToString());
                    lRes.Add(pInfo);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    //Console.WriteLine("[/] Error: " + e);
                #endif
                string pInfo = string.Format("CPU Temperature | False");
                lRes.Add(pInfo);
            }
            response = string.Join("\n", lRes.ToArray());
            return response;
        }
    }
}
