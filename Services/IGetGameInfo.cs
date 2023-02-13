using BoardGame.Models;
using System.Threading.Tasks;

namespace BoardGame.Services
{
    public interface IGetGameInfo
    {
        public Task<GameInfo> GetCollection(string objectId);
    }
}