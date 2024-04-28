using Phi.Model.Api;

namespace Phi.Service;

public interface IStoryService
{
    Task<IEnumerable<Story>> GetNBestStories(int n);
}
