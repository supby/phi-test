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
        try
        {
            var bestStories = new List<Story>();

            foreach (var storyId in await _dataClient.GetBestStoryIds())
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
        catch (Exception ex)
        {
            _logger.LogError(0, ex, "Error while processing request {0}", ex.Message);
            return Enumerable.Empty<Story>();
        }
    }
}
