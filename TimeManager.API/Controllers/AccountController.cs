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
        public IActionResult GetRoles(string userName)
        {
            if(userName != null)
            {
                var user = _userManager.FindByNameAsync(userName).Result;

                var response = _userManager.GetRolesAsync(_userManager.FindByNameAsync(userName).Result);
                if (response.Result.Count > 0)
                {
                    string s = "";
                    foreach (var i in response.Result)
                    {
                        s += i;
                    }
                    return Ok("Found roles!: " + s);
                }
                    
                else
                {
                    return BadRequest(response);
                }
            }
            return BadRequest("No input params");
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

                var userRolesResponse = await _userManager.GetRolesAsync(_userManager.FindByIdAsync(user.Id).Result);
                List<string> userRoles = userRolesResponse.ToList();

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
                            Roles = userRoles,
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


        // Test method for roles

        [HttpGet]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Manager")]
        public Task<User> GetUser(string email)
        {
            var user = _userManager.FindByEmailAsync(email);

            foreach(var i in _roleManager.Roles)
            {
                Console.WriteLine(i);
            }
            return user;
            //CHanged this from return ok and return type of ActionResult
        }

    }
}


