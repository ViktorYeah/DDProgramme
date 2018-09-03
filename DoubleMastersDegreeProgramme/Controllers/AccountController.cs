using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DoubleMastersDegreeProgramme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DoubleMastersDegreeProgramme.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DoubleMastersDegreeProgramme.Accounts
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHostingEnvironment _appEnvironment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    BirthDate = model.BirthDate,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Chair = model.Chair,
                    Department = model.Department,
                    Gender = model.Gender,
                    Group = model.Group,
                    MastersProjectTitle = model.MastersProjectTitle,
                    PhoneNumber = model.PhoneNumber,
                    ScientificSupervisor = model.ScientificSupervisor,
                };

                // Add user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {                   
                    return RedirectToAction("IndexReg", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // check wether the URL belongs to the app
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewData["Message"] = "Login or/and Password are incorrect";
                    ModelState.AddModelError("", "Login or/and Password are incorrect");
                }
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // Remove authentication cookies
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        [Authorize(Roles = "admin, student")]
        public async Task<IActionResult> Edit(string name)
        {
            if (_signInManager.Context.User.Identity.Name == name || _signInManager.Context.User.IsInRole("admin"))
            {
                User user = await _userManager.FindByNameAsync(name);
                if (user == null)
                {
                    return NotFound();
                }

                EditUserViewModel model = new EditUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.Email,
                    BirthDate = user.BirthDate,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    Chair = user.Chair,
                    Department = user.Department,
                    Gender = user.Gender,
                    Group = user.Group,
                    MastersProjectTitle = user.MastersProjectTitle,
                    PhoneNumber = user.PhoneNumber,
                    ScientificSupervisor = user.ScientificSupervisor,
                    Avatar = user.Avatar

                };
                return View(model);
            }

            return View();
        }

        [Authorize(Roles = "admin, student")]
        public async Task<IActionResult> UpdateAvatar(EditUserAvatarViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    byte[] imageData = null;
                    // 
                    using (var binaryReader = new BinaryReader(model.Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Avatar.Length);
                    }

                    user.Avatar = imageData;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Edit", "Account", new { name = user.UserName });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return RedirectToAction("Edit", "Account");
        }

        [Authorize(Roles = "admin, student")]
        public async Task<IActionResult> Update(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.BirthDate = model.BirthDate;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.MiddleName = model.MiddleName;
                    user.Chair = model.Chair;
                    user.Department = model.Department;
                    user.Gender = model.Gender;
                    user.Group = model.Group;
                    user.MastersProjectTitle = model.MastersProjectTitle;
                    user.PhoneNumber = model.PhoneNumber;
                    user.ScientificSupervisor = model.ScientificSupervisor;
                    user.Avatar = model.Avatar ?? user.Avatar;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Edit", "Account", new { name = user.UserName });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return RedirectToAction("Edit", "Account");
        }

        [Authorize(Roles = "admin, student")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            //return View(model);

            return PartialView("_ChangePassword", model);
        }

        [HttpPost]
        [Authorize(Roles = "admin, student")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // 1. Get pass ~Validator and ~Hasher
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    // 2. Validate pass
                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);

                    // 3. Hash pass
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Edit", "Account", new { name = user.UserName });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                }
            }
            return View(model);
        }
    }
}