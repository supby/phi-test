using Phi.Client;
using Phi.Model.Api;

namespace Phi.Service;

public class StoryService : IStoryService
{
    private readonly IDataClient _dataClient;
    private readonly ICacheService<int, Story> _cacheService;

    public StoryService(
        IDataClient dataClient,
        ICacheService<int,Story> cacheService)
    {
        _dataClient = dataClient;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<Story>> GetNBestStories(int n)
    {
        var bestStories = new List<Story>();
        foreach (var storyId in await _dataClient.GetBestStoryIds())
        {   
            var cachedStory = _cacheService.GetByKey(storyId);
            if (cachedStory != null)
            {
                bestStories.Add(cachedStory);
                continue;
            }

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
