using GraduationProject.API.DAL.Models.BaseModel;
using GraduationProject.API.DAL.Models.IdentityModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.DAL.Models.ApartmentModels
{
    public class Apartment:BaseEntity
    {
        [Required]
        public string City { get; set; }
        public string? Village { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Location { get; set; }

        [Required]
        public double Price { get; set; }


        [Required]
        public int NumOfRooms { get; set; }

        [Required]
        public string BaseImage { get; set; } 

        [Required]
        public ApartmentType Type { get; set; } 

        [Required]
        public double DistanceByMeters { get; set; }

        [Required]
        public bool IsRent { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<ApartmentImages>? ApartmentImages { get; set; }
    }
}
