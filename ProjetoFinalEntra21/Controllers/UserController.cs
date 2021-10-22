using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjetoFinalEntra21.Data;
using ProjetoFinalEntra21.Enum;
using ProjetoFinalEntra21.Models;
using ProjetoFinalEntra21.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signManager;
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTConfig _jwtConfig;

        public UserController(ILogger<UserController> logger, SignInManager<User> signManager, UserManager<User> userManager, IOptions<JWTConfig> jWTConfig, RoleManager<IdentityRole> roleManager)
        {
            _jwtConfig = jWTConfig.Value;
            _signManager = signManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;

        }

        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var usersList = _userManager.Users.ToList();
                foreach (var user in usersList)
                {
                    var roles = (await _userManager.GetRolesAsync(user)).ToList();

                    allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, roles, user.Address, user.PhotoURL, user.Id));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetAllRestaurants")]
        public async Task<object> GetAllRestaurants()
        {
            try
            {
                List<RestaurantDTO> allRestaurantDTO = new List<RestaurantDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).ToList();
                    if (role.Any(x => x == "Restaurant"))
                    {
                        allRestaurantDTO.Add(new RestaurantDTO(user.FullName, user.Email, user.UserName, user.Address, user.Description, user.Id, user.PhotoURL));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allRestaurantDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("Restaurant/{email}")]
        public async Task<object> GetRestaurant(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", user));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, " Invalid Parameters", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("GetUserList")]
        public async Task<object> GetUserList()
        {
            
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).ToList();
                    if (role.Any(x => x == "User"))
                    {
                        allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, role, user.Address, user.PhotoURL, user.Id));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));

              
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("GetRoles")]
        public async Task<object> GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles.Select(x => x.Name).ToList();
                if (roles != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", roles));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Roles does not exist", null));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpPost("RegisterRestaurant")]
        public async Task<object> RegisterModelRestaurant([FromBody] AddRegisterModelRestaurant model)
        {
            try
            {
                string role = "Restaurant";
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
                }

                var user = new User()
                {
                    FullName = model.Username,
                    UserName = model.Username,
                    Cnpj = model.CNPJ,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    CloseTime = model.CloseTime,
                    OpenTime = model.OpenTime,
                    PhotoURL = model.PhotoURL,
                    DateCreated = DateTime.UtcNow.ToShortTimeString(),
                    DateModified = DateTime.UtcNow.ToShortTimeString()
                };


                var result = await (_userManager.CreateAsync(user, model.Password));
                if (result.Succeeded)
                {
                    var tempUser = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(tempUser, role);

                    return await Task.FromResult(new ResponseModel(ResponseCode.Ok, "User Has Been Registered", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => (x.Description).ToArray())));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpPost("RegisterClient")]
        public async Task<object> RegisterModelClient([FromBody] AddRegisterModelBindingModel model)
        {
            try
            {
                string role = "Client";
                if (!await _roleManager.RoleExistsAsync(role))
                {

                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
                }



                var user = new User()
                {
                    FullName = $"{model.FirstName} {model.LastName}",
                    UserName = model.FirstName,
                    CPF = model.CPF,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    DateCreated = DateTime.UtcNow.ToShortTimeString(),
                    DateModified = DateTime.UtcNow.ToShortTimeString()
                };


                var result = await (_userManager.CreateAsync(user, model.Password));
                if (result.Succeeded)
                {
                    var tempUser = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(tempUser, role);

                    return await Task.FromResult(new ResponseModel(ResponseCode.Ok, "User Has Been Registered", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => (x.Description).ToArray())));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = await _userManager.FindByEmailAsync(model.Email);
                    if (resultado == null)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, " Invalid Email or Password", null));
                    }

                    if (resultado.Email == model.Email)
                    {
                        var result = await _signManager.PasswordSignInAsync(resultado.UserName, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            var appUser = await _userManager.FindByEmailAsync(model.Email);
                            var roles = (await _userManager.GetRolesAsync(appUser)).ToList();
                            var user = new UserDTO(appUser.FullName, appUser.Email, appUser.UserName, appUser.DateCreated, roles, appUser.Address, appUser.PhotoURL, appUser.Id);
                            user.Token = GenerateToken(appUser, roles);
                            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", user));
                        }
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, " Invalid Email or Password", null));
            }
            catch (Exception ex)

            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpPost("AddRole")]
        public async Task<object> AddRole([FromBody] AddRoleBindingModels model)
        {
            try
            {
                if (model == null || model.Role == "")
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "parameters are missing", null));
                }
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role already exist", null));

                }
                var role = new IdentityRole();
                role.Name = model.Role;
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "something went wrong please try again later", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpPut]
        public async Task<object> UpdateUser([FromBody] UpdateUserBindingModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.PasswordHash = model.PhoneNumber;
                    user.PhotoURL = model.PhotoURL;
                    user.DateModified = DateTime.UtcNow.ToString();


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "UserUpdate", null));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Invalid Parameters", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, ex.Message, null));
            }
        }


        [HttpDelete("{email}")]
        public async Task<object> DeleteUser(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User Delete", null));
                    }

                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Parameters", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }
        private string GenerateToken(User user, List<string> roles)
        {
            var claims = new List<Claim>(){
               new Claim(JwtRegisteredClaimNames.NameId,user.Id),
               new Claim(JwtRegisteredClaimNames.Email,user.Email),
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
           };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}


