using System;
using Logbound.Services;
using UnityEngine;
using UnityEngine.Splines;

namespace Logbound.Rats
{
    public class Rat : MonoBehaviour
    {
        public static event Action OnRatDied;
        
        [SerializeField] private SplineFollower _splineFollower;

        public void Die()
        {
            OnRatDied?.Invoke();
            RatController.Instance.RemoveRat(_splineFollower);
            // TODO: Ebin death animation/state.
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hazard") || other.CompareTag("RatTrap"))
            {
                Die();
            }
        }
    }
}
