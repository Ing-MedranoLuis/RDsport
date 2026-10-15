using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RDsport.ApiResponse;
using RDsport.DTO.Player;
using RDsport.DTO.Teams;
using RDsport.Models;
using RDsport.Services;

namespace RDsport.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        private readonly IPlayersServices _sv;
        private readonly ITeamServices _teamServices;
        private readonly IImageService _isv;
        private readonly IMapper _mp;
        public PlayersController(IPlayersServices sv, IMapper mp, IImageService isv,ITeamServices teamServices)
        {
            _sv = sv;
            _mp = mp;
            _isv = isv;
            _teamServices = teamServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Task<ActionResult<IEnumerable<PlayerDTO>>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ApiResponseClass<PlayerDTO>>>> AllPlayers(int page = 1, int pageSize = 5)
        {
            var data = await _sv.GetAll(page, pageSize);
            var dataMapped = _mp.Map<List<PlayerDTO>>(data);

            if (dataMapped == null)
            {
                var response = ApiResponseClass<object?>.Ok("No data", dataMapped);
                return Ok(response);

            }
            return Ok(ApiResponseClass<object?>.Ok("Success", dataMapped));


        }


        [HttpGet("{Id:int}")]
        [ProducesResponseType(typeof(Task<ActionResult<Player>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Task<ActionResult<Player>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Task<ActionResult<Player>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<Player>>> GetById(int Id)
        {

            try
            {
                if (Id <= 0)
                {
                    return BadRequest(ApiResponseClass<Player>.BadRequest("Please insert an Id greater than 0", new { }));
                }
                var data = await _sv.GetById(Id);
                if (data == null) return NotFound(ApiResponseClass<Player>.NotFound($"There is not records with that Id: {Id}", new { }));
                var mappedData = _mp.Map<PlayerDTO>(data);
                return Ok(ApiResponseClass<PlayerDTO>.Ok($"These is the record with this Id: {Id}", mappedData));

            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerCreateDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerCreateDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<PlayerCreateDTO>>> CreatePlayer([FromForm] PlayerCreateDTO playerCreateDTO)
        {

            try
            {
                if (playerCreateDTO.Name == null)
                {
                    return BadRequest(ApiResponseClass<PlayerCreateDTO>.BadRequest("Please fill the fields out", new { }));
                }
                if (await _sv.DominicanIdExist(playerCreateDTO.DominicanId))
                {
                    return BadRequest(ApiResponseClass<PlayerCreateDTO>.BadRequest($"Inser a different Cedula: {playerCreateDTO.DominicanId} already exist", new { }));
                }
                var validatingTeamId =await _teamServices.GetById(playerCreateDTO.TeamId);
                if (validatingTeamId==null) return NotFound(ApiResponseClass<PlayerCreateDTO>.BadRequest($"Please make sure there is a team with that Id: {playerCreateDTO.TeamId}", new { }));

                //UPLOADING AN IMAGE
                if (playerCreateDTO.Image != null)
                {
                    if (_isv.ValidateImage(playerCreateDTO.Image))
                    {
                        var url = await _isv.UploadImage(playerCreateDTO.Image,"Player");
                        playerCreateDTO.ImageUrl = url;
                    }
                }
                var mappedData = _mp.Map<Player>(playerCreateDTO);
                var result = await _sv.Create(mappedData);
                if (!result) return BadRequest(ApiResponseClass<PlayerCreateDTO>.BadRequest("Something happened posting the team", new { }));

                var response = ApiResponseClass<PlayerCreateDTO>.CreatedAt("Team posted successfully", _mp.Map<PlayerCreateDTO>(mappedData));
                return CreatedAtAction(nameof(GetById), new { id = mappedData.Id }, response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }


        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerUpdateDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerUpdateDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<PlayerUpdateDTO>>> UpdateTeam(int Id, [FromForm] PlayerUpdateDTO playerUpdateDTO)
        {

            try
            {
                if (playerUpdateDTO.Name == null)
                {
                    return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest("Please fill the fields out", new { }));
                }


                var playerToUpdate = await _sv.GetById(Id);
                if (playerToUpdate == null) return BadRequest(ApiResponseClass<TeamCreateDTO>.BadRequest("Theres is not team with the Id", new { }));
                if (playerUpdateDTO.Image != null)
                {
                    if (_isv.ValidateImage(playerUpdateDTO.Image))
                    {
                        var newImage = await _isv.UploadImage(playerUpdateDTO.Image,"Player");
                        if (playerToUpdate.ImageUrl != null) await _isv.DeleteImage(playerToUpdate.ImageUrl,"Player");
                        playerUpdateDTO.ImageUrl = newImage;

                    }
                }

                var mappedData = _mp.Map(playerUpdateDTO, playerToUpdate);
                var result = await _sv.Update(mappedData);
                if (!result) return BadRequest(ApiResponseClass<PlayerUpdateDTO>.BadRequest("Something happened updating the player", new { }));

                var response = ApiResponseClass<PlayerUpdateDTO>.NoContent("Player updated successfully", _mp.Map<PlayerUpdateDTO>(mappedData));
                return Ok(response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }
        [HttpPatch("{Id:int}",Name = "UpdateRandom")]
        
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerPatchDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerPatchDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<PlayerPatchDTO>>> UpdatedRandom(int Id, [FromForm] PlayerPatchDTO playerUpdateDTO)
        {

            try
            {
                if (playerUpdateDTO.Name == null)
                {
                    return BadRequest(ApiResponseClass<PlayerPatchDTO>.BadRequest("Please fill the fields out", new { }));
                }


                var playerToUpdate = await _sv.GetById(Id);
                if (playerToUpdate == null) return BadRequest(ApiResponseClass<PlayerPatchDTO>.BadRequest("Theres is not team with the Id", new { }));
                if (playerUpdateDTO.Image != null)
                {
                    if (_isv.ValidateImage(playerUpdateDTO.Image))
                    {
                        var newImage = await _isv.UploadImage(playerUpdateDTO.Image, "Player");
                        if (playerToUpdate.ImageUrl != null) await _isv.DeleteImage(playerToUpdate.ImageUrl, "Player");
                        playerUpdateDTO.ImageUrl = newImage;

                    }
                }

              
                var result = await _sv.UpdatePatch(playerUpdateDTO,Id);
                var mappedData = _mp.Map(playerUpdateDTO, playerToUpdate);
                if (!result) return BadRequest(ApiResponseClass<PlayerPatchDTO>.BadRequest("Something happened updating the player", new { }));

                var response = ApiResponseClass<PlayerPatchDTO>.NoContent("Player updated successfully", _mp.Map<PlayerPatchDTO>(mappedData));
                return Ok(response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }



        [HttpDelete("{Id:int}")]

        [ProducesResponseType(typeof(Task<ActionResult<PlayerDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Task<ActionResult<PlayerDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseClass<PlayerDTO>>> DeleteTeam(int Id)
        {

            try
            {

                var playerToDelete = await _sv.GetById(Id);
                if (playerToDelete == null)
                {
                    return BadRequest(ApiResponseClass<PlayerDTO>.NotFound("We could not find a team with that Id", new { }));
                }

                if (playerToDelete.ImageUrl != null)
                {
                    await _isv.DeleteImage(playerToDelete.ImageUrl,"Player");
                }
                var result = await _sv.Delete(Id);



                if (!result) return BadRequest(ApiResponseClass<PlayerDTO>.BadRequest("Something happened deleting the team", new { }));

                var response = ApiResponseClass<PlayerDTO>.NoContent("Team deleted successfully", _mp.Map<PlayerDTO>(playerToDelete));
                return Ok(response);


            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);

            }



        }
    }
}
