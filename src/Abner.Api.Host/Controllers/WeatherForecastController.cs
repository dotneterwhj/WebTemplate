using Abner.Domain.BlogAggregate;
using Abner.Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Abner.Api.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IRepository<Blog> _repository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository<Blog> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var blogs = await _repository.GetListAsync(cancellationToken: cancellationToken);
        return Ok(blogs);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        var blog = await _repository.InsertAsync(Blog.Create("test", "test desc"), cancellationToken);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Ok(blog);
    }
}
