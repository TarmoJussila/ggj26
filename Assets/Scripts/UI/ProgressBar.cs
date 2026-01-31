using Logbound.Gameplay;
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
            // Convert temperature to 0-1 range: -50 = 0, 0+ = 1
            float warmthFactor = 1f - HouseWarmthHandler.Instance.GetColdFactor();
            _fillImage.fillAmount = warmthFactor;
        }

        private void UpdateRatCount()
        {
            // TODO: Implement rat count logic
            _fillImage.fillAmount = 0f;
        }
    }
}
