using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RDsport.ApiResponse;
using RDsport.DTO.Teams;
using RDsport.Models;
using RDsport.Services;

namespace RDsport.Controllers
{
    [Route("api/Team")]
    [ApiController]
    [Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamServices _sv;
        private readonly IImageService _isv;
        private readonly IMapper _mp;
        public TeamsController(ITeamServices sv, IMapper mp, IImageService isv)
        {
            _sv = sv;
            _mp = mp;
            _isv = isv;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Task<ActionResult<IEnumerable<TeamDTO>>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ApiResponseClass<TeamDTO>>>> AllTeams(int page = 1, int pageSize = 5)
        {
            var data = await _sv.GetAll(page, pageSize);
            var dataMapped = _mp.Map<List<TeamDTO>>(data);

            if (dataMapped == null)
            {
                var response = ApiResponseClass<object?>.Ok("No data", dataMapped);
                return Ok(response);

            }
            return Ok(ApiResponseClass<object?>.Ok("There is not data to retrive", dataMapped));


        }


        [HttpGet("{Id:int}")]
        [ProducesResponseType(typeof(Task<ActionResult<TeamDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Task<ActionResult<TeamDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Task<ActionResult<TeamDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<TeamDTO>>> GetById(int Id)
        {

            try
            {
                if (Id <= 0)
                {
                    return BadRequest(ApiResponseClass<TeamDTO>.BadRequest("Please insert an Id greater than 0", new { }));
                }
                var data = await _sv.GetById(Id);
                if (data == null) return NotFound(ApiResponseClass<TeamDTO>.NotFound($"There is not records with that Id: {Id}", new { }));
                var mappedData = _mp.Map<TeamDTO>(data);
                return Ok(ApiResponseClass<TeamDTO>.Ok($"These is the record with this Id: {Id}", mappedData));

            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Task<ActionResult<TeamCreateDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<TeamCreateDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<TeamCreateDTO>>> CreateTeam([FromForm] TeamCreateDTO teamCreateDTO)
        {

            try
            {
                if (teamCreateDTO.TeamName == null)
                {
                    return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest("Please fill the fields out", new { }));
                }
                if (await _sv.ExistTeam(teamCreateDTO.TeamName))
                {
                    return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest($"Chose a different name: {teamCreateDTO.TeamName} already exist", new { }));
                }

                //UPLOADING AN IMAGE
                if (teamCreateDTO.Image != null)
                {
                    if (_isv.ValidateImage(teamCreateDTO.Image))
                    {
                        var url = await _isv.UploadImage(teamCreateDTO.Image,"Team");
                        teamCreateDTO.ImageUrl = url;
                    }
                }
                var mappedData = _mp.Map<Team>(teamCreateDTO);
                var result = await _sv.Create(mappedData);
                if (!result) return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest("Something happened posting the team", new { }));

                var response = ApiResponseClass<TeamCreateDTO>.CreatedAt("Team posted successfully", _mp.Map<TeamCreateDTO>(mappedData));
                return CreatedAtAction(nameof(GetById), new { id = mappedData.Id }, response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }


        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Task<ActionResult<TeamUpdateDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<TeamUpdateDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<TeamUpdateDTO>>> UpdateTeam(int Id, [FromForm] TeamUpdateDTO teamUpdateDTO)
        {

            try
            {
                if (teamUpdateDTO.TeamName == null)
                {
                    return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest("Please fill the fields out", new { }));
                }
               

                var teamToUpdate = await _sv.GetById(Id);
                if (teamToUpdate == null) return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest("Theres is not team with the Id", new { }));
                if (teamUpdateDTO.Image != null)
                {
                    if (_isv.ValidateImage(teamUpdateDTO.Image))
                    {
                        var newImage = await _isv.UploadImage(teamUpdateDTO.Image,"Team");
                        if (teamToUpdate.ImageUrl != null) await _isv.DeleteImage(teamToUpdate.ImageUrl,"Team");
                        teamUpdateDTO.ImageUrl = newImage;

                    }
                }

                var mappedData = _mp.Map(teamUpdateDTO, teamToUpdate);
                var result = await _sv.Update(mappedData);
                if (!result) return BadRequest(ApiResponseClass<TeamUpdateDTO>.BadRequest("Something happened posting the team", new { }));

                var response = ApiResponseClass<TeamUpdateDTO>.NoContent("Team updated successfully", _mp.Map<TeamUpdateDTO>(mappedData));
                return Ok(response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }


        [HttpDelete("{Id:int}")]

        [ProducesResponseType(typeof(Task<ActionResult<TeamDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<TeamDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<TeamDTO>>> DeleteTeam(int Id)
        {

            try
            {

                var teamToDelete = await _sv.GetById(Id);
                if (teamToDelete == null)
                {
                    return BadRequest(ApiResponseClass<TeamDTO>.NotFound("We could not find a team with that Id", new { }));
                }

                if (teamToDelete.ImageUrl != null)
                {
                    await _isv.DeleteImage(teamToDelete.ImageUrl, "Team");
                }
                var result = await _sv.Delete(Id);



                if (!result) return BadRequest(ApiResponseClass<TeamDTO>.BadRequest("Something happened deleting the team", new { }));

                var response = ApiResponseClass<TeamDTO>.NoContent("Team deleted successfully", _mp.Map<TeamDTO>(teamToDelete));
                return Ok(response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }
    }
}
