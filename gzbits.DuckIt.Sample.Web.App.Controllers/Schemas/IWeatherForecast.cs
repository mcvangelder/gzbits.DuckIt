namespace gzbits.DuckIt.Sample.Web.App.Controllers.Schemas
{
    public interface IWeatherForecastImperial
    {
        DateTime Date { get; }
        string Summary { get; }
        int TemperatureF { get; }
    }
}
