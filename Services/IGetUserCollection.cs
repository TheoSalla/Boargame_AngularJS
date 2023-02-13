using BoardGamePickerWithAngularJS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardGamePickerWithAngularJS.Services
{
    public interface IGetUserCollection
    {
        Task<List<Game>> GetCollection(string userName);
    }
}