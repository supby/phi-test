using Phi.Model.Client;
using System.Net.Http.Json;
using System.Text.Json;

namespace Phi.Client;

public class HackerNewsClientConfig
{
    public required string GetBestStoriesUri { get; set; }
    public required string GetStoryByIdBaseUri { get; set; }
}

public class HackerNewsDataClient : IDataClient
{
    private const string GetStoryByIdUriTemplate = "{0}.json";

    private readonly HttpClient _httpClient;
    private readonly HackerNewsClientConfig _config;

    public HackerNewsDataClient(
        HttpClient httpClient,
        HackerNewsClientConfig config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<IEnumerable<int>> GetBestStoryIds()
    {
        var storyIds = await _httpClient.GetFromJsonAsync<int[]>(_config.GetBestStoriesUri);
        if (storyIds == null || storyIds.Length == 0)
        {
            return Enumerable.Empty<int>();
        }

        return storyIds;
    }

    public async Task<Story?> GetStoryById(int id)
    {
		var getByIdUri = new Uri(new Uri(_config.GetStoryByIdBaseUri), string.Format(GetStoryByIdUriTemplate, id));
        return await _httpClient.GetFromJsonAsync<Story>(getByIdUri);
    }
}
