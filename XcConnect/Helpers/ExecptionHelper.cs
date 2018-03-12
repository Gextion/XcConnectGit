using System;

namespace XcConnect.Helpers
{
    /// <summary>
    /// Exception Helper
    /// </summary>
    public static class ExecptionHelper
    {
        /// <summary>
        /// Get Exception Detail as String
        /// </summary>
        /// <param name="eX"></param>
        /// <returns></returns>
        public static string GetDetail(Exception eX)
        {
            if (eX != null)
            {
                System.Text.StringBuilder StrB = new System.Text.StringBuilder();
                StrB.Append("");

                if (!string.IsNullOrEmpty(eX.Message))
                {
                    StrB.Append($"Mensaje: {eX.Message}");
                    StrB.Append("");
                    StrB.AppendLine();
                }

                if (eX.InnerException != null && !string.IsNullOrEmpty(eX.InnerException.Message))
                {
                    StrB.Append($"Mensaje Interno: {eX.InnerException.Message}");
                    StrB.Append("");
                    StrB.AppendLine();
                }

                if (!string.IsNullOrEmpty(eX.StackTrace))
                {
                    StrB.Append($"Traza: {eX.StackTrace}");
                    StrB.Append("");
                    StrB.AppendLine();
                }

                return StrB.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}