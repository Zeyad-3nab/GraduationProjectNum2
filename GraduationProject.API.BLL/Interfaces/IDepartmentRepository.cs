using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.Sanaie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.BLL.Interfaces
{
	public interface IDepartmentRepository : IGenaricRepository<Department,int>
	{
		public Task<IEnumerable<Department>> Search(string temp); 
	}
}
