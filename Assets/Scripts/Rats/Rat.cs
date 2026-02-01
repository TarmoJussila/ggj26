using System;
using Logbound.Gameplay;
using UnityEngine;

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
                var hazard = other.GetComponent<Hazard>();

                if (hazard is not RatTrapHazard trap)
                {
                    Die();
                    return;
                }
                
                if (!trap.IsActive)
                {
                    return;
                }
                
                trap.Triggered();
                Die();
            }
        }
    }
}
