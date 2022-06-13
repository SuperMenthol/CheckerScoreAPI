using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CheckerScoreAPI.Data.Abstracts
{
    public interface IDataContext
    {
        Player GetPlayerByID(int id);
        Task<ActionResult> GetAllAsync<T>();
        Player GetPlayerByName(string name);
        Task<List<Result>> GetMatchesOfPlayer(int playerId);
        void AddPlayer(Player player);
        void UpdatePlayer(Player player);
        void RemovePlayer(Player player);

        Task<PlayerResultModel> GetPlayerInformation(int playerId);
        Result GetMatchResultByID(int id);
        void AddMatchResult(Result result);
        int GetLastPlayerID();
    }
}
