using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.IdentityModels;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.ApartmentsDTO
{
    public class ApartmentDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        public string? Village { get; set; }



        [Required(ErrorMessage = "Location is required")]
        [DataType(DataType.Url)]
        public string Location { get; set; }



        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }


        [Required(ErrorMessage ="Image is required")]
        public IFormFile Image { get; set; }

        public string BaseImageUrl { get; set; } = null!;

        [Required(ErrorMessage ="Type is required")]
        public ApartmentType Type { get; set; }


        [Required(ErrorMessage = "address lat is required")]
        public double address_Lat { get; set; }


        [Required(ErrorMessage = "address lon is required")]
        public double address_Lon { get; set; }


        [Required(ErrorMessage = "NumberOfRooms is required")]
        public int NumOfRooms { get; set; }

        public double? DistanceByMeters { get; set; }

        [Required(ErrorMessage ="Is Rent is required")]
        public bool IsRent { get; set; }

        [Required(ErrorMessage ="UserId is required")]
        public string UserId { get; set; }

       
      

    }
}
