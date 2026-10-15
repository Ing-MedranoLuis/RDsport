using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using RDsport.DTO.Player;
using RDsport.DTO.Teams;
using RDsport.DTO.User;
using RDsport.Models;

namespace RDsport.AutoMapper
{
    public class AutoMapperDTO:Profile
    {
      

        public AutoMapperDTO()
        {
           CreateMap<Team,TeamDTO>().ReverseMap();
            CreateMap<Team, TeamUpdateDTO>().ReverseMap();
            CreateMap<Team, TeamCreateDTO>().ReverseMap();
            CreateMap<Player, PlayerDTO>().ReverseMap();
            CreateMap<Player, PlayerCreateDTO>().ReverseMap();
            CreateMap<Player, PlayerUpdateDTO>().ReverseMap();
            CreateMap<Player, PlayerPatchDTO>().ReverseMap();
            CreateMap<ApplicationUser, LoginRequest>().ReverseMap();
            CreateMap<ApplicationUser, LoginResponseDTO>().ReverseMap();
            CreateMap<ApplicationUser, RegisterRequestDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }


       

    }
}
