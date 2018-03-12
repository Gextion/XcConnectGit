using System;
using XcConnect.Models.Security;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using XcConnect.Models;

namespace XcConnect.Helpers
{
    /// <summary>
    /// Application Context
    /// </summary>
    public sealed class ApplicationContext
    {
        private static Dictionary<string, ApplicationUser> applicationUsers = new Dictionary<string, ApplicationUser>();
        private static List<Models.Actividades> todayactivities = null;
        private static DateTime lastLoadAct = DateTime.Now;

        /// <summary>
        /// Get Current User
        /// </summary>
        public static ApplicationUser CurrentUser
        {
            get
            {
                return GetUser(HttpContext.Current.User.Identity.Name);
            }
        }

        /// <summary>
        /// Actividades del Usuario en Contexto y que vence hoy.
        /// </summary>
        public static List<Actividades> ActividadesDeHoy
        {
            get
            {
                if (todayactivities == null)
                {
                    LoadActivities();
                }
                else
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan span = endTime.Subtract(lastLoadAct);

                    if (span.Minutes >= 2)
                        LoadActivities();
                }

                if (todayactivities == null)
                    todayactivities = new List<Actividades>();

                return todayactivities;
            }
        }

        /// <summary>
        /// Load Activities
        /// </summary>
        private static void LoadActivities()
        {
            CRMContext db = new CRMContext();
            todayactivities = db.Actividades.Where(ac => ac.FechaEntrega == DateTime.Today && ac.UserID == CurrentUser.Id).OrderBy(ac => ac.FechaEntrega).Take(10).ToList();

            lastLoadAct = DateTime.Now;
        }

        /// <summary>
        /// Load DB User Data
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static ApplicationUser GetUser(string username)
        {
            ApplicationUser user = null;

            if (!string.IsNullOrEmpty(username))
            {
                lock (applicationUsers)
                {
                    if (!applicationUsers.TryGetValue(username, out user))
                    {
                        SecurityDbContext _db = new SecurityDbContext();
                        user = _db.Users.Where(Usr => Usr.UserName.Equals(username)).FirstOrDefault();
                        applicationUsers.Add(username, user);
                    }
                }
            }

            if (user == null)
            {
                user = new ApplicationUser();
                user.FirstName = "Anónimo";
            }

            return user;
        }
    }
}