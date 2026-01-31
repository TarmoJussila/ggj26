using System;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PlayerDamage : MonoBehaviour
    {
        public event Action OnPlayerTakeDamage;
        public event Action OnPlayerDead;
        public event Action OnPlayerHeal;
        public event Action OnPlayerResurrect;
        public event Action OnPlayerHeatChange;

        public int Health { get; private set; }
        public int PlayerHeat { get; private set; }

        public int MaxHealth = 10000;
        public int MaxHeat = 10000; 

        private void Start()
        {
            Health = MaxHealth;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Heat") && other.TryGetComponent(out Fireplace fireplace))
            {
                PlayerHeat += fireplace.GetPlayerHeatingEffect();
            }
            
            if (!other.CompareTag($"Hazard"))
            {
                return;
            }

            if (!other.TryGetComponent(out Hazard hazard))
            {
                return;
            }

            TakeDamage(hazard.DamagePerTick);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Take damage from rat trap.
            if (other.CompareTag("Hazard") && other.TryGetComponent(out RatTrapHazard hazard))
            {
                TakeDamage(hazard.DamagePerTick);
                hazard.Triggered();
            }
        }

        private void Update()
        {
            float currentTemperature = WeatherTransitionController.Instance.GetCurrentTemperature();
            if (currentTemperature >= 0)
            {
                return;
            }

            int weatherDamage = Mathf.CeilToInt(Mathf.Abs(currentTemperature) / 5f);
            
            PlayerHeat = Mathf.Clamp(PlayerHeat - weatherDamage, 0, MaxHeat);
            
            if (PlayerHeat <= 0)
            {
                TakeDamage(weatherDamage);
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            OnPlayerTakeDamage?.Invoke();

            if (Health <= 0)
            {
                Kill();
            }
        }

        public void Heal(int health)
        {
            int oldHealth = Health;

            Health += health;

            if (oldHealth < 0 && Health > 0)
            {
                OnPlayerResurrect?.Invoke();
            }

            OnPlayerHeal?.Invoke();
        }

        public void Kill()
        {
            OnPlayerDead?.Invoke();
        }
    }
}
