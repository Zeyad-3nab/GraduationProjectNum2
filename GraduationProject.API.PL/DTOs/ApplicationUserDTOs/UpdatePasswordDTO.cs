using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApplicationUserDTOs
{
    public class UpdatePasswordDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
