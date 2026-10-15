using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RDsport.ApiResponse;
using RDsport.DTO.Player;
using RDsport.DTO.User;
using RDsport.Models;
using RDsport.Services;
using RDsport.Services.Auth;

namespace RDsport.Controllers
{
    [Route("api/AuhtController")]
    [ApiController]

    public class AuthController : ControllerBase
    {

        private readonly IAuthServices _sv;
        //private readonly ITeamServices _teamServices;
        //private readonly IImageService _isv;
        private readonly IMapper _mp;
        public AuthController(IAuthServices sv, IMapper mp)
        {
            _sv = sv;
            _mp = mp;

        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(Task<ActionResult<UserDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseClass<UserDTO>>> Register(RegisterRequestDTO registerRequestDTO)
        {
            var user = await _sv.Register(registerRequestDTO);
            if (user == null)
            {


                return BadRequest(ApiResponseClass<UserDTO>.BadRequest("Please fill out all the fields", new { }));
            }
            var response= ApiResponseClass<UserDTO>.BadRequest("User registred successfully", user);
            return Ok(response);


        }


        [HttpPost("Login")]
        [ProducesResponseType(typeof(Task<ActionResult<LoginResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Task<ActionResult<LoginResponseDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Task<ActionResult<LoginResponseDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<LoginResponseDTO>>> Login(LoginRequestDTO loginRequestDTO)
        {

            try
            {
              
                var response = await _sv.Login(loginRequestDTO);
                if (response == null) return BadRequest(ApiResponseClass<LoginResponseDTO>.BadRequest("Passsword or email incorrect", new { }));

                var apiResponse =   ApiResponseClass<LoginResponseDTO>.Ok("Login successfull", response);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }




    }
}
