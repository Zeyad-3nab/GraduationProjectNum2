using AutoMapper;
using GraduationProject.API.BLL.Interfaces;
using GraduationProject.API.BLL.Repositories;
using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.PL.DTOs.ApartmentsDTO;
using GraduationProject.API.PL.DTOs.DepartmentDTOs;
using GraduationProject.API.PL.Errors;
using GraduationProject.API.PL.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace GraduationProject.API.PL.Controllers
{
    public class ApartmentImageController : APIController
    {
        private readonly IApartmentImageRepository apartmentImageRepository;
        private readonly IMapper mapper;
        private readonly IHostEnvironment env;
        private readonly ApartmentRepository apartmentRepository;

        public ApartmentImageController(IApartmentImageRepository apartmentImageRepository , IMapper mapper , IHostEnvironment env , ApartmentRepository apartmentRepository)
        {
            this.apartmentImageRepository = apartmentImageRepository;
            this.mapper = mapper;
            this.env = env;
            this.apartmentRepository = apartmentRepository;
        }



		[ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("GetAllOfApartment/{ApartmentId:int}")]
        public async Task<ActionResult> GetAllImageOfApartment([FromRoute]int ApartmentId) 
        {

            var Images = mapper.Map<IEnumerable<ApartmentImagesDTO>>(await apartmentImageRepository.GetAllOfIpartmentAsync(ApartmentId));
            if (Images is not null)
                return Ok(Images);

            return BadRequest("Empty");
        }





		[ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
		[HttpDelete("{ImageId:int}")]
        public async Task<ActionResult> Delete([FromRoute] int ImageId) 
        {
           var Image= await apartmentImageRepository.GetAsync(ImageId);
            if (Image is not null) 
            {
                int count =await apartmentImageRepository.DeleteAsync(Image);
                if (count > 0)
                {
                    DocumentSettings.Delete(Image.Name, "ApartmentImages");
                    return Ok();
                }
                

				return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
			}
			return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
		}




		[ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
        public async Task<ActionResult> Add(ApartmentImagesDTO apartmentImages) 
        {
            if (ModelState.IsValid) 
            {
                var apartment=await apartmentRepository.GetAsync(apartmentImages.ApartmentId);
                if(apartment is not null) 
                {
                    if (apartmentImages.image is not null)
                    {
                        apartmentImages.Name = DocumentSettings.Upload(apartmentImages.image, "ApartmentImages");
                    }

                    var Images = mapper.Map<ApartmentImages>(apartmentImages);
                    var count = await apartmentImageRepository.AddAsync(Images);
                    if (count > 0)
                    {
                        return Ok(Images);
                    }
					return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
				}
				return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

			}
			return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
		}




		[ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpDelete("DeleteAll/({ApartmentId:int})")]
        public async Task<IActionResult> DeleteAllOfApartment([FromRoute]int ApartmentId) 
        {
            var apartment = await apartmentRepository.GetAsync(ApartmentId);
            if (apartment is not null)
            {
                var images = await apartmentImageRepository.GetAllOfIpartmentAsync(ApartmentId);
                var count = await apartmentImageRepository.DeleteAllOfIpartmentAsync(ApartmentId);
                if (count > 0)
                {
                    foreach (var image in images)
                    {
                        DocumentSettings.Delete(image.Name, "ApartmentImages");
                    }
                    return Ok();
                }
				return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
			}
			return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

		}


		[ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("{ImageId:int}")]
        public async Task<ActionResult> Get(int ImageId)
        {
            var image =await apartmentImageRepository.GetAsync(ImageId);
            if(image is not null) 
            {
                var file = env.ContentRootFileProvider.GetFileInfo($"ApartmentImages/{image.Name}")?.PhysicalPath;
                if (file is not null)
                    return Ok(file);

				return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
			}
			return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
		}


    }
}