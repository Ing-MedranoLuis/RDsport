using RDsport.DTO.User;
using RDsport.Models;

namespace RDsport.Services.Auth
{
    public interface IAuthServices
    {
        bool EmailExist(string email);
        Task<string> GenerateToken(ApplicationUser user);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
