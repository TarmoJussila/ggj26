using Logbound.Gameplay;
using Logbound.Rats;
using UnityEngine;
using UnityEngine.UI;

namespace Logbound.UI
{
    public enum ProgressBarType
    {
        HouseWarmth,
        RatCount
    }

    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private ProgressBarType _progressBarType;
        [SerializeField] private Image _fillImage;
        [SerializeField] private AudioSource _loseAudioSource;
        [SerializeField] private AudioClip _loseSoundRats;
        [SerializeField] private AudioClip _loseSoundCold;
        
        private const float LoseThreshold = 0.01f;
        private bool hasPlayedLoseSound = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _loseAudioSource.PlayOneShot(_loseSoundCold);
            }
            
            if (_fillImage == null)
                return;

            switch (_progressBarType)
            {
                case ProgressBarType.HouseWarmth:
                    UpdateHouseWarmth();
                    break;
                case ProgressBarType.RatCount:
                    UpdateRatCount();
                    break;
            }
        }

        private void UpdateHouseWarmth()
        {
            // Convert temperature to 0-1 range: -30 or below = 0, 20 or above = 1
            float indoorTemp = HouseWarmthHandler.Instance.IndoorTemperature;
            // Map -30 to 20 range to 0 to 1
            float warmthFactor = Mathf.Clamp01((indoorTemp + 30f) / 50f);
            _fillImage.fillAmount = warmthFactor;

            if (warmthFactor < LoseThreshold)
            {
                GameLoseController.Instance.LoseGame();
                if (!hasPlayedLoseSound)
                {
                    _loseAudioSource.PlayOneShot(_loseSoundCold);
                    hasPlayedLoseSound = true;
                }
            }
        }

        private void UpdateRatCount()
        {
            const int maxRats = 20;
            int ratCount = RatController.Instance.GetActiveRatCount();
            // 0 rats = full bar (1), 20 rats = empty bar (0)
            _fillImage.fillAmount = 1f - Mathf.Clamp01((float)ratCount / maxRats);

            if (_fillImage.fillAmount < LoseThreshold)
            {
                GameLoseController.Instance.LoseGame();
                if (!hasPlayedLoseSound)
                {
                    _loseAudioSource.PlayOneShot(_loseSoundRats);
                    hasPlayedLoseSound = true;
                }
            }
        }
    }
}
