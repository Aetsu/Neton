using System;
using System.Collections.Generic;
using System.IO;

namespace Neton
{
    public class Filesystem
    {
        //Check if specific files exist
        public string checkFiles()
        {
            List<string> lRes = new List<string>();
            string[] list1 = { @"c:\take_screenshot.ps1", @"c:\loaddll.exe", @"c:\email.doc", @"c:\email.htm", @"c:\123\email.doc", @"c:\123\email.docx", @"c:\a\foobar.bmp", @"c:\a\foobar.doc", @"c:\a\foobar.gif", @"c:\symbols\aagmmc.pdb" };
            foreach (string f in list1)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        string info = string.Format("{0} | {1}", "General", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }

            }
            string[] list2 = { @"c:\windows\system32\drivers\prleth.sys", @"c:\windows\system32\drivers\prlfs.sys", @"c:\windows\system32\drivers\prlmouse.sys", @"c:\windows\system32\drivers\prlvideo.sys", @"c:\windows\system32\drivers\prltime.sys", @"c:\windows\system32\drivers\prl_pv32.sys", @"c:\windows\system32\drivers\prl_paravirt_32.sys" };
            foreach (string f in list2)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        string info = string.Format("{0} | {1}", "Parallels", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            string[] list3 = { @"c:\windows\system32\drivers\VBoxMouse.sys", @"c:\windows\system32\drivers\VBoxGuest.sys", @"c:\windows\system32\drivers\VBoxSF.sys", @"c:\windows\system32\drivers\VBoxVideo.sys", @"c:\windows\system32\vboxdisp.dll", @"c:\windows\system32\vboxhook.dll", @"c:\windows\system32\vboxmrxnp.dll", @"c:\windows\system32\vboxogl.dll", @"c:\windows\system32\vboxoglarrayspu.dll", @"c:\windows\system32\vboxoglcrutil.dll", @"c:\windows\system32\vboxoglerrorspu.dll", @"c:\windows\system32\vboxoglfeedbackspu.dll", @"c:\windows\system32\vboxoglpackspu.dll", @"c:\windows\system32\vboxoglpassthroughspu.dll", @"c:\windows\system32\vboxservice.exe", @"c:\windows\system32\vboxtray.exe", @"c:\windows\system32\VBoxControl.exe" };
            foreach (string f in list3)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        string info = string.Format("{0} | {1}", "VirtualBox", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            string[] list4 = { @"c:\windows\system32\drivers\vmsrvc.sys", @"c:\windows\system32\drivers\vpc-s3.sys" };
            foreach (string f in list4)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        string info = string.Format("{0} | {1}", "VirtualPC", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            string[] list5 = { @"c:\windows\system32\drivers\vmmouse.sys", @"c:\windows\system32\drivers\vmnet.sys", @"c:\windows\system32\drivers\vmxnet.sys", @"c:\windows\system32\drivers\vmhgfs.sys", @"c:\windows\system32\drivers\vmx86.sys", @"c:\windows\system32\drivers\hgfs.sys" };
            foreach (string f in list5)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        string info = string.Format("{0} | {1}", "VMWare", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            string response = string.Join("\n", lRes.ToArray());
            return response;
        }

        

       

        //Check if the executable files with specific names are present in physical disk drives' root
        public string checkExeRoot()
        {
            List<string> lRes = new List<string>();
            string[] list1 = { "malware.exe", "sample.exe" };
            foreach (string f in list1)
            {
                try
                {
                    if (File.Exists(Path.GetPathRoot(Environment.SystemDirectory) + "\\" + f))
                    {
                        string info = string.Format("{0} | {1}", "General", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            string[] list2 = { @"c:\insidetm" };
            foreach (string f in list2)
            {
                try
                {
                    if (Directory.GetCurrentDirectory().ToLower().Contains(f.ToLower()))
                    {
                        string info = string.Format("{0} | {1}", "Anubis", f);
                        lRes.Add(info);
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("[/] Error: " + e);
                    #endif
                }
            }
            string response = string.Join("\n", lRes.ToArray());
            return response;
        }
    }
}
