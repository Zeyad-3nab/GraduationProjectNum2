using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.DAL.Models.ApartmentModels
{
    public class ApartmentImages
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        //public static ApartmentImages create(string url)
        //{
        //    var x =
        //}

    }
}
