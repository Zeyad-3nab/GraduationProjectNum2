using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.PL.DTOs.ApplicationUserDTOs;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.API.PL.DTOs.DepartmentDTOs
{
	public class DepartmentDTO
	{
		public int Id { get; set; }

		[Required(ErrorMessage ="DepartmentName is required")]
		public string Name { get; set; }
	}
}
