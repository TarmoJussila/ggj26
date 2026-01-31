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

        public int Health { get; private set; }

        public int MaxHealth;

        private void Start()
        {
            Health = MaxHealth;
        }

        private void OnTriggerStay(Collider other)
        {
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
