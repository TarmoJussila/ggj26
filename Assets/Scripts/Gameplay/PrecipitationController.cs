using Logbound.Data;
using Logbound.Services;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PrecipitationController : MonoBehaviour
    {
        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem _rainParticleSystem;
        [SerializeField] private ParticleSystem _snowParticleSystem;

        private void OnEnable()
        {
            WeatherService.Instance.OnTargetWeatherStateChanged += OnWeatherStateChanged;
            OnWeatherStateChanged(WeatherService.Instance.GetTargetWeatherState());
        }

        private void OnDisable()
        {
            WeatherService.Instance.OnTargetWeatherStateChanged -= OnWeatherStateChanged;
        }

        private void OnWeatherStateChanged(WeatherState state)
        {
            switch (state)
            {
                case WeatherState.Rain:
                case WeatherState.Thunderstorm:
                    StopSnow();
                    PlayRain();
                    break;
                case WeatherState.Snowfall:
                    StopRain();
                    PlaySnow();
                    break;
                default:
                    StopRain();
                    StopSnow();
                    break;
            }
        }

        private void PlayRain()
        {
            if (_rainParticleSystem != null && !_rainParticleSystem.isPlaying)
            {
                _rainParticleSystem.Play();
            }
        }

        private void StopRain()
        {
            if (_rainParticleSystem != null && _rainParticleSystem.isPlaying)
            {
                _rainParticleSystem.Stop();
            }
        }

        private void PlaySnow()
        {
            if (_snowParticleSystem != null && !_snowParticleSystem.isPlaying)
            {
                _snowParticleSystem.Play();
            }
        }

        private void StopSnow()
        {
            if (_snowParticleSystem != null && _snowParticleSystem.isPlaying)
            {
                _snowParticleSystem.Stop();
            }
        }
    }
}

