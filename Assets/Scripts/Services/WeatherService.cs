using System;
using Logbound.Data;
using Logbound.Utilities;
using UnityEngine;

namespace Logbound.Services
{
    public class WeatherService : Singleton<WeatherService>
    {
        public event Action<float> OnTemperatureChanged;
        public event Action<WeatherState> OnWeatherChanged;

        private float _currentTemperature;
        private WeatherState _currentWeatherState;

        public void SetWeatherState(WeatherState state)
        {
            if (_currentWeatherState != state)
            {
                _currentWeatherState = state;
                OnWeatherChanged?.Invoke(state);
            }
        }

        public WeatherState GetWeatherState()
        {
            return _currentWeatherState;
        }

        public void SetTemperature(float temperature)
        {
            if (!Mathf.Approximately(temperature, _currentTemperature))
            {
                _currentTemperature = temperature;
                OnTemperatureChanged?.Invoke(temperature);
            }
        }
        
        public float GetTemperature(bool inCelsius = true)
        {
            if (inCelsius)
            {
                return _currentTemperature;
            }
            else
            {
                return (_currentTemperature * 9 / 5) + 32;
            }
        }
        
        public string GetTemperatureString(bool inCelsius = true)
        {
            if (inCelsius)
            {
                return GetTemperatureCelsiusString();
            }
            else
            {
                return GetTemperatureFahrenheitString();
            }
        }
        
        private string GetTemperatureCelsiusString()
        {
            return $"{GetTemperature(true)} °C";
        }
        
        private string GetTemperatureFahrenheitString()
        {
            return $"{GetTemperature(false)} °F";
        }
    }
}