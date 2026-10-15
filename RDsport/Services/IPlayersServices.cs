using RDsport.DTO.Player;
using RDsport.Models;

namespace RDsport.Services
{
    public interface IPlayersServices
    {
        Task<IEnumerable<Player>> GetAll(int pageSize,int page);
        Task<Player> GetById(int Id);
        Task<bool> DominicanIdExist(string dominicanId);


        Task<bool> Save();
        Task<bool> Create(Player player);
        Task<bool> UpdatePatch(PlayerPatchDTO player, int id);
        Task<bool> Update(Player player);
        Task<bool> Delete(int Id);


    }
}
