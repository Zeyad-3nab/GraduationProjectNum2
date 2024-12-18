using AutoMapper;
using GraduationProject.API.BLL.Interfaces;
using GraduationProject.API.DAL.Data.Contexts;
using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.PL.DTOs.ApartmentsDTO;
using GraduationProject.API.PL.Errors;
using GraduationProject.API.PL.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.API.PL.Controllers
{
   
    public class ApartmentController : APIController
    {
        private readonly IApartmentRepository apartmentRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ApartmentController(IApartmentRepository apartmentRepository , IMapper mapper  , UserManager<ApplicationUser> userManager , IWebHostEnvironment webHostEnvironment )
        {
            this.apartmentRepository = apartmentRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        //Get All Apartments
        [HttpGet]
        public async Task<ActionResult> GetAll() => Ok(mapper.Map<IEnumerable<ApartmentDTO>>(await apartmentRepository.GetAllAsync()));

        //----------------------------------------------------------GetById------------------------------------------------------------------------
        //Get By Id
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var result = mapper.Map<ApartmentDTO>(await apartmentRepository.GetAsync(id));    //Auto mapper from apartment to apartmentDTO
            if (result is not null)
                return Ok(result);

            return NotFound();
        }



        //----------------------------------------------------------Search-------------------------------------------------------------------------
        // Search in City and Village
        [HttpGet("Search({SearchInput:alpha})")]
        public async Task<ActionResult> Search([FromRoute] string? SearchInput , double? MinPrice , double? MaxPrice , double? Distance ) => Ok(mapper.Map<IEnumerable<ApartmentDTO>>(await apartmentRepository.Search(SearchInput , MinPrice , MaxPrice , Distance)));




        //----------------------------------------------------------Add-----------------------------------------------------------------------------
        // Add Apartment   [Authorize as a apartment Manager Role]
        [ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
        public async Task<ActionResult> Add([FromForm]ApartmentDTO apartmentDTO) 
        {
            if (ModelState.IsValid) 
            {
                
                if (apartmentDTO.BaseImageUrl is not null) 
                {
                    apartmentDTO.BaseImageUrl = DocumentSettings.Upload(apartmentDTO.Image , "ApartmentImages");   //add image of apartment in wwwroot
                }

                apartmentDTO.DistanceByMeters = CalcDistance.CalculateDistance(apartmentDTO.address_Lat,apartmentDTO.address_Lon);

               var apartment = mapper.Map<Apartment>(apartmentDTO);    //Auto mapper from apartmentDTO to apartment

                apartment.UserId = userManager.GetUserId(User);   //From SignInManager


               var count= await apartmentRepository.AddAsync(apartment);   //result of add in db

                if (count > 0) 
                {
                    return Ok();
                }

				//return BadRequest(apartmentDTO);
			  return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
			}
			//return BadRequest(apartmentDTO);
			return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
		}




        //----------------------------------------------------------Update-----------------------------------------------------------
		[ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPut]
        public async Task<ActionResult> Update([FromForm]ApartmentDTO apartmentDTO) 
        {
            if (ModelState.IsValid) 
            {
                if (apartmentDTO.BaseImageUrl is not null)
                    DocumentSettings.Delete(apartmentDTO.BaseImageUrl, "ApartmentImages");


                if (apartmentDTO.Image is not null)
                    apartmentDTO.BaseImageUrl = DocumentSettings.Upload(apartmentDTO.Image, "ApartmentImages");   // خزن الصوره وهات اسمها


                apartmentDTO.DistanceByMeters=CalcDistance.CalculateDistance(apartmentDTO.address_Lat,apartmentDTO.address_Lon);   //حساب المسافه 

                var apartment = mapper.Map<Apartment>(apartmentDTO);

                var count = await apartmentRepository.UpdateAsync(apartment);   //result of add in db

                if (count > 0)
                {
                    return Ok();
                }

				//return BadRequest(apartmentDTO);
				BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

			}
			return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
		}



        //----------------------------------------------------------Delete-----------------------------------------------------------
        //[Authorize as a Apartment Manager Role]
        [ProducesResponseType(typeof(ApartmentImagesDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpDelete]
        public async Task<ActionResult> Delete(int id) 
        {
            var apartment = await apartmentRepository.GetAsync(id);

            //if (apartment is not null)
            //{
            //    if (apartment.UserId == userManager.GetUserId(User) || await userManager.IsInRoleAsync(User, "Admin"))
            //    {
            //        int count = await apartmentRepository.DeleteAsync(apartment);
            //        if (count > 0)
            //        {
            //            if (apartment.BaseImage is not null)
            //            {
            //                DocumentSettings.Delete(apartment.BaseImage, "ApartmentImages");
            //            }
            //            return Ok();
            //        }
            //        return BadRequest(apartment);
            //    }
            //    else
            //    {
            //        return BadRequest("Unable");
            //    }
            //}
            //return NotFound();


                     if (apartment is not null)
                     {
                         int count = await apartmentRepository.DeleteAsync(apartment);
                         if (count > 0)
                         {
                             if (apartment.BaseImage is not null)
                             {
                                 DocumentSettings.Delete(apartment.BaseImage, "ApartmentImages");
                             }
                             return Ok();
                         }
                     	return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
                     }
                  return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));


        }




    }
}
