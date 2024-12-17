using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.Sanaie;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.DAL.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        public string? Photo { get; set; }

        //unique
        public string WhatsappNumber { get; set; }
        //unique
        public string? WebsiteURL { get; set; }



        [Required]
        [MaxLength(14)]
        [MinLength(14)]
        //unique
        
        public string NationalId { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public List<Apartment> Apartments { get; set; } = new();
    }
}