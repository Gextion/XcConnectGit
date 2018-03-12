using XcConnect.Helpers;
using XcConnect.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Inspinia_MVC5.Controllers
{
	/// <summary>
	/// Page Controller
	/// </summary>
	[NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
	[NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
	public class AuthenticationController : Controller
	{
        private SecurityDbContext db = new SecurityDbContext();

        /// <summary>
        /// Basic Constructor
        /// </summary>
        public AuthenticationController() : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext()) { }))
		{
		}
		/// <summary>
		/// Inti Overload
		/// </summary>
		/// <param name="userManager"></param>
		public AuthenticationController(UserManager<ApplicationUser> userManager)
		{
			UserManager = userManager;
		}

		/// <summary>
		/// User Manager
		/// </summary>
		public UserManager<ApplicationUser> UserManager { get; private set; }

		/// <summary>
		/// Authentication Manager
		/// </summary>
		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		/// <summary>
		/// Present Login View
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Login()
		{
			return View(new XcConnect.Models.LoginModel());
		}

        [AllowAnonymous]
        public ActionResult ForgotPwd()
        {
            return View(new XcConnect.Models.ForgotPwdModel());
        }

        [Authorize(Roles = "AccessAll")]
        public ActionResult Audit()
        {
            ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", ApplicationContext.CurrentUser.EmpresaID);

            var audit = db.AuthenticationAudit.Include(e => e.User).Where(a => a.User.EmpresaID == ApplicationContext.CurrentUser.EmpresaID).OrderByDescending(a => a.LoginDate);
            return View(audit.ToList());
        }

        /// <summary>
        /// View in List Mode
        /// <example> GET: Dispositivos </example>
        /// <param name="idSede"></param>
        /// </summary>    
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult FilterByEmpresaID(int? id)
        {
            if (id != null && id.HasValue)
            {
                List<AuthenticationAudit> ViewData = null;

                if (id.Value == -777)
                {
                    ViewData = (from c in db.AuthenticationAudit select c).OrderByDescending(a => a.LoginDate).ToList();
                }
                else
                {
                    ViewData = (from c in db.AuthenticationAudit where c.User.EmpresaID == id.Value select c).OrderByDescending(a => a.LoginDate).ToList();
                }

                var Empresa = db.Empresas.Where(e => e.EmpresaID.Equals(id.Value)).FirstOrDefault();
                if (Empresa != null)
                {
                    ViewBag.Caption = $"Auditorías para la empresa: {Empresa.Nombre}";
                }
                else
                {
                    ViewBag.Caption = "Auditoría de Ingreso";
                }

                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", id.Value);

                return View("Audit", ViewData);
            }
            else
            {
                return Audit();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendPwd(XcConnect.Models.ForgotPwdModel Model)
        {
            bool HasErrros = false;

            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(Model.UserName);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        ModelState.AddModelError("", "El usuario no tiene un correo electrónico válido.");
                        HasErrros = true;
                    }
                    else
                    {
                        var newPwd = Guid.NewGuid().ToString().Substring(1, 10);
                        user.PasswordHash = new PasswordHasher().HashPassword(newPwd);
                        var Result = UserManager.Update(user);
                        if (Result.Succeeded)
                        {
                            if (!EmailHelper.SendPwdRecoveryEmail(user.Email, newPwd))
                            {
                                ModelState.AddModelError("", "No fue posible enviar el correo electrónico con la nueva contraseña. Por favor, notifique al administrador de la plataforma.");
                                HasErrros = true;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "No fue posible modificar la contraseña del usuario. Por favor, intente de nuevo.");
                            HasErrros = true;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El usuario no existe.");
                    HasErrros = true;
                }
            }
            else
            {
                ModelState.AddModelError("", "Por favor, intente de nuevo.");
                HasErrros = true;
            }

            if (HasErrros)
            {
                return View("ForgotPwd", Model);
            }
            else
            {
                return View("Login");
            }
        }

        /// <summary>
        /// Action Response To Login Button
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Authenticate(XcConnect.Models.LoginModel Model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindAsync(Model.Nit, Model.Phone);
				if (user != null)
				{
					await SignInAsync(user, Model.RememberMe);
                    
                    SecurityDbContext db = new SecurityDbContext();
                    db.AuthenticationAudit.Add(new AuthenticationAudit()
                    {
                        LoginDate = DateHelper.GetColombiaDateTime(),
                        LoginIP = Request.UserHostAddress,
                        LoginBrowser = Request.UserAgent,
                        LoginPlatform = Request.Browser.Platform,
                        LoginBrowserVersion = Request.Browser.Version,
                        UserId = user.Id
                    });
                    db.SaveChanges();

                    return RedirectToAction("Dashboard", "Dashboards");
				}
				else
				{
					ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
				}
			}

			return View("Login", Model);
		}

		/// <summary>
		/// Sign In
		/// </summary>
		/// <param name="user"></param>
		/// <param name="isPersistent"></param>
		/// <returns></returns>
		private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
		}

		/// <summary>
		/// Log Off
		/// </summary>
		/// <returns></returns>
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Login", "Authentication");
		}
	}
}