using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoubleMastersDegreeProgramme.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Genders Gender { get; set; }
        public string Department { get; set; }
        public string Chair { get; set; }
        public string Group { get; set; }
        public string ScientificSupervisor { get; set; }
        public string MastersProjectTitle { get; set; }
        public byte[] Avatar { get; set; }
    }
}
