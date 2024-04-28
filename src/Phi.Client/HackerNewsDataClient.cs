using Phi.Model.Client;
using System.Net.Http.Json;
using System.Text.Json;

namespace Phi.Client;

public class HackerNewsDataClient : IDataClient
{
    private const string GetBestStoriesUri = "https://hacker-news.firebaseio.com/v0/beststories.json";
    private const string GetStoryByIdUriTemplate = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
    private readonly HttpClient _httpClient;

    public HackerNewsDataClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<int>> GetBestStoryIds()
    {
        var storyIds = await _httpClient.GetFromJsonAsync<int[]>(GetBestStoriesUri);
        if (storyIds == null || storyIds.Length == 0)
        {
            return Enumerable.Empty<int>();
        }

        return storyIds;
    }

    public async Task<Story?> GetStoryById(int id)
    {
        return await _httpClient.GetFromJsonAsync<Story>(string.Format(GetStoryByIdUriTemplate, id));
    }
}
