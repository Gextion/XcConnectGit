using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace XcConnect.Helpers
{
    public static class EmailHelper
    {
        /// <summary>
        /// Send Passwrod Recovery Email
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Pwd"></param>
        public static bool SendPwdRecoveryEmail(string Email, string Pwd)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    MailMessage Mail = new MailMessage();
                    Mail.From = new MailAddress(ConfigurationManager.AppSettings["SmtpUsrCredentials"], ConfigurationManager.AppSettings["SmtpUsrDisplayName"]);
                    Mail.To.Add(new MailAddress(Email));

                    Mail.Subject = "TI CRM - Olvido de Contraseña";
                    Mail.IsBodyHtml = true;
                    Mail.Body = Properties.EmailTemplates.ForgotPwd.Replace("{PWD_NEW}", Pwd);

                    if (bool.Parse(ConfigurationManager.AppSettings["SmtpUseSecurityData"]))
                    {
                        smtp.UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["SmtpUseDefaultCredentials"]);
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUsrCredentials"], ConfigurationManager.AppSettings["SmtpPwdCredentials"]);
                        smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                    }

                    smtp.Send(Mail);
                }

                return true;
            }
            catch (Exception eX)
            {
                Serilog.Log.Information($"Error On SendPwdRecoveryEmail - {ExecptionHelper.GetDetail(eX)}");
                return false;
            }
        }
    }
}