using GraduationProject.API.DAL.Models.ApartmentModels;

namespace GraduationProject.API.BLL.Interfaces
{
    public interface IApartmentImageRepository
    {
        Task<IEnumerable<ApartmentImages>> GetAllOfIpartmentAsync(int IpartId);
        Task<int> AddAsync(ApartmentImages entity);
        Task<ApartmentImages> GetAsync(int ImageId);
        Task<int> DeleteAsync(ApartmentImages entity);
        Task<int> DeleteAllOfIpartmentAsync(int IpartId);
    }
}
