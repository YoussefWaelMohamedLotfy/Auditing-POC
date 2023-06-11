using Microsoft.AspNetCore.Mvc;

namespace Auditing_POC.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("GetWeatherForecast endpoint");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("{id}")]
    public IActionResult GetWithException(int id)
    {
        _logger.LogInformation("GetWeatherForecast endpoint with exception for ID {inputID}", id);

        try
        {
            if (id % 2 == 0)
            {
                var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();

                _logger.LogInformation("Returning Data to Client: {result}", result);
                return Ok(result);
            }
            else
                throw new Exception($"Odd number found: {id}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised for id {inputID}", id);
            return BadRequest();
        }
    }
}
