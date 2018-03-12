using System.Diagnostics;

namespace XcConnect.Helpers
{
    /// <summary>
    /// Sys Helper
    /// </summary>
    public static class SysHelper
    {
        /// <summary>
        /// Get SYstem Version
        /// </summary>
        /// <returns></returns>
        public static string GetSysVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}