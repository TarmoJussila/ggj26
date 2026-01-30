using System;
using UnityEngine;

namespace Logbound
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class CarryableItem : InteractableItem
    {
        private Rigidbody _rb;
        private Collider[] _colliders;

        private PlayerInteraction beingCarriedBy;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        public void StartCarry(PlayerInteraction playerInteraction)
        {
            _rb.isKinematic = true;

            foreach (Collider col in _colliders)
            {
                col.enabled = false;
            }

            beingCarriedBy = playerInteraction;

            OnStartCarry();
        }

        public void StopCarry()
        {
            _rb.isKinematic = false;

            foreach (Collider col in _colliders)
            {
                col.enabled = true;
            }

            OnStopCarry();
        }

        protected abstract void OnStartCarry();

        protected abstract void OnStopCarry();
    }
}
