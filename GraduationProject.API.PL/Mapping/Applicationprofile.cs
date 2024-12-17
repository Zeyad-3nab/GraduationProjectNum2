using AutoMapper;
using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.Sanaie;
using GraduationProject.API.PL.DTOs.ApartmentsDTO;
using GraduationProject.API.PL.DTOs.DepartmentDTOs;

namespace GraduationProject.API.PL.Mapping
{
    public class Applicationprofile:Profile
    {
        public Applicationprofile(IConfiguration configuration)
        {
            CreateMap<Apartment, ApartmentDTO>()
                .ForMember(p => p.BaseImageUrl, options => options.MapFrom(s => $"{configuration["BaseURL"]}ApartmentImages/{s.BaseImage}"));


            CreateMap<ApartmentDTO, Apartment>()
                .ForMember(p => p.BaseImage, options => options.MapFrom(s => s.BaseImageUrl));



            CreateMap<Department, DepartmentDTO>().ReverseMap();

            CreateMap<ApartmentImages, ApartmentImagesDTO>()
                .ForMember(p => p.Name, options => options.MapFrom(s => $"{configuration["BaseURL"]}ApartmentImages/{s.Name}"));


            CreateMap<ApartmentImagesDTO, ApartmentImages>()
               .ForMember(p => p.Name, options => options.MapFrom(s => s.Name));
        }
    }
}
