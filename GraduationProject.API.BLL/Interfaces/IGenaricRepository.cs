using GraduationProject.API.DAL.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.BLL.Interfaces
{
    public interface IGenaricRepository<T,K> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(K id);
        Task<K> AddAsync(T entity);
        Task<K> UpdateAsync(T entity);
        Task<K> DeleteAsync(T entity);
    }
}
