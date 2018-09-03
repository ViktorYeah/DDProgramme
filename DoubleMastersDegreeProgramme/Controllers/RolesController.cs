using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleMastersDegreeProgramme.Models;
using DoubleMastersDegreeProgramme.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoubleMastersDegreeProgramme.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index() => View(_roleManager.Roles.ToList());

        [Authorize(Roles = "admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
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
            return View(name);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult UserList() => View(_userManager.Users.ToList());

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string userId)
        {
            // get user
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // get list of user roles
                var userRoles = await _userManager.GetRolesAsync(user);
                // get list of all roles
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // get user
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // get list of user roles
                var userRoles = await _userManager.GetRolesAsync(user);
                // get list of all roles
                var allRoles = _roleManager.Roles.ToList();
                // get list of all added roles
                var addedRoles = roles.Except(userRoles);
                // get list of all removed roles
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}