using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApplicationUserDTOs
{
    public class LoginDTO
    {
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; } = null!;


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
