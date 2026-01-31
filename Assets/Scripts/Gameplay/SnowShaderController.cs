using Logbound.Data;
using Logbound.Services;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class SnowShaderController : MonoBehaviour
    {
        [Header("Material")]
        [SerializeField] private Material _snowMaterial;

        [Header("Height Decay Settings")]
        [SerializeField] private float _snowfallHeightDecay = 100f;
        [SerializeField] private float _noSnowHeightDecay = 10f;

        [Header("Decay Duration Settings")]
        [SerializeField] private float _snowfallDecayDuration = 100f;
        [SerializeField] private float _noSnowDecayDuration = 10f;

        [Header("Transition")]
        [SerializeField] private float _transitionSpeed = 10f;

        private static readonly int HeightDecayProperty = Shader.PropertyToID("_HeightDecay");
        private static readonly int DecayDurationProperty = Shader.PropertyToID("_DecayDuration");

        private float _targetHeightDecay;
        private float _targetDecayDuration;
        private float _currentHeightDecay;
        private float _currentDecayDuration;

        private void Start()
        {
            _currentHeightDecay = _noSnowHeightDecay;
            _currentDecayDuration = _noSnowDecayDuration;
            _targetHeightDecay = _noSnowHeightDecay;
            _targetDecayDuration = _noSnowDecayDuration;
        }

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
            if (state == WeatherState.Snowfall)
            {
                _targetHeightDecay = _snowfallHeightDecay;
                _targetDecayDuration = _snowfallDecayDuration;
            }
            else
            {
                _targetHeightDecay = _noSnowHeightDecay;
                _targetDecayDuration = _noSnowDecayDuration;
            }
        }

        private void Update()
        {
            if (_snowMaterial == null)
            {
                return;
            }

            bool heightDecayChanged = !Mathf.Approximately(_currentHeightDecay, _targetHeightDecay);
            bool decayDurationChanged = !Mathf.Approximately(_currentDecayDuration, _targetDecayDuration);

            if (!heightDecayChanged && !decayDurationChanged)
            {
                return;
            }

            if (heightDecayChanged)
            {
                _currentHeightDecay = Mathf.MoveTowards(_currentHeightDecay, _targetHeightDecay, _transitionSpeed * Time.deltaTime);
                _snowMaterial.SetFloat(HeightDecayProperty, _currentHeightDecay);
            }

            if (decayDurationChanged)
            {
                _currentDecayDuration = Mathf.MoveTowards(_currentDecayDuration, _targetDecayDuration, _transitionSpeed * Time.deltaTime);
                _snowMaterial.SetFloat(DecayDurationProperty, _currentDecayDuration);
            }
        }
    }
}

