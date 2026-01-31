using UnityEngine;
using UnityEngine.UI;

namespace Logbound.UI
{
    public class ProgressBarIconFollower : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private RectTransform _iconRectTransform;

        private RectTransform _fillRectTransform;

        private void Awake()
        {
            if (_fillImage != null)
            {
                _fillRectTransform = _fillImage.rectTransform;
            }
        }

        private void Update()
        {
            UpdateIconPosition();
        }

        private void UpdateIconPosition()
        {
            if (_fillImage == null || _fillRectTransform == null || _iconRectTransform == null)
                return;

            // Get the fill value directly from the Image component
            float fillValue = _fillImage.fillAmount;

            // Get the width of the fill bar
            float fillWidth = _fillRectTransform.rect.width;
            
            // Calculate the x position based on fill value
            float xPosition = fillWidth * fillValue;
            
            // Position the icon at the fill point
            Vector3 iconPosition = _iconRectTransform.localPosition;
            iconPosition.x = _fillRectTransform.localPosition.x - (fillWidth * _fillRectTransform.pivot.x) + xPosition;
            _iconRectTransform.localPosition = iconPosition;
        }
    }
}
