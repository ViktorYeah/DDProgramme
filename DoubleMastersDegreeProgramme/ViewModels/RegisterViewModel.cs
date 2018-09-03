using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DoubleMastersDegreeProgramme.Models;

namespace DoubleMastersDegreeProgramme.ViewModels
{
    public class RegisterViewModel 
    {
        [Required]
        [Display(Name = "Firsd Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Birthday")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Genders Gender { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Chair")]
        public string Chair { get; set; }

        [Required]
        [Display(Name = "Group")]
        public string Group { get; set; }

        [Required]
        [Display(Name = "Scientific Supervisor")]
        public string ScientificSupervisor { get; set; }

        [Required]
        [Display(Name = "Title of Master's Project")]
        public string MastersProjectTitle { get; set; }

        public byte[] Avatar { get; set; }
    }
}
