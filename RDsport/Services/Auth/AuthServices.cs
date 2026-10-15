using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RDsport.Data;
using RDsport.DTO.User;
using RDsport.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RDsport.Services.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;
        private readonly IMapper _mp;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;
        public AuthServices(UserManager<ApplicationUser> user, RoleManager<IdentityRole> role, IMapper mp, IConfiguration config, ApplicationDbContext db)
        {
            _user = user;
            _role = role;
            _mp = mp;
            _config = config;
            _db = db;
        }

        public bool EmailExist(string email)
        {
            if (_db.users.Any(e => e.Email.ToLower() == email.ToLower())) return true; return false;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var key = Encoding.ASCII.GetBytes(_config.GetSection("JwtKey")["SecureKeyToken"]);
            var roles = await _user.GetRolesAsync(user);
            var tokenDescritor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                  new Claim(ClaimTypes.Name,user.FirstName),
                      new Claim(ClaimTypes.Email,user.Email),
                       new Claim(ClaimTypes.Role, roles.FirstOrDefault()),



                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescritor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _db.users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
            if (user == null)
            {
                return null;
            }
            var success = await _user.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!success)
            {
                throw null;
            }
            var roles = await _user.GetRolesAsync(user);
            var token = await GenerateToken(user);
            var response = new LoginResponseDTO()
            {
                Token = token,
                userDTO = _mp.Map<UserDTO>(user)
            };
            user.Role = roles.FirstOrDefault() ?? "Customer";
            return response;
        }

        public async Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            if (EmailExist(registerRequestDTO.Email))
            {
                return null;
            }

            var user = new ApplicationUser()
            {
                FirstName = registerRequestDTO.FirstName,
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.Email,
                NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                EmailConfirmed = true,
                Role = registerRequestDTO.Role

            };
            var success = await _user.CreateAsync(user, registerRequestDTO.Password);
            if (!success.Succeeded) throw new NotImplementedException("Something happened creating the user");

            var role = String.IsNullOrEmpty(registerRequestDTO.Role) ? "Customer" : registerRequestDTO.Role;

            if (!await _role.RoleExistsAsync(role))
            {
                await _role.CreateAsync(new IdentityRole(role));
            }

            await _user.AddToRoleAsync(user, role);




            await _db.SaveChangesAsync();
            var userDTO = _mp.Map<UserDTO>(user);
            userDTO.Role = role;
            return userDTO;



        }
    }
}
