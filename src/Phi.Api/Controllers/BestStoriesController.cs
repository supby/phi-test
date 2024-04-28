using Microsoft.AspNetCore.Mvc;
using Phi.Model.Api;
using Phi.Service;

namespace Phi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase
{
    private readonly ILogger<BestStoriesController> _logger;
    private readonly IStoryService _storyService;

    public BestStoriesController(
        IStoryService storyService,
        ILogger<BestStoriesController> logger)
    {
        _logger = logger;
        _storyService = storyService;
    }

    [HttpGet("{storiesCount}")]
    public async Task<IEnumerable<Story>> Get(int storiesCount)
    {
        // TODO: validate input
        
        return await _storyService.GetNBestStories(storiesCount);
    }
}
