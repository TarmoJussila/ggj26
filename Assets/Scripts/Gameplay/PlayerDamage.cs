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

        private void Update()
        {
            PlayerHeat = Mathf.Clamp(PlayerHeat - 1, 0, MaxHeat);
            
            if (PlayerHeat <= 0)
            {
                TakeDamage(2);
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
