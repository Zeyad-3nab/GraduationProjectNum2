using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApplicationUserDTOs
{
    public class LoginDTO
    {
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;


        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
