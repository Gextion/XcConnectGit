using XcConnect.Models.Security;
using XcConnect.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using XcConnect.Helpers;

namespace XcConnect.Controllers
{
    /// <summary>
    /// Account Controller (Users, Groups, Roles, Permissions)
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class AccountController : Controller
    {
        private SecurityDbContext db = new SecurityDbContext();

        /// <summary>
        /// User Manager
        /// </summary>
        public UserManager<ApplicationUser> UserManager { get; private set; }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        public AccountController()
           : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext())))
        {
        }

        /// <summary>
        /// Overload Constructor
        /// </summary>
        /// <param name="userManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        // GET: Account
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser == null || Helpers.ApplicationContext.CurrentUser.Empresa == null)
                return View("../Authentication/Login");

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                var identityUsers = db.Users.Include(a => a.Empresa).OrderBy(u => u.UserName);
                return View(identityUsers.ToList());
            }
            else
            {
                var identityUsers = db.Users.Include(a => a.Empresa).Where(u => u.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID)).OrderBy(u => u.UserName);
                return View(identityUsers.ToList());
            }
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser editUserViewModel = db.Users.Find(id);
            if (editUserViewModel == null)
            {
                return HttpNotFound();
            }
            return View(editUserViewModel);
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Register()
        {
            SetViewBagListData(Helpers.ApplicationContext.CurrentUser.EmpresaID);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "Id,Email,PasswordHash,PasswordConfirm,PhoneNumber,UserName,FirstName,LastName,EmpresaID")] ApplicationUser editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                if (editUserViewModel.PasswordHash.Equals(editUserViewModel.PasswordConfirm))
                {
                    var result = await UserManager.CreateAsync(editUserViewModel, editUserViewModel.PasswordHash);
                    if (result.Succeeded)
                    {
                        IdentityManager idManager = new IdentityManager();
                        SecurityDbContext Db = new SecurityDbContext();
                        var group = Db.Groups.Where(gr => gr.Name.Equals(Startup.SellerName)).FirstOrDefault();
                        if (group != null)
                            idManager.AddUserToGroup(editUserViewModel.Id, group.Id);

                        return RedirectToAction("UserGroups", "Account", new { id = editUserViewModel.UserName });
                    }
                    else
                    {
                        ModelState.AddModelError("", String.Join(", ", result.Errors.Select(u => u.ToString())));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "La contraseña y su confirmación no son iguales");
                }
            }

            SetViewBagListData(editUserViewModel.EmpresaID);
            return View(editUserViewModel);
        }
        
        [Authorize(Roles = "AccessAll, BusinessEntity")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser editUserViewModel = db.Users.Find(id);
            if (editUserViewModel == null)
            {
                return HttpNotFound();
            }

            SetViewBagListData(editUserViewModel.EmpresaID);

            return View(editUserViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,UserName,FirstName,LastName,EmpresaID")] ApplicationUser editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(editUserViewModel).State = EntityState.Modified;
                db.SaveChanges();

                if (ApplicationContext.CurrentUser.IsSuperAdmin || ApplicationContext.CurrentUser.IsAdmin)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Dashboards");
                }
            }

            SetViewBagListData(editUserViewModel.EmpresaID);
            return View(editUserViewModel);
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser editUserViewModel = db.Users.Find(id);
            if (editUserViewModel == null)
            {
                return HttpNotFound();
            }
            return View(editUserViewModel);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser editUserViewModel = db.Users.Find(id);
            db.Users.Remove(editUserViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult UserGroups(string id)
        {
            var user = db.Users.First(u => u.UserName == id);
            var model = new SelectUserGroupsViewModel(user);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        [ValidateAntiForgeryToken]
        public ActionResult UserGroups(SelectUserGroupsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var user = db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserGroups(user.Id);
                foreach (var group in model.Groups)
                {
                    if (group.Selected)
                    {
                        idManager.AddUserToGroup(user.Id, group.GroupId);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }

        [Authorize(Roles = "AccessAll, Companies, BusinessEntity")]
        public ActionResult UserPermissions(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == id);
            var model = new UserPermissionsViewModel(user);
            return View(model);
        }

        [Authorize(Roles = "AccessAll, Admin, BusinessEntity")]
        public ActionResult Manage(ManageMessageId? message, string id)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Tu contraseña ha sido cambiada."
                : message == ManageMessageId.SetPasswordSuccess ? "Se ha establecido la contraseña."
                : message == ManageMessageId.RemoveLoginSuccess ? "Se ha eliminado el inicio de sesión externo."
                : message == ManageMessageId.Error ? "Se ha producido un error."
                : "";
            ViewBag.HasLocalPassword = HasPassword(id);
            ViewBag.ReturnUrl = Url.Action("Manage");

            ManageUserViewModel model = new ManageUserViewModel();
            model.UserId = id;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AccessAll, Admin, BusinessEntity")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
           bool hasPassword = HasPassword(model.UserId);
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid || (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin && !String.IsNullOrEmpty(model.UserId) && !String.IsNullOrEmpty(model.NewPassword)))
                {
                    IdentityResult result = null;

                    if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                    {
                        result = await UserManager.RemovePasswordAsync(model.UserId);
                        if (result.Succeeded)
                        {
                            result = await UserManager.AddPasswordAsync(model.UserId, model.NewPassword);
                        }
                        else
                        {
                            ModelState.AddModelError("", "No fue posible remover la contraseña actual del usuario. Intente de nuevo.");
                        }
                    }
                    else
                    {
                        result = await UserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
                    }
                    
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Edit", new { id = model.UserId });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(model.UserId, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Edit", new { id = model.UserId });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Has Password
        /// </summary>
        /// <returns></returns>
        private bool HasPassword(string UserId)
        {
            var user = UserManager.FindById(UserId);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        /// <summary>
        /// Add Error To Model State
        /// </summary>
        /// <param name="result"></param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        /// <summary>
        /// Dispose Resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Set ViewBag Data
        /// </summary>
        private void SetViewBagListData(object EmpresaSelectedValue)
        {
            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", EmpresaSelectedValue);
            }
            else
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas
                    .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID))
                    .OrderBy(e => e.Nombre), "EmpresaID", "Nombre", EmpresaSelectedValue);
            }
        }
    }
}
