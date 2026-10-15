using Microsoft.EntityFrameworkCore;
using RDsport.Data;
using RDsport.Migrations;
using RDsport.Models;

namespace RDsport.Services
{
    public class TeamServices : ITeamServices
    {
        private readonly ApplicationDbContext _db;


        public TeamServices(ApplicationDbContext db)
        {
            _db= db;
        }
        public async Task<bool> Create(Team team)
        {

            try
            {
                await _db.teams.AddAsync(team);
                return await Save();
            }
            catch (Exception ex) { 
            
            throw new NotImplementedException("Somenthing went wrong creating the entity",ex);
            
            
            }
          
        }

        public async Task<bool> Delete(int Id)
        {
            try
            {
                var objectToDelete=await GetById(Id);
                if (objectToDelete == null)return false;
                _db.teams.Remove(objectToDelete);
                return await Save();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Somenthing went wrong deleting the entity", ex);


            }
        }

        public async Task<bool> ExistTeam(string name)
        {
            try
            {
            
                if (String.IsNullOrEmpty(name)) return false;
                var result=await _db.teams.AnyAsync(u => u.TeamName.ToLower() == name.ToLower());
                if(result) return true;
                return false;
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("There is an entity with the same name", ex);


            }
        }

        public async Task<IEnumerable<Team>> GetAll( int page=1, int pageSize = 5)
        {
            try
            { 
              var skip=(page -1) * pageSize;

               return await _db.teams.Skip(skip).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Error retriving all data", ex);


            }
        }

        public async Task<Team> GetById(int Id)
        {
            try
            {    
                return await _db.teams.FirstOrDefaultAsync(u => u.Id == Id);
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
            catch (Exception ex)
            {

                throw new NotImplementedException("Error retriving data by id", ex);


            }
        
        }

        public async Task<bool> Update(Team team)
        {

            try
            {
                _db.teams.Update(team);
                return await Save();
            }
            catch (Exception ex)
            {

                throw new NotImplementedException("Error retriving data by id", ex);


            }
         
        }
    }
}
