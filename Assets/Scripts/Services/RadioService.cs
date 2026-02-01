using System;
using System.Collections;
using System.Linq;
using Logbound.Data;
using Logbound.Utilities;
using UnityEngine;

namespace Logbound.Services
{
    public class RadioService : Singleton<RadioService>
    {
        [Serializable]
        public class TaggedNewsAudioClip
        {
            public WeatherState WeatherStates;
            public float Temperature;
            public AudioClip AudioClip;
        }
        
        [Header("References")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _newsSource;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _radioLocation;
        
        [Header("Settings")]
        [SerializeField] [Range(0.1f, 10f)] private float _musicVolume;
        [SerializeField] [Range(0.1f, 10f)] private float _newsVolume;
        [SerializeField] private TaggedNewsAudioClip[] _audioClips;
        [SerializeField] private AudioClip _intro;
        [SerializeField] private AudioClip _start;
        [SerializeField] private AudioClip _middle;
        [SerializeField] private AudioClip _end;

        private bool _isPlaying = false;
        private bool isUpdate = false;

        private void Start()
        {
            _musicSource.transform.position = _radioLocation.transform.position;
            _newsSource.transform.position = _radioLocation.transform.position;
            ForecastService.Instance.OnForecastUpdated += OnWeatherUpdate;
        }

        private void OnWeatherUpdate(WeatherState state,  float temperature)
        {
            isUpdate = !isUpdate;
            if (!isUpdate)
            {
                return;
            }

            WeatherState currentState = ForecastService.Instance.GetWeatherState(WeatherUtility.WeatherTimeState.Current);
            WeatherState nextState = ForecastService.Instance.GetWeatherState(WeatherUtility.WeatherTimeState.Next);
            float currentTemp = ForecastService.Instance.GetTemperature(WeatherUtility.WeatherTimeState.Current);
            float nextTemp = ForecastService.Instance.GetTemperature(WeatherUtility.WeatherTimeState.Next);
            
            AudioClip currentClip = _audioClips.Where(x => x.WeatherStates == currentState)
                .OrderBy(x => Mathf.Abs(x.Temperature - currentTemp))
                .FirstOrDefault()?.AudioClip;
            
            AudioClip nextClip = _audioClips.Where(x => x.WeatherStates == nextState)
                .OrderBy(x => Mathf.Abs(x.Temperature - nextTemp))
                .FirstOrDefault()?.AudioClip;

            if (currentClip == null || nextClip == null)
            {
                Debug.LogError("Clips not found!");
                return;
            }

            PlayNewsSnippet(_intro, _start, currentClip, _middle, nextClip);
        }

        private IEnumerator PlayClipsInOrder(params AudioClip[] clips)
        {
            _isPlaying = true;
            foreach (AudioClip clip in clips)
            {
                while (_newsSource.isPlaying)
                {
                    yield return null;
                }
                
                _newsSource.PlayOneShot(clip);
            }

            _isPlaying = false;
        }

        private IEnumerator FadeCanvasCoroutine(float start, float end, float speed)
        {
            for (float time = 0; time < 1f; time += Time.deltaTime * speed)
            {
                _canvasGroup.alpha = Mathf.Lerp(start, end, time);
                yield return null;
            }
                
            _canvasGroup.alpha = end;
        }

        private IEnumerator FadeVolume(float start, float end, float speed)
        {
            for (float time = 0; time < 1f; time += Time.deltaTime * speed)
            {
                _musicSource.volume = Mathf.Lerp(start, end, time);
                _newsSource.volume = Mathf.Lerp(start, end, time);
                yield return null;
            }
                
            _musicSource.volume = end;
            _newsSource.volume = end;
        }

        private void PlayNewsSnippet(params AudioClip[] clips)
        {
            StopAllCoroutines();
            IEnumerator Coroutine()
            {
                yield return FadeCanvasCoroutine(0, 1, 1);
                yield return FadeVolume(0, _musicVolume, 1);
                while (_isPlaying)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(2f);
                yield return FadeVolume(_musicVolume, 0, 1);
                yield return FadeCanvasCoroutine(1, 0, 1);
            }
            
            StartCoroutine(PlayClipsInOrder(clips));;
            StartCoroutine(Coroutine());
        }
    }
}
