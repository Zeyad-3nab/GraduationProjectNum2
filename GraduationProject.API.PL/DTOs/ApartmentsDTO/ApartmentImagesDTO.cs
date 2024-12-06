using GraduationProject.API.DAL.Models.ApartmentModels;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApartmentsDTO
{
    public class ApartmentImagesDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="URL is required")]
        public string Url { get; set; }
        public IFormFile image { get; set; }


        [Required]
        public int ApartmentId { get; set; }
    }
}
