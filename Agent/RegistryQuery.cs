using Microsoft.Win32;
using System;
using System.Collections.Generic;


namespace Neton
{
    class RegistryQuery
    {
        //Check if particular registry paths exist
        public string checkPath()
        {
            List<string> lRes = new List<string>();
            //VMware
            try
            {
                RegistryKey regUser = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VMware, Inc.\VMware Tools", false);
                if (regUser != null)
                {
                    string pInfo = string.Format("{0} | {1}", "VMware", @"HKCU\SOFTWARE\VMware, Inc.\VMware Tools");
                    lRes.Add(pInfo);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            string[] list1 = { @"SOFTWARE\VMware, Inc.\VMware Tools", @"SYSTEM\ControlSet001\Services\vmdebug", @"SYSTEM\ControlSet001\Services\vmmouse", @"SYSTEM\ControlSet001\Services\VMTools", @"SYSTEM\ControlSet001\Services\VMMEMCTL", @"SYSTEM\ControlSet001\Services\vmware", @"SYSTEM\ControlSet001\Services\vmci", @"SYSTEM\ControlSet001\Services\vmx86" };
            foreach (string s in list1)
            {
                try
                {
                    RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(s, false);
                    if (regMachine != null)
                    {
                        string pInfo = string.Format("{0} | {1}", "VMware", "HKLM\\" + s);
                        lRes.Add(pInfo);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            //General
            try
            {
                RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(@"Software\Classes\Folder\shell\sandbox", false);
                if (regMachine != null)
                {
                    string pInfo = string.Format("{0} | {1}", "General", @"HKLM\Software\Classes\Folder\shell\sandbox");
                    lRes.Add(pInfo);
                }
            }
            catch (Exception e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
            //Hyper-V
            string[] list2 = { @"SOFTWARE\Microsoft\Hyper-V", @"SOFTWARE\Microsoft\VirtualMachine", @"SOFTWARE\Microsoft\Virtual Machine\Guest\Parameters", @"SYSTEM\ControlSet001\Services\vmicheartbeat", @"SYSTEM\ControlSet001\Services\vmicvss", @"SYSTEM\ControlSet001\Services\vmicshutdown", @"SYSTEM\ControlSet001\Services\vmicexchange" };
            foreach (string s in list2)
            {
                try
                {
                    RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(s, false);
                    if (regMachine != null)
                    {
                        string pInfo = string.Format("{0} | {1}", "Hyper-V", "HKLM\\" + s);
                        lRes.Add(pInfo);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            //Sandboxie
            string[] list3 = { @"SYSTEM\CurrentControlSet\Services\SbieDrv", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sandboxie" };
            foreach (string s in list3)
            {
                try
                {
                    RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(s, false);
                    if (regMachine != null)
                    {
                        string pInfo = string.Format("{0} | {1}", "Sandboxie", "HKLM\\" + s);
                        lRes.Add(pInfo);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            //VirtualBox
            string[] list4 = { @"HARDWARE\ACPI\DSDT\VBOX__", @"HARDWARE\ACPI\FADT\VBOX__", @"HARDWARE\ACPI\RSDT\VBOX__", @"SOFTWARE\Oracle\VirtualBox Guest Additions", @"SYSTEM\ControlSet001\Services\VBoxGuest", @"SYSTEM\ControlSet001\Services\VBoxMouse", @"SYSTEM\ControlSet001\Services\VBoxService", @"SYSTEM\ControlSet001\Services\VBoxSF", @"SYSTEM\ControlSet001\Services\VBoxVideo" };
            foreach (string s in list4)
            {
                try
                {
                    RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(s, false);
                    if (regMachine != null)
                    {
                        string pInfo = string.Format("{0} | {1}", "VirtualBox", "HKLM\\" + s);
                        lRes.Add(pInfo);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            //VirtualPC
            string[] list5 = { @"SYSTEM\ControlSet001\Services\vpcbus", @"SYSTEM\ControlSet001\Services\vpc-s3", @"SYSTEM\ControlSet001\Services\vpcuhub", @"SYSTEM\ControlSet001\Services\msvmmouf" };
            foreach (string s in list5)
            {
                try
                {
                    RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(s, false);
                    if (regMachine != null)
                    {
                        string pInfo = string.Format("{0} | {1}", "VirtualPC", "HKLM\\" + s);
                        lRes.Add(pInfo);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            //Xen
            string[] list6 = { @"HARDWARE\ACPI\DSDT\xen", @"HARDWARE\ACPI\FADT\xen", @"HARDWARE\ACPI\RSDT\xen", @"SYSTEM\ControlSet001\Services\xenevtchn", @"SYSTEM\ControlSet001\Services\xennet", @"SYSTEM\ControlSet001\Services\xennet6", @"SYSTEM\ControlSet001\Services\xensvc", @"SYSTEM\ControlSet001\Services\xenvdb" };
            foreach (string s in list6)
            {
                try
                {
                    RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(s, false);
                    if (regMachine != null)
                    {
                        string pInfo = string.Format("{0} | {1}", "Xen", "HKLM\\" + s);
                        lRes.Add(pInfo);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            //Wine
            try
            {
                RegistryKey regUser = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Wine", false);
                if (regUser != null)
                {
                    string pInfo = string.Format("{0} | {1}", "Wine", @"SOFTWARE\Wine");
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

        //Check if particular registry keys contain specified strings
        public string checkKeyValue()
        {
            List<string> lRes = new List<string>();
            //VMWare
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0 : Identifier - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 1\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 1\Scsi Bus 0\Target Id 0\Logical Unit Id 0 : Identifier - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 2\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 2\Scsi Bus 0\Target Id 0\Logical Unit Id 0 : Identifier - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosVersion", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\Description\System : SystemBiosVersion - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosVersion", "INTEL  - 6040000"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\Description\System : SystemBiosVersion - INTEL - 6040000");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "VideoBiosVersion", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\Description\System : VideoBiosVersion - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System\BIOS", "SystemProductName", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\HARDWARE\Description\System\BIOS : SystemProductName - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "0", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum : 0 - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "1", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum : 1 - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "DeviceDesc", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum : DeviceDesc - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "FriendlyName", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum : FriendlyName - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "DeviceDesc", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum : DeviceDesc - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet002\Services\Disk\Enum", "FriendlyName", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet002\Services\Disk\Enum : FriendlyName - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet002\Services\Disk\Enum", "DeviceDesc", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet002\Services\Disk\Enum : DeviceDesc - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet003\Services\Disk\Enum", "FriendlyName", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet003\Services\Disk\Enum : FriendlyName - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet003\Services\Disk\Enum", "DeviceDesc", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet003\Services\Disk\Enum : DeviceDesc - VMWARE");
                lRes.Add(info);
            }
            if (checkKey("HKCR", @"Installer\Products", "ProductName", "vmware tools"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKCR\Installer\Products : ProductName - vmware tools");
                lRes.Add(info);
            }
            if (checkKey("HKCU", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", "DisplayName", "vmware tools"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall : DisplayName - vmware tools");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", "DisplayName", "vmware tools"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall : DisplayName - vmware tools");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000", "CoInstallers32", "vmx"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000 : CoInstallers32 - vmx");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000", "DriverDesc", "VMware"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000 : DriverDesc - VMware");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000", "InfSection", "vmx"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000 : InfSection - vmx");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000", "ProviderName", "VMware"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000 : ProviderName - VMware");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000", "Device Description", "VMware"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\ControlSet001\Control\Class\{4D36E968-E325-11CE-BFC1-08002BE10318}\0000 : Device Description - VMware");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\CurrentControlSet\Control\SystemInformation", "SystemProductName", "VMWARE"))
            {
                string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\CurrentControlSet\Control\SystemInformation : SystemProductName - VMWARE");
                lRes.Add(info);
            }
            RegistryKey regMachine = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Video", false);
            string[] valueNames = regMachine.GetSubKeyNames();
            foreach (string entry in valueNames)
            {
                if (checkKey("HKLM", @"SYSTEM\CurrentControlSet\Control\Video\" + entry + @"\Video", "Service", "vm3dmp"))
                {
                    string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\CurrentControlSet\Control\Video\" + entry + @"\Video : Service - vm3dmp");
                    lRes.Add(info);
                }
                if (checkKey("HKLM", @"SYSTEM\CurrentControlSet\Control\Video\" + entry + @"\Video", "Service", "vmx_svga"))
                {
                    string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\CurrentControlSet\Control\Video\" + entry + @"\Video : Service - vmx_svga");
                    lRes.Add(info);
                }
                if (checkKey("HKLM", @"SYSTEM\CurrentControlSet\Control\Video\" + entry + @"\0000", "Device Description", "VMware SVGA"))
                {
                    string info = string.Format("{0} | {1}", "VMware", @"HKLM\SYSTEM\CurrentControlSet\Control\Video\" + entry + @"\0000 : Device Description - VMware SVGA");
                    lRes.Add(info);
                }
            }
            //Xen
            if (checkKey("HKLM", @"HARDWARE\Description\System\BIOS", "SystemProductName", "Xen"))
            {
                string info = string.Format("{0} | {1}", "Xen", @"HKLM\HARDWARE\Description\System\BIOS : SystemProductName - Xen");
                lRes.Add(info);
            }
            //General
            if (checkKey("HKLM", @"HARDWARE\Description\System\BIOS", "SystemProductName", "A M I"))
            {
                string info = string.Format("{0} | {1}", "General", @"HKLM\HARDWARE\Description\System\BIOS : SystemProductName - A M I");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosDate", "06/23/99"))
            {
                string info = string.Format("{0} | {1}", "General", @"HKLM\HARDWARE\Description\System : SystemBiosDate - 06/23/99");
                lRes.Add(info);
            }
            //BOCHS
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosVersion", "BOCHS"))
            {
                string info = string.Format("{0} | {1}", "BOCHS", @"HKLM\HARDWARE\Description\System : SystemBiosVersion - BOCHS");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "VideoBiosVersion", "BOCHS"))
            {
                string info = string.Format("{0} | {1}", "BOCHS", @"HKLM\HARDWARE\Description\System : VideoBiosVersion - BOCHS");
                lRes.Add(info);
            }
            //Anubis
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows\CurrentVersion", "ProductID", "76487-337-8429955-22614"))
            {
                string info = string.Format("{0} | {1}", "Anubis", @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion : ProductID - 76487-337-8429955-22614");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductID", "76487-337-8429955-22614"))
            {
                string info = string.Format("{0} | {1}", "Anubis", @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion : ProductID - 76487-337-8429955-22614");
                lRes.Add(info);
            }
            //CwSandbox
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows\CurrentVersion", "ProductID", "76487-644-3177037-23510"))
            {
                string info = string.Format("{0} | {1}", "CwSandbox", @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion : ProductID - 76487-644-3177037-23510");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductID", "76487-644-3177037-23510"))
            {
                string info = string.Format("{0} | {1}", "CwSandbox", @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion : ProductID - 76487-644-3177037-23510");
                lRes.Add(info);
            }
            //JoeBox
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows\CurrentVersion", "ProductID", "55274-640-2673064-23950"))
            {
                string info = string.Format("{0} | {1}", "CwSandbox", @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion : ProductID - 55274-640-2673064-23950");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductID", "55274-640-2673064-23950"))
            {
                string info = string.Format("{0} | {1}", "CwSandbox", @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion : ProductID - 55274-640-2673064-23950");
                lRes.Add(info);
            }
            //Parallels
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosVersion", "PARALLELS"))
            {
                string info = string.Format("{0} | {1}", "Parallels", @"HKLM\HARDWARE\Description\System : SystemBiosVersion - PARALLELS");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "VideoBiosVersion", "PARALLELS"))
            {
                string info = string.Format("{0} | {1}", "Parallels", @"HKLM\HARDWARE\Description\System : VideoBiosVersion - PARALLELS");
                lRes.Add(info);
            }
            //QEMU
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "QEMU"))
            {
                string info = string.Format("{0} | {1}", "QEMU", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0 : Identifier - QEMU");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosVersion", "QEMU"))
            {
                string info = string.Format("{0} | {1}", "QEMU", @"HKLM\HKLM\HARDWARE\Description\System : SystemBiosVersion - QEMU");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "VideoBiosVersion", "QEMU"))
            {
                string info = string.Format("{0} | {1}", "QEMU", @"HKLM\HARDWARE\Description\System : VideoBiosVersion - QEMU");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System\BIOS	", "SystemManufacturer", "QEMU"))
            {
                string info = string.Format("{0} | {1}", "QEMU", @"HKLM\HARDWARE\Description\System\BIOS : VideoBiosVersion - QEMU");
                lRes.Add(info);
            }
            //VirtualBox
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0: Identifier - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 1\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 1\Scsi Bus 0\Target Id 0\Logical Unit Id 0: Identifier - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\DEVICEMAP\Scsi\Scsi Port 2\Scsi Bus 0\Target Id 0\Logical Unit Id 0", "Identifier", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\HARDWARE\DEVICEMAP\Scsi\Scsi Port 2\Scsi Bus 0\Target Id 0\Logical Unit Id 0: Identifier - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "SystemBiosVersion", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\HARDWARE\Description\System: SystemBiosVersion - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System", "VideoBiosVersion", "VIRTUALBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\HARDWARE\Description\System: VideoBiosVersion - VIRTUALBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"HARDWARE\Description\System\BIOS", "SystemProductName", "VIRTUAL"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\HARDWARE\Description\System\BIOS: SystemProductName - VIRTUAL");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "DeviceDesc", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum: DeviceDesc - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet001\Services\Disk\Enum", "FriendlyName", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\ControlSet001\Services\Disk\Enum: FriendlyName - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet002\Services\Disk\Enum", "DeviceDesc", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\ControlSet002\Services\Disk\Enum: DeviceDesc - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet002\Services\Disk\Enum", "FriendlyName", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\ControlSet002\Services\Disk\Enum: FriendlyName - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet003\Services\Disk\Enum", "DeviceDesc", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\ControlSet003\Services\Disk\Enum: DeviceDesc - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\ControlSet003\Services\Disk\Enum", "FriendlyName", "VBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\ControlSet003\Services\Disk\Enum: FriendlyName - VBOX");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\CurrentControlSet\Control\SystemInformation", "SystemProductName", "VIRTUAL"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\CurrentControlSet\Control\SystemInformation: SystemProductName - VIRTUAL");
                lRes.Add(info);
            }
            if (checkKey("HKLM", @"SYSTEM\CurrentControlSet\Control\SystemInformation", "SystemProductName", "VIRTUALBOX"))
            {
                string info = string.Format("{0} | {1}", "VirtualBox", @"HKLM\SYSTEM\CurrentControlSet\Control\SystemInformation: SystemProductName - VIRTUALBOX");
                lRes.Add(info);
            }
            string response = string.Join("\n", lRes.ToArray());
            return response;
        }

        private bool checkKey(string root, string registryPath, string key, string value)
        {
            bool output = false;
            try
            {
                RegistryKey auxKey = null;
                if (root == "HKLM")
                {
                    auxKey = Registry.LocalMachine.OpenSubKey(registryPath, false);
                }
                else if (root == "HKCU")
                {
                    auxKey = Registry.CurrentUser.OpenSubKey(registryPath, false);
                }
                else if (root == "HKCR")
                {
                    auxKey = Registry.ClassesRoot.OpenSubKey(registryPath, false);
                }
                if (auxKey != null)
                {
                    object auxValue = auxKey.GetValue(key);
                    if (auxValue is string)
                    {
                        string v = (string)auxValue;
                        if (v.ToLower().Contains(value.ToLower()))
                        {
                            output = true;
                        }
                    }
                    else if (auxValue is string[])
                    {
                        string[] v = (string[])auxValue;
                        foreach (string element in v)
                        {
                            if (element.ToLower().Contains(value.ToLower()))
                            {
                                output = true;
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
                output = false;
            }
            return output;
        }
    }
}
