using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleMastersDegreeProgramme.Models;
using DoubleMastersDegreeProgramme.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoubleMastersDegreeProgramme.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        
        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles="admin")]
        public IActionResult Index() => View(_userManager.Users.ToList());

        [Authorize(Roles = "admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
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
                    Avatar = model.Avatar
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
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
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
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
                    user.Avatar = model.Avatar;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Edit", "Users", new { id = user.Id });
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
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            
            return PartialView("_ChangePassword", model);
        }

        [HttpPost]
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
                        return RedirectToAction("Index");
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