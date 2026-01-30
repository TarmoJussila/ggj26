using Logbound.Data;
using Logbound.Utilities;

namespace Logbound.Services
{
    public class WeatherService : Singleton<WeatherService>
    {
        public float TemperatureCelsius { get; set; }
        public WeatherState CurrentWeather { get; set; }

        public void SetWeatherState(WeatherState state)
        {
            CurrentWeather = state;
        }

        public void SetTemperature(float temperature)
        {
            TemperatureCelsius = temperature;
        }
    }
}