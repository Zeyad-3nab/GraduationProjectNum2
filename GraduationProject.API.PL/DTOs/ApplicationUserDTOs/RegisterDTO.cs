﻿using GraduationProject.API.DAL.Data.Migrations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApplicationUserDTOs
{
    public class RegisterDTO
    {
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }


        [MaxLength(60)]
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } 


        [MaxLength(14)]
        [MinLength(14)]
        [Required(ErrorMessage = "NationalId is required")]
        public string NationalId { get; set; }


        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; } 


        [Required(ErrorMessage = "WhatsappNumber is Required")]
        
        public string WhatsappNumber { get; set; }
        public string? WebsiteURL { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        public int? DepartmentId { get; set; } = null;
    }
}
