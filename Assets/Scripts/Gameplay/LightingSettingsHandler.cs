using Logbound.Data;
using Logbound.Services;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class LightingSettingsHandler : MonoBehaviour
    {
        [Header("Fog Density Settings")]
        [SerializeField] private float _minFogDensity = 0.01f;
        [SerializeField] private float _maxFogDensity = 0.1f;
        [SerializeField] private float _transitionSpeed = 0.01f;

        private float _currentFogDensity;
        private float _targetFogDensity;

        private void Start()
        {
            _currentFogDensity = _minFogDensity;
            _targetFogDensity = _minFogDensity;
            RenderSettings.fogDensity = _currentFogDensity;
        }

        private void OnEnable()
        {
            WeatherService.Instance.OnTargetWeatherStateChanged += OnTargetWeatherStateChanged;
        }

        private void OnDisable()
        {
            WeatherService.Instance.OnTargetWeatherStateChanged -= OnTargetWeatherStateChanged;
        }

        private void OnTargetWeatherStateChanged(WeatherState weatherState)
        {
            _targetFogDensity = weatherState == WeatherState.Fog ? _maxFogDensity : _minFogDensity;
        }

        private void Update()
        {
            if (Mathf.Approximately(_currentFogDensity, _targetFogDensity))
            {
                return;
            }
            
            _currentFogDensity = Mathf.MoveTowards(_currentFogDensity, _targetFogDensity, _transitionSpeed * Time.deltaTime);
            RenderSettings.fogDensity = _currentFogDensity;
        }
    }
}

