using System;
using Logbound.Data;
using Logbound.Utilities;
using UnityEngine;

namespace Logbound.Services
{
    public class WeatherService : Singleton<WeatherService>
    {
        public event Action<WeatherState> OnTargetWeatherStateChanged;
        public event Action<float> OnTargetTemperatureChanged;

        private WeatherState _targetWeatherState;
        private float _targetTemperature;

        private void OnEnable()
        {
            ForecastService.Instance.OnForecastUpdated += SetTargetWeatherStateAndTemperature;
        }

        private void OnDestroy()
        {
            ForecastService.Instance.OnForecastUpdated -= SetTargetWeatherStateAndTemperature;
        }
        
        private void SetTargetWeatherStateAndTemperature(WeatherState state, float temperature)
        {
            SetTargetWeatherState(state);
            SetTargetTemperature(temperature);
        }

        private void SetTargetWeatherState(WeatherState state)
        {
            if (_targetWeatherState != state)
            {
                _targetWeatherState = state;
                OnTargetWeatherStateChanged?.Invoke(state);
            }
        }
        
        private void SetTargetTemperature(float temperature)
        {
            if (!Mathf.Approximately(temperature, _targetTemperature))
            {
                _targetTemperature = temperature;
                OnTargetTemperatureChanged?.Invoke(temperature);
            }
        }

        public WeatherState GetTargetWeatherState()
        {
            return _targetWeatherState;
        }
        
        public float GetTargetTemperature(bool inCelsius = true)
        {
            return WeatherUtility.GetTemperature(_targetTemperature, inCelsius);
        }
    }
}