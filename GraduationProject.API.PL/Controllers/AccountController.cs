using GraduationProject.API.DAL.Data.Contexts;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.PL.DTOs.ApplicationUserDTOs;
using GraduationProject.API.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
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

        public AccountController(UserManager<ApplicationUser> userManager , IConfiguration configuration , SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
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
        public ActionResult GetUserById(string userId)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
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
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError($"{loginDTO.Email}", "Email not Found");
                }
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                if (registerDTO.DepartmentId is null || registerDTO.DepartmentId == 0)  
                {
                      applicationUser = new ApplicationUser()
                     {
                         FirstName = registerDTO.FirstName,
                         LastName = registerDTO.LastName,
                         UserName = registerDTO.UserName,
                         PhoneNumber = registerDTO.PhoneNumber,
                         WhatsappNumber = registerDTO.WhatsappNumber,
                         NationalId = registerDTO.NationalId,
                         Email = registerDTO.Email,
                         WebsiteURL = registerDTO.WebsiteURL,
                
                     };
                }
                else 
                {
                    applicationUser = new ApplicationUser()
                    {
                        FirstName = registerDTO.FirstName,
                        LastName = registerDTO.LastName,
                        UserName = registerDTO.UserName,
                        PhoneNumber = registerDTO.PhoneNumber,
                        WhatsappNumber = registerDTO.WhatsappNumber,
                        NationalId = registerDTO.NationalId,
                        Email = registerDTO.Email,
                        WebsiteURL = registerDTO.WebsiteURL,
                        DepartmentId = registerDTO.DepartmentId,
                    };
                }

                var result = await userManager.CreateAsync(applicationUser, registerDTO.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, $"{registerDTO.Role}");
                    return Ok(result);
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return BadRequest(ModelState);
        }



        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(registerDTO.Email);

                if (user is not null)
                {
                    user.FirstName = registerDTO.FirstName;
                    user.LastName = registerDTO.LastName;
                    user.UserName = registerDTO.UserName;
                    user.PhoneNumber = registerDTO.PhoneNumber;
                    user.Email = registerDTO.Email;
                    user.NationalId = registerDTO.NationalId;
                    user.WhatsappNumber = registerDTO.WhatsappNumber;
                    user.WebsiteURL = registerDTO.WebsiteURL;


                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok("Updated");
                    }
                    return BadRequest(result);

                }
                return NotFound(registerDTO);


            }
            return BadRequest(ModelState);

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
                    var result=await userManager.DeleteAsync(user);
                    if (result.Succeeded) 
                    {
                        return Ok();
                    }
                    return BadRequest(result);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
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
                        Body= $"Resetting Your Password in Mo3tarib App\r\n\r\nOpen the app and click \"Forgot Password?\r\n\r\nEnter your email or username.Code\r\n\r\nCode = {Code}"
                    };

                    EmailSettings.SendEmail(email);
                    return Ok(Code);
                }
                return NotFound(Email);
            }
            return BadRequest(ModelState);

        }

        [AllowAnonymous]
        [HttpPut("ChangePassword")]
        public async Task<ActionResult> UpdatePassword(string password, string email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, password);
                        if (result.Succeeded)
                        {
                            return Ok("changed");
                        }
                    }
                    return BadRequest("error");
                }
                return NotFound(email);
            }
            return BadRequest(ModelState);
        }


        [HttpGet("UserRoles")]
        public async Task<ActionResult> GetUserRoles(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            return NotFound("Not Found");
        }

    }
}
