using AutoMapper;
using GraduationProject.API.DAL.Data.Contexts;
using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.DAL.Models.Sanaie;
using GraduationProject.API.PL.DTOs;
using GraduationProject.API.PL.DTOs.ApartmentsDTO;
using GraduationProject.API.PL.DTOs.ApplicationUserDTOs;
using GraduationProject.API.PL.DTOs.DepartmentDTOs;
using GraduationProject.API.PL.Errors;
using GraduationProject.API.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace GraduationProject.API.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;
        private readonly ApiResponse response;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration ,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.mapper = mapper;
            response = new ApiResponse();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public ActionResult GetAllUsers()
        {
            var users = userManager.Users.ToList();
            return Ok(users);
        }




        [AllowAnonymous]
        [HttpGet("GetUserById")]
        public async Task<ActionResult> GetUserById( string userId)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    return Ok(user);
                }

                response.statusCode = HttpStatusCode.NotFound;
                response.errors.Add("User with this Id Not Found.");
                response.message = "a bad Request , You have made";
                return NotFound(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }




        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? user = await userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    if (await userManager.CheckPasswordAsync(user, loginDTO.Password))
                    {
                        var claims = new List<Claim>();

                        //claims.Add(new Claim("tokenNo", "75"));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
                        }


                        //SigningCradiential

                        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var SigningCradiential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);


                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddMonths(1),
                            signingCredentials: SigningCradiential
                            );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        response.errors.Add("Password with this email InCorrect");
                        response.message = "a bad Request , You have made";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.statusCode = HttpStatusCode.NotFound;
                    response.errors.Add("User with this email NotFound.");
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                }
            }
            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }



        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(registerDTO.Email);
                if (user is not null)
                {
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.errors.Add("User with this email already exists.");
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                }


                var appUser = mapper.Map<ApplicationUser>(registerDTO);
             
                var result = await userManager.CreateAsync(appUser, registerDTO.Password);    //Create Account
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, $"{registerDTO.Role}");         //Add To Role


                    var claims = new List<Claim>();                                         //Generate Token

                    //claims.Add(new Claim("tokenNo", "75"));
                    claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var roles = await userManager.GetRolesAsync(appUser);
                    foreach (var item in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
                    }


                    //SigningCradiential

                    var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    var SigningCradiential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);


                    var token = new JwtSecurityToken(
                        claims: claims,
                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        expires: DateTime.Now.AddMonths(1),
                        signingCredentials: SigningCradiential
                        );
                    var _token = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    return Ok(_token);
                }
                else
                {
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.errors = new List<string>();

                    foreach (var err in result.Errors) 
                    {
                       response.errors.Add(err.Description);
                    }
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                }

            }
             response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }



        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(registerDTO.Email);

                if (user is not null)
                {
                    var appUser = mapper.Map<ApplicationUser>(registerDTO);


                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok("Updated");
                    }
                    else 
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        response.errors = new List<string>();

                        foreach (var err in result.Errors)
                        {
                            response.errors.Add(err.Description);
                        }
                        response.message = "a bad Request , You have made";
                        return BadRequest(response);
                    }
                   

                }

                    response.statusCode = HttpStatusCode.BadRequest;
                    response.errors.Add("");
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                
               
            }
            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    var result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.errors = new List<string>();

                    foreach (var err in result.Errors)
                    {
                        response.errors.Add(err.Description);
                    }
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                }
                response.statusCode = HttpStatusCode.NotFound;
                response.errors.Add("User with this Id Not Found");
                response.message = "a bad Request , You have made";
                return NotFound(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }


        [HttpPost("SendEmail")]
        public async Task<ActionResult> SendEmail([DataType(DataType.EmailAddress)] string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    Random random = new Random();
                    int Code = random.Next(1000, 9999);
                    var email = new Email()
                    {
                        To = Email,
                        Subject = "Reset Password",
                        Body = $"Resetting Your Password in Mo3tarib App\r\n\r\nOpen the app and click \"Forgot Password?\r\n\r\nEnter your email or username.Code\r\n\r\nCode = {Code}"
                    };

                    EmailSettings.SendEmail(email);
                    return Ok(Code);
                }

                response.statusCode = HttpStatusCode.NotFound;
                response.errors.Add("User with this email Not Found.");
                response.message = "a bad Request , You have made";
                return NotFound(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);

        }

        [AllowAnonymous]
        [HttpPut("ChangePassword")]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(updatePasswordDTO.Email);
                if (user is not null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, updatePasswordDTO.Password);
                        if (result.Succeeded)
                        {
                            return Ok("changed");
                        }
                    }

                    response.statusCode = HttpStatusCode.BadRequest;
                    foreach (var err in result.Errors)
                    {
                        response.errors.Add(err.Description);
                    }
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                }

                response.statusCode = HttpStatusCode.BadRequest;
                response.errors.Add("User with this email already exists.");
                response.message = "a bad Request , You have made";
                return BadRequest(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }


        [HttpGet("UserRoles")]
        public async Task<ActionResult> GetUserRoles([FromBody]string Email)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    return Ok(roles);
                }
                response.statusCode = HttpStatusCode.NotFound;
                response.errors.Add("User with this email Not Found .");
                response.message = "a bad Request , You have made";
                return NotFound(response);
            }
            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }



        [HttpPost("AddToRole")]
        public async Task<IActionResult> AddToRole([FromBody]AddToRoleDTO addToRoleDTO)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.FindByEmailAsync(addToRoleDTO.Email);
                if (user is not null)
                {
                    var result= await userManager.AddToRoleAsync(user, addToRoleDTO.Role);
                    if (result.Succeeded)
                        return Ok();

                    else
                        response.statusCode = HttpStatusCode.BadRequest;
                    foreach (var err in result.Errors)
                    {
                        response.errors.Add(err.Description);
                    }
                    response.message = "a bad Request , You have made";
                    return BadRequest(response);
                }


                response.statusCode = HttpStatusCode.NotFound;
                response.errors.Add("User with this email Not Found .");
                response.message = "a bad Request , You have made";
                return NotFound(response);
            }
            response.statusCode = HttpStatusCode.BadRequest;
            response.errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(response);
        }

    }
}
