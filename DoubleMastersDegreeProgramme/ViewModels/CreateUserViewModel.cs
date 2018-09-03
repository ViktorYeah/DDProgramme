using DoubleMastersDegreeProgramme.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubleMastersDegreeProgramme.ViewModels
{
    public class CreateUserViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public Genders Gender { get; set; }
        public string Department { get; set; }
        public string Chair { get; set; }
        public string Group { get; set; }
        public string ScientificSupervisor { get; set; }
        public string MastersProjectTitle { get; set; }
        public byte[] Avatar { get; set; }
    }
}
