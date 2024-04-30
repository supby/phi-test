using Phi.Client;
using Phi.Model.Api;
using Microsoft.Extensions.Logging;

namespace Phi.Service;

public class StoryService : IStoryService
{
    private readonly IDataClient _dataClient;
    private readonly ICacheService<int, Story> _cacheService;
    private readonly ILogger<StoryService> _logger;

    public StoryService(
        IDataClient dataClient,
        ICacheService<int,Story> cacheService,
        ILogger<StoryService> logger)
    {
        _dataClient = dataClient;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<IEnumerable<Story>> GetNBestStories(int n)
    {
        var bestStories = new List<Story>();

        // NOTE: i did not find any confirmnation in docs that beststories.json endpoint returns ids sorted in descending order
        // but according to tests it is true so my assaumtion here beststories.json returns ids in descending by score order
        // In case it is not true it needs to load all stories to be able to sort them, which would require warm-up cache by firts requests after start.
        // var storyIds = await _dataClient.GetBestStoryIds();
        var storyIds = (await _dataClient.GetBestStoryIds()).Take(n);
        foreach (var storyId in storyIds)
        {   
            var cachedStory = _cacheService.GetByKey(storyId);
            if (cachedStory != null)
            {
                _logger.LogDebug("Story {0} exists in cache, using cached version.", storyId);

                bestStories.Add(cachedStory);
                continue;
            }

            _logger.LogDebug("Story {0} is NOT found in cache, requesting from API.", storyId);

            var story = await _dataClient.GetStoryById(storyId);
            if (story == null) continue;

            // TODO: add AutoMapper
            var newStory = new Story() {
                Title = story.Title,
                Uri = story.Url,
                PostedBy = story.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime,
                Score = story.Score,
                CommentCount = story.Descendants
            };

            
            bestStories.Add(newStory);
            _cacheService.Add(storyId, newStory);
        }

        return bestStories.OrderByDescending(story => story.Score).Take(n).ToList();
    }
}
