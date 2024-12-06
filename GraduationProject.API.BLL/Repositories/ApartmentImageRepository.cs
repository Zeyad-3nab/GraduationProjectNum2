using GraduationProject.API.BLL.Interfaces;
using GraduationProject.API.DAL.Data.Contexts;
using GraduationProject.API.DAL.Models.ApartmentModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.BLL.Repositories
{
    public class ApartmentImageRepository : IApartmentImageRepository
    {
        private readonly ApplicationDbContext context;

        public ApartmentImageRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(ApartmentImages entity)
        {
            await context.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAllOfIpartmentAsync(int IpartId)
        {
            await context.ApartmentImages.Where(e => e.ApartmentId == IpartId).ExecuteDeleteAsync();
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(ApartmentImages entity)
        {
            context.ApartmentImages.Remove(entity);
            return await context.SaveChangesAsync();
        }



        public async Task<ApartmentImages> GetAsync(int ImageId) => await context.ApartmentImages.FindAsync(ImageId);


        public async Task<IEnumerable<ApartmentImages>> GetAllOfIpartmentAsync(int IpartId) => await context.ApartmentImages.Where(e => e.ApartmentId == IpartId).ToListAsync();
        
    }
}
