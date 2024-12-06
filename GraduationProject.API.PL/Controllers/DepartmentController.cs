using AutoMapper;
using GraduationProject.API.BLL.Interfaces;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.DAL.Models.Sanaie;
using GraduationProject.API.PL.DTOs.DepartmentDTOs;
using GraduationProject.API.PL.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.API.PL.Controllers;

public class DepartmentController : APIController
{
	private readonly IDepartmentRepository _repository;
	private readonly IMapper _mapper;
	private readonly UserManager<ApplicationUser> _userManager;

	public DepartmentController(IDepartmentRepository repository, IMapper mapper, UserManager<ApplicationUser> userManager)
	{
		_repository = repository;
		_mapper = mapper;
		_userManager = userManager;
	}

	[HttpGet]
	public async Task<ActionResult> GetAll()
	{
		var result = _mapper.Map<IEnumerable<DepartmentDTO>>(await _repository.GetAllAsync());
		return Ok(result);
	}

	[ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
	//اللي راجع Respone بتكزن تحسين لشكل ال ProducesResponseType ال 2 حاجات بتاعت ال
	[HttpGet("{id:int}")]
	public async Task<ActionResult> GetById(int id)
	{
		var department = await _repository.GetAsync(id);
		if (department == null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

		var result = _mapper.Map<DepartmentDTO>(department);
		return Ok(result);
	}

	[HttpGet("search/{searchInput}")]
	public async Task<ActionResult> Search(string searchInput)
	{
		var result = _mapper.Map<IEnumerable<DepartmentDTO>>(await _repository.Search(searchInput));
		return Ok(result);
	}


	[ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<ActionResult> Add([FromBody] DepartmentDTO departmentDto)
	{
		if (!ModelState.IsValid) return /*BadRequest(ModelState);*/  BadRequest(new ApiErrorResponse(404));

		var department = _mapper.Map<Department>(departmentDto);
		var count = await _repository.AddAsync(department);

		if (count > 0) return Ok(departmentDto);

		//return BadRequest("Failed to add department.");
		return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
	}

	[ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
	[HttpPut]
	public async Task<ActionResult> Update([FromBody] DepartmentDTO departmentDto)
	{
		if (!ModelState.IsValid)/* return BadRequest(ModelState);*/ return BadRequest(new ApiErrorResponse(404));

		var department = _mapper.Map<Department>(departmentDto);
		var count = await _repository.UpdateAsync(department);

		if (count > 0) return Ok(departmentDto);

		//return BadRequest("Failed to update department.");
		return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
	}

	[ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
	[HttpDelete("{id:int}")]
	public async Task<ActionResult> Delete(int id)
	{
		var department = await _repository.GetAsync(id);
		if (department == null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

		//var Admin = await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "admin");
		//if (!Admin)
		//{
		//	return Forbid("You do not have permission to delete this department.");
		//}
		var count = await _repository.DeleteAsync(department);
		if (count > 0)
		{
			return Ok("Department deleted successfully.");
		}

		//return BadRequest("Failed to delete department.");
		return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
	}

}
