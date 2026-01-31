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

        private void Update()
        {
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
        }

        private void UpdateRatCount()
        {
            const int maxRats = 20;
            int ratCount = RatController.Instance.GetActiveRatCount();
            // 0 rats = full bar (1), 20 rats = empty bar (0)
            _fillImage.fillAmount = 1f - Mathf.Clamp01((float)ratCount / maxRats);
        }
    }
}
