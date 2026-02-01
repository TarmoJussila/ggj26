using UnityEngine;
using UnityEngine.EventSystems;

namespace Logbound.UI
{
    public class CustomDropdown : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var child = GameObject.Find("Dropdown List");
            if (child == null)
            {
                Debug.LogWarning("Dropdown List not found");
                return;
            }

            ((RectTransform)child.transform).sizeDelta = new Vector2(0, 550);
        }
    }
}