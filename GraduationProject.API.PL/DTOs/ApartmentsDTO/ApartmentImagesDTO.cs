using GraduationProject.API.DAL.Models.ApartmentModels;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApartmentsDTO
{
    public class ApartmentImagesDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } 

        [Required(ErrorMessage ="Image is required")]
        public IFormFile image { get; set; }


        [Required(ErrorMessage ="ApartmentId is required")]
        public int ApartmentId { get; set; }
    }
}
