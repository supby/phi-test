using Microsoft.AspNetCore.Mvc;

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
    public IEnumerable<WeatherForecast> Get(int storiesCount)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = storiesCount,
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
