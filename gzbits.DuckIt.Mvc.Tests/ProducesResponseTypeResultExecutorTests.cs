using gzbits.DuckIt.Sample.Web.App.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;

namespace gzbits.DuckIt.Mvc.Tests
{
    [TestClass]
    public class ProducesResponseTypeResultExecutorTests
    {
        private readonly HttpClient weatherForecastClient;

        public ProducesResponseTypeResultExecutorTests()
        {
            weatherForecastClient = new WebApplicationFactory<Program>().CreateClient();
        }

        [TestMethod]
        public void ImperialWeatherForecast_ReturnsSummaryAndTemperatureF()
        {
            var expectedPropertyNames = new HashSet<string>(){ "Summary", "TemperatureF"};
            var forecast = weatherForecastClient.GetFromJsonAsync<JsonElement>("imperial").Result;

            foreach (var property in forecast.EnumerateObject())
            {
                Assert.IsTrue(expectedPropertyNames.Contains(property.Name));
                expectedPropertyNames.Remove(property.Name);
            }

            Assert.IsTrue(0 == expectedPropertyNames.Count());
        }

        [TestMethod]
        public void FullWeatherForecast_ReturnsSummaryTemperatureFTemperatureCAndDate()
        {
            var expectedPropertyNames = new HashSet<string>() { "Summary", "TemperatureF", "TemperatureC", "Date" };
            var forecast = weatherForecastClient.GetFromJsonAsync<JsonElement>("full").Result;

            foreach (var property in forecast.EnumerateObject())
            {
                Assert.IsTrue(expectedPropertyNames.Contains(property.Name));
                expectedPropertyNames.Remove(property.Name);
            }

            Assert.IsTrue(0 == expectedPropertyNames.Count());
        }
    }
}
