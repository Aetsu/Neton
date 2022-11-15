using System;
using System.Net;

namespace Neton
{
    class CheckSandbox
    {
        public static void SendInfo(Settings settings)
        {
            try
            {
                Filesystem fsChecks = new Filesystem();
                RegistryQuery rQuery = new RegistryQuery();
                UiArtifacts uiArtifact = new UiArtifacts();
                OsFeatures osFeature = new OsFeatures();
                Hardware hwHelper = new Hardware();

                string checkFiles = fsChecks.checkFiles();
                string checkExeRoot = fsChecks.checkExeRoot();
                string checkPath = rQuery.checkPath();
                string checkKeyValue = rQuery.checkKeyValue();
                string checkWindowTitle = uiArtifact.checkWindowTitle();
                string checkNWindows = uiArtifact.checkNWindows();
                string checkDebugPrivs = osFeature.checkDebugPrivs();
                string checkHdName = hwHelper.checkHdName();
                string checkTemp = hwHelper.checkTemp();

                object data = new
                {
                    checkFilesTags = Helpers.B64e(checkFiles),
                    checkExeRootTags = Helpers.B64e(checkExeRoot),
                    checkPathTags = Helpers.B64e(checkPath),
                    checkKeyValueTags = Helpers.B64e(checkKeyValue),
                    checkWindowTitleTags = Helpers.B64e(checkWindowTitle),
                    checkNWindowsTags = Helpers.B64e(checkNWindows),
                    checkDebugPrivsTags = Helpers.B64e(checkDebugPrivs),
                    checkHdName = Helpers.B64e(checkHdName),
                    checkTemp = Helpers.B64e(checkTemp),
                    sandboxId = Helpers.B64e(settings.sandboxId),
                    executionId = Helpers.B64e(settings.executionId),
                    wave = Helpers.B64e(settings.wave),
                    module = Helpers.B64e("checksandbox"),
                    version = Helpers.B64e(settings.version),
                    md5Hash = Helpers.B64e(settings.md5Hash),
                    sha256Hash = Helpers.B64e(settings.sha256Hash),
                };
                Helpers.SendData(settings.url, data);
            }
            catch (WebException e)
            {
                #if DEBUG
                    Console.WriteLine("[/] Error: " + e);
                #endif
            }
        }
        public static void Launch(Settings settings)
        {
            SendInfo(settings);
        }
    }
}
