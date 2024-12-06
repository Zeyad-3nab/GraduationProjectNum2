using GraduationProject.API.BLL.Interfaces;
using GraduationProject.API.DAL.Data.Contexts;
using GraduationProject.API.DAL.Models.Sanaie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.BLL.Repositories
{
	public class DepartmentRepository : IDepartmentRepository
	{
		private readonly ApplicationDbContext _context;

		public DepartmentRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<int> AddAsync(Department entity)
		{
			await _context.Departments.AddAsync(entity);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(Department entity)
		{
			_context.Departments.Remove(entity);
			return await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Department>> GetAllAsync()
		{

			return await _context.Departments.ToListAsync();
		}

		public async Task<Department> GetAsync(int id)
		{
			return await _context.Departments.FindAsync(id);
		}

		public async Task<IEnumerable<Department>> Search(string temp)
		{
			var result = await _context.Departments.Where(D => D.Name.Contains(temp)).ToListAsync();
			return result;
		}

		public async Task<int> UpdateAsync(Department entity)
		{
			_context.Departments.Update(entity);
			return await _context.SaveChangesAsync();
		}
	}
}
