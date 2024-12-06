using AutoMapper;
using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.Sanaie;
using GraduationProject.API.PL.DTOs.ApartmentsDTO;
using GraduationProject.API.PL.DTOs.DepartmentDTOs;

namespace GraduationProject.API.PL.Mapping
{
    public class Applicationprofile:Profile
    {
        public Applicationprofile()
        {
            CreateMap<Apartment, ApartmentDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<ApartmentImagesDTO, ApartmentImages > ().ReverseMap();
        }
    }
}
