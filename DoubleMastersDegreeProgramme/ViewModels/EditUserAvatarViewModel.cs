using DoubleMastersDegreeProgramme.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace DoubleMastersDegreeProgramme.ViewModels
{
    public class EditUserAvatarViewModel
    {
        public string UserName { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
