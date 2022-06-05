using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ScopoERP.Models;
using ScopoERP.Web.Controllers;
using ScopoERP.UserManagement.BLL;
using ScopoERP.UserManagement.ViewModel;

namespace ScopoERP.Web.Controllers
{
    public class AccountController : Controller
    {
        private UserLogic userLogic;
        private AccountLogic accountLogic;
        public AccountController(UserLogic userLogic, AccountLogic accountLogic)
        {
            this.userLogic = userLogic;
            this.accountLogic = accountLogic;

        }

        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {

                    Guid userID = userLogic.GetUserID(model.UserName);
                    Session["UserID"] = userID;
                    Session["AccountID"] = userLogic.GetAccountID(userID);

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            return RedirectToAction("LogOn", "Account");
        }
        
        #region User Management

        [Authorize(Roles = "Admin")]
        public ActionResult Users()

        {
            var data = userLogic.GetAll().ToList();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return HttpNotFound();
            }

            var data = userLogic.GetByUserName(userName);
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = new MultiSelectList(Roles.GetAllRoles());            
            ViewBag.Accounts = new SelectList(accountLogic.GetAccountDropdown(), "Value", "Text", data.AccountId);
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(Roles.GetAllRoles());
                ViewBag.Accounts = new SelectList(accountLogic.GetAccountDropdown(), "Value", "Text");
                return View(model);
            }

            var existingRoles = Roles.GetRolesForUser(model.UserName);
            if (existingRoles != null && existingRoles.Count() > 0)
            {
                Roles.RemoveUserFromRoles(model.UserName, existingRoles);
            }
            Roles.AddUserToRoles(model.UserName, model.RoleNames.ToArray());

            //accountLogic.UpdateUserAccount(model);
            return RedirectToAction("Users");
        }
        
        #endregion
        
        [Authorize(Roles ="Admin")]
        public ActionResult CreateUser()
        {
            ViewBag.Accounts = new SelectList(accountLogic.GetAccountDropdown(), "Value", "Text");
            ViewBag.Roles = new SelectList(Roles.GetAllRoles());
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    if (model.Roles.Count() > 0)
                    {
                        Roles.AddUserToRoles(model.UserName, model.Roles);
                    }
                    return RedirectToAction("Users", "Account");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Accounts = new SelectList(accountLogic.GetAccountDropdown(), "Value", "Text");
            ViewBag.Roles = new SelectList(Roles.GetAllRoles());
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
