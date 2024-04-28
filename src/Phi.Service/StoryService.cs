using Phi.Client;
using Phi.Model.Api;

namespace Phi.Service;

public class StoryService : IStoryService
{
    private readonly IDataClient _dataClient;

    public StoryService(IDataClient dataClient)
    {
        _dataClient = dataClient;
    }

    public async Task<IEnumerable<Story>> GetNBestStories(int n)
    {
        var bestStories = new List<Story>();
        foreach (var storyId in await _dataClient.GetBestStoryIds())
        {
            var story = await _dataClient.GetStoryById(storyId);
            
            if (story == null) continue;

            bestStories.Add(new Story() {
                Title = story.Title,
                Uri = story.Url,
                PostedBy = story.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime,
                Score = story.Score,
                CommentCount = story.Descendants
            });
        }

        return bestStories.OrderByDescending(story => story.Score).ToList();
    }
}
