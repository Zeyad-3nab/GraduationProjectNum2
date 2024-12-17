using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.BaseModel;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.DAL.Models.Sanaie;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.DAL.Models.Sanaie
{
    public class Department:BaseEntity
    {
        [Required]
        public string Name { get; set; }

        //Location
        public List<ApplicationUser> ApplicationUsers { get; set; } = new();
    }
}



