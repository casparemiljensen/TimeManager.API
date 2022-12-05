using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TimeManager.API.Data;
using TimeManager.API.JwtHelpers;
using TimeManager.API.Models;
namespace TimeManager.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(JwtSettings jwtSettings, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public IActionResult CreateRole(string roleName)
        {
            var role = new Role
            {
                Name = roleName,
                NormalizedName = roleName,
                ConcurrencyStamp = DateTime.Now.ToString()
            };
            var response = _roleManager.CreateAsync(role);
            if (response.Result.Succeeded)
                return Ok("Created role");
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(UserRegistration userRegistration)
        {
            var user = new User
            {
                UserName = userRegistration.FirstName + userRegistration.LastName,
                Email = userRegistration.Email,
                PhoneNumber = userRegistration.PhoneNumber,
            };
            var userResult = await _userManager.CreateAsync(user, userRegistration.Password);

            if (!userResult.Succeeded)
                return BadRequest("Failed to create user: " + StringHelper.IdentityError(userResult.Errors));
            var currentUser = _userManager.FindByEmailAsync(userRegistration.Email).Result;

            if(_roleManager.Roles.Any())
            {
                if (_roleManager.Roles.Select(i => i.Name.Equals(userRegistration.Role)).First())
                {
                    await _userManager.AddToRoleAsync(currentUser, userRegistration.Role);
                }
                else
                {
                    CreateRole(userRegistration.Role);
                }
            }
            else
            {
                CreateRole(userRegistration.Role);
                    
            }
            await _userManager.AddToRoleAsync(currentUser, userRegistration.Role);

            if (userResult.Succeeded)
                return Ok("User created");
            else
                return BadRequest(userResult);
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var user = await _userManager.FindByEmailAsync(userLogins.Email);

                if (user != null)
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    var correctPassword = hasher.VerifyHashedPassword(user,user.PasswordHash,userLogins.Password);
                    
                    if(correctPassword.HasFlag(PasswordVerificationResult.Success))
                    
                    {
                        Token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                        {
                            EmailId = user.Email,
                            GuidId = Guid.NewGuid(),
                            UserName = user.UserName,
                            Id = user.Id,
                        }, _jwtSettings);
                    }
                }
                else
                {
                    return BadRequest("Wrong email or password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get List of UserAccounts
        /// </summary>
        /// <returns>List Of UserAccounts</returns>

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public Task<User> GetUser(string email)
        {
            return _userManager.FindByEmailAsync(email);
            //CHanged this from return ok and return type of ActionResult
        }

    }
}


