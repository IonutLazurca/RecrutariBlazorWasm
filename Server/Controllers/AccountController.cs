using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared;
using RecrutariBlazorWasm.Shared.DTO;
using RecrutariBlazorWasm.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<Recruiter> _signInManager;
        private readonly UserManager<Recruiter> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountController(SignInManager<Recruiter> signInManager, UserManager<Recruiter> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRequestDTO userRequestDTO)
        {
            if (userRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await UserEmailExists(userRequestDTO)) { return BadRequest("User exists"); }

            var user = new Recruiter
            {
                UserName = userRequestDTO.Email,
                Name = userRequestDTO.Email,
                Email = userRequestDTO.Email,
                PhoneNumber = userRequestDTO.Phone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRequestDTO.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO { Errors = errors, IsRegistrationSuccessful = false });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Recruiter);

            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO { Errors = errors, IsRegistrationSuccessful = false });
            }
            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticationDTO authenticationDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(authenticationDTO.Email, authenticationDTO.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(authenticationDTO.Email);
                if (user == null)
                {
                    return Unauthorized(new AuthenticationResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid authentication"
                    });
                }
                var token = await _tokenService.CreateToken(user);

                return Ok(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    UserDTO = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNo = user.PhoneNumber
                    }
                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid authentication"
                });
            }


        }

        private async Task<bool> UserEmailExists(UserRequestDTO userRequestDTO)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == userRequestDTO.Email);
        }



    }
}
