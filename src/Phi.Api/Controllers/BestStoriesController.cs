using Microsoft.AspNetCore.Mvc;
using Phi.Model.Api;
using Phi.Service;

namespace Phi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase
{
    private readonly IStoryService _storyService;

    public BestStoriesController(IStoryService storyService)
    {
        _storyService = storyService;
    }

    [HttpGet("{storiesCount}")]
    public async Task<ActionResult<Story>> Get(int storiesCount)
    {
        if (storiesCount <= 0)
        {
            return BadRequest();
        }
        
        return Ok(await _storyService.GetNBestStories(storiesCount));
    }
}
