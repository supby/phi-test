using Microsoft.AspNetCore.Mvc;
using Phi.Model.Api;
using Phi.Service;

namespace Phi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase
{
    private readonly IStoryService _storyService;
    private readonly ILogger<BestStoriesController> _logger;

    public BestStoriesController(
        IStoryService storyService,
        ILogger<BestStoriesController> logger)
    {
        _storyService = storyService;
        _logger = logger;
    }

    [HttpGet("{storiesCount}")]
    public async Task<ActionResult<Story>> Get(int storiesCount)
    {
        if (storiesCount <= 0)
        {
            return BadRequest();
        }

        try
        {
            return Ok(await _storyService.GetNBestStories(storiesCount));
        }
        catch (Exception ex)
        {
            _logger.LogError(0, ex, "Error while processing request {0}", ex.Message);
            return StatusCode(500, "Error while processing request"); 
        }
    }
}
