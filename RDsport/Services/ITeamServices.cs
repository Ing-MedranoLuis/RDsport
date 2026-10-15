using RDsport.Models;

namespace RDsport.Services
{
    public interface ITeamServices
    {
        Task<IEnumerable<Team>> GetAll(int pageSize,int page);
        Task<Team> GetById(int Id);
        Task<bool> ExistTeam(string name);


        Task<bool> Save();
        Task<bool> Create(Team team);
        Task<bool> Update(Team team);
        Task<bool> Delete(int Id);


    }
}
