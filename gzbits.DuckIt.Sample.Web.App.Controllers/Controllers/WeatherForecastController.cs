using gzbits.DuckIt.Sample.Web.App.Controllers.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace gzbits.DuckIt.Sample.Web.App.Controllers.Controllers
{
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

        [HttpGet("/imperial",Name = "GetWeatherForecastImperial")]
        [ProducesResponseType(typeof(IWeatherForecastImperial), 200)]
        public WeatherForecast GetImperial()
        {
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        }

        [HttpGet("/full", Name = "GetWeatherForecastFull")]
        public WeatherForecast GetFull()
        {
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        }
    }
}