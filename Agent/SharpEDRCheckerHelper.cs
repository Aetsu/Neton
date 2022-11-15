namespace Neton
{
    class SharpEDRCheckerHelper
    {

        public static void Launch(Settings settings)
        {
            SendInfo(settings);
        }

        static void SendInfo(Settings settings)
        {
            string launchProcesses = SharpEDRChecker.Program.LaunchProcesses();
            string currentProcessModules = SharpEDRChecker.Program.LaunchCurrentProcessModules();
            string launchCheckDirectories = SharpEDRChecker.Program.LaunchCheckDirectories();
            string launchServiceChecker = SharpEDRChecker.Program.LaunchServiceChecker();
            string launchCheckDrivers = SharpEDRChecker.Program.LaunchCheckDrivers();
            object data = new
            {
                launchProcesses = Helpers.B64e(launchProcesses),
                currentProcessModules = Helpers.B64e(currentProcessModules),
                launchCheckDirectories = Helpers.B64e(launchCheckDirectories),
                launchServiceChecker = Helpers.B64e(launchServiceChecker),
                launchCheckDrivers = Helpers.B64e(launchCheckDrivers),
                sandboxId = Helpers.B64e(settings.sandboxId),
                executionId = Helpers.B64e(settings.executionId),
                wave = Helpers.B64e(settings.wave),
                module = Helpers.B64e("sharpedrchecker"),
                version = Helpers.B64e(settings.version),
                md5Hash = Helpers.B64e(settings.md5Hash),
                sha256Hash = Helpers.B64e(settings.sha256Hash),
            };
            Helpers.SendData(settings.url, data);
        }
    }
}
