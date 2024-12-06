using GraduationProject.API.DAL.Models.ApartmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.BLL.Interfaces
{
    public interface IApartmentRepository:IGenaricRepository<Apartment,int>
    {
        public Task<IEnumerable<Apartment>> GetAllWithUserAsync(string id);

        public Task<IEnumerable<Apartment>> Search(string temp);
    }
}
