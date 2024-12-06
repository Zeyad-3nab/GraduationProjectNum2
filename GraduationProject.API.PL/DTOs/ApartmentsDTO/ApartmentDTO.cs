using GraduationProject.API.DAL.Models.ApartmentModels;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApartmentsDTO
{
    public class ApartmentDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Governorate { get; set; }
        public string? City { get; set; }
        public string? Village { get; set; }



        [Required]
        [DataType(DataType.Url)]
        public string Location { get; set; }



        [Required]
        public string Price { get; set; }

        [DataType(DataType.Currency)]
        public string Curency { get; set; }

        public IFormFile Image { get; set; } = null!;
        public string BaseImage { get; set; } = null!;

        [Required]
        public ApartmentType Type { get; set; }


        [Required]
        public double address_Lat { get; set; }


        [Required]
        public double address_Lon { get; set; }

        public double? DistanceByMeters { get; set; }
        public string? UserId { get; set; }
    }
}
