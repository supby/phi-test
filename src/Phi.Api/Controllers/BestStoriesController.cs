using Microsoft.AspNetCore.Mvc;
using Phi.Model;

namespace Phi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<BestStoriesController> _logger;

    public BestStoriesController(ILogger<BestStoriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{storiesCount}")]
    public IEnumerable<Story> Get(int storiesCount)
    {
        return new [] {
            new Story {
                Title = "test title 1",
                Uri = "https://test-site-1.com",
                PostedBy = "author 123",
                DateTime = DateTime.Now,
                Score = 3,
                CommentCount = 55
            },
            new Story {
                Title = "test title 2",
                Uri = "https://test-site-2.com",
                PostedBy = "author 222",
                DateTime = DateTime.Now,
                Score = 7,
                CommentCount = 33
            },
        };
    }
}
