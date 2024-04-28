using System.Collections.Generic;
using Phi.Model.Client;

namespace Phi.Client;


public interface IDataClient
{
    Task<IEnumerable<int>> GetBestStoryIds();
    Task<Story?> GetStoryById(int id);
}
