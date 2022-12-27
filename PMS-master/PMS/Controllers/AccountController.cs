using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Models;
using PMS.Models.Context;

namespace PMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            UserViewModel model = new UserViewModel();
            model.ApplicationRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToList();
          
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            model.ApplicationRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToList();
            if (ModelState.IsValid)
            {
                var usernameOrEmail = this.userManager.Users.FirstOrDefault(p => p.Email.Equals(model.Email) || p.UserName.Equals(model.UserName));
                if (usernameOrEmail != null)
                {
                    if (usernameOrEmail.Email.Equals(model.Email))
                    {
                        ModelState.AddModelError("Email", "user with same email already exist");
                        return View(model);
                    }
                    if (usernameOrEmail.UserName.Equals(model.UserName))
                    {
                        ModelState.AddModelError("UserName", "username already taken");
                        return View(model);
                    }
                }
                ApplicationUser user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email
                };
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);
                user.UserType = applicationRole.Name;
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            var signinResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
                            if (signinResult.Succeeded)
                            {
                                return RedirectToAction("Index", "Rooms");
                            }
                           
                        }
                    }
                }
                else
                {
                    result.Errors.ToList().ForEach(p => { 
                        ModelState.AddModelError(p.Code,p.Description.ToString());
                    });
                    return View(model);
                }
            }
            
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Rooms");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult MyProfile()
        {
            ViewBag.Message = "";
            var UserId = signInManager.Context.User.Claims.FirstOrDefault().Value;
            var user = userManager.Users.FirstOrDefault(p => p.Id.Equals(Convert.ToInt32(UserId)));
            ProfileViewModel model = new ProfileViewModel();
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.Password = "";
            return View(model);
        }
        [HttpPost]
        public ActionResult MyProfile(ProfileViewModel profileViewModel)
        {
            ViewBag.Message = "";
            if (String.IsNullOrEmpty(profileViewModel.UserName))
            {
                ModelState.AddModelError("Username", "Username field is required");
                return View(profileViewModel);
            }
            else if (String.IsNullOrEmpty(profileViewModel.Email))
            {
                ModelState.AddModelError("Email", "Email field is required");
                return View(profileViewModel);
            }
            else
            {
                var usernameOrEmail = this.userManager.Users.FirstOrDefault(p => p.Email.Equals(profileViewModel.Email) || p.UserName.Equals(profileViewModel.UserName));
                if (usernameOrEmail != null)
                {
                    if (usernameOrEmail.Email.Equals(profileViewModel.Email))
                    {
                        ModelState.AddModelError("Email", "user with same email already exist");
                        return View(profileViewModel);
                    }
                    if (usernameOrEmail.UserName.Equals(profileViewModel.UserName))
                    {
                        ModelState.AddModelError("UserName", "username already taken");
                        return View(profileViewModel);
                    }
                }
                var UserId = signInManager.Context.User.Claims.FirstOrDefault().Value;
                var user = userManager.Users.FirstOrDefault(p => p.Id.Equals(Convert.ToInt32(UserId)));
                user.UserName = profileViewModel.UserName;
                user.Email = profileViewModel.Email;
                userManager.UpdateAsync(user);
                ViewBag.Message = "Profile updated successfully";
                return View(profileViewModel);
            }
            
            
        }
        public ActionResult ChangePassword()
        {
            ViewBag.Message = "";           
            ProfileViewModel model = new ProfileViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ProfileViewModel profileViewModel)
        {
            ViewBag.Message = "";
            var UserId = signInManager.Context.User.Claims.FirstOrDefault().Value;
            var user = userManager.Users.FirstOrDefault(p => p.Id.Equals(Convert.ToInt32(UserId)));

            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var result = await this.userManager.ResetPasswordAsync(user, token, profileViewModel.Password);
            if (result.Succeeded)
            {
                ViewBag.Message = "Password changed successfully";
                return View(profileViewModel);
            }
            else
            {
                result.Errors.ToList().ForEach(p => {
                    ModelState.AddModelError(p.Code, p.Description.ToString());
                });
                return View(profileViewModel);
            }
        }
    }
}
