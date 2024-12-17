using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.DAL.Models.ApartmentModels
{
    public class ApartmentImages
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
    }
}
