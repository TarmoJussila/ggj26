using System;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class RatTrapHazard : Hazard
    {
        [SerializeField] private GameObject _activeVisual;
        [SerializeField] private GameObject _inactiveVisual;
        [SerializeField] private AudioSource _triggerSound;
        public bool IsActive = true;
        public void Triggered()
        {
            IsActive = false;
            UpdateVisual();
            _triggerSound.Play();
        }

        public void SetActive()
        {
            IsActive = true;
            UpdateVisual();
        }
        
        public void UpdateVisual()
        {
            _activeVisual.SetActive(IsActive);
            _inactiveVisual.SetActive(!IsActive);
        }
    }
}
