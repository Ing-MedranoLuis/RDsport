using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RDsport.Data;
using RDsport.DTO.Player;
using RDsport.Migrations;
using RDsport.Models;
using System.Numerics;

namespace RDsport.Services
{
    public class PlayersServices : IPlayersServices
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mp;


        public PlayersServices(ApplicationDbContext db, IMapper mp)
        {
            _db = db;
            _mp = mp;
        }
        public async Task<bool> Create(Player player)
        {

            try
            {
                await _db.players.AddAsync(player);
                return await Save();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Somenthing went wrong creating the entity", ex);


            }

        }

        public async Task<bool> Delete(int Id)
        {
            try
            {
                var objectToDelete = await GetById(Id);
                if (objectToDelete == null) return false;
                _db.players.Remove(objectToDelete);
                return await Save();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Somenthing went wrong deleting the entity", ex);


            }
        }

        public async Task<bool> DominicanIdExist(string dominicanId)
        {
            try
            {

                if (String.IsNullOrEmpty(dominicanId)) return false;
                var result = await _db.players.AnyAsync(u => u.DominicanId.ToLower() == dominicanId.ToLower());
                if (result) return true;
                return false;
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("There is an entity with the same name", ex);


            }
        }

        public async Task<IEnumerable<Player>> GetAll(int page = 1, int pageSize = 5)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                return await _db.players.Skip(skip).Take(pageSize).Include(u => u.Team).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Error retriving all data", ex);


            }
        }

        public async Task<Player> GetById(int Id)
        {
            try
            {

                return await _db.players.FirstOrDefaultAsync(u => u.Id == Id);

            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Error retriving data by id", ex);


            }
        }

        public async Task<bool> Save()
        {

            try
            {
                return await _db.SaveChangesAsync() > 0 ? true : false;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Error principal:");
                Console.WriteLine(ex.Message);

                Console.WriteLine("Error interno:");
                Console.WriteLine(ex.InnerException?.Message);

                throw;
            }

        }

        public async Task<bool> Update(Player player)
        {

            try
            {

                _db.players.Update(player);


                return await Save();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);


            }

        }

        public async Task<bool> UpdatePatch(PlayerPatchDTO player, int id)
        {
            try
            {
                var jugador = await GetById(id);


                if (string.IsNullOrEmpty(player.Name))
                    player.Name = jugador.Name;

                if (string.IsNullOrEmpty(player.ImageUrl))
                    player.ImageUrl = jugador.ImageUrl;

                if (string.IsNullOrEmpty(player.DateOfBirth.ToString()))
                    player.DateOfBirth = jugador.DateOfBirth;

                if (player.TeamId > 0)
                {
                    var team = await _db.teams.FirstOrDefaultAsync(t => t.Id == id);
                    if (team != null)
                    {
                        player.TeamId = team.Id;
                    }
                    else
                    {
                        player.TeamId = jugador.TeamId;

                    }

                }
                else
                {
                    player.TeamId = jugador.TeamId;
                }


                if (string.IsNullOrEmpty(player.DominicanId))
                    player.DominicanId = jugador.DominicanId;



                var map = _mp.Map(player, jugador);
                _db.players.Update(map);


                return await Save();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException(ex.Message);


            }
        }
    }
}
