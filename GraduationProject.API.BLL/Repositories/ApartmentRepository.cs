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
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApplicationDbContext context;

        public ApartmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync() => await context.Apartments.ToListAsync();

        public async Task<Apartment> GetAsync(int id) => await context.Apartments.FindAsync(id);

        //public async Task<IEnumerable<Apartment>> GetAllWithUserAsync(string id) => await context.Apartments.Where(e => e.UserId == id).ToListAsync();
        public async Task<IEnumerable<Apartment>> GetAllWithUserAsync(string id) => await context.Apartments.Where(e => e.UserId == id).ToListAsync();

        public async Task<int> AddAsync(Apartment entity)
        {
            await context.Apartments.AddAsync(entity);
            return await context.SaveChangesAsync();

        }

        public async Task<int> UpdateAsync(Apartment entity)
        {
            context.Apartments.Update(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Apartment entity)
        {
            context.Apartments.Remove(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Apartment>> Search(string temp)
        {
            var result = await context.Apartments.Where(e => e.City.Contains(temp) || e.Village.Contains(temp) || e.Governorate.Contains(temp)).ToListAsync();
            return result;
        }

     
    }
}
