using System;
using Logbound.Masks;
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

        public float Health { get; private set; }
        public float PlayerHeat { get; private set; }

        public float MaxHealth = 10000;
        public float MaxHeat = 10000;

        private PlayerMaskHelper _playerMaskHelper;

        private void Awake()
        {
            _playerMaskHelper = GetComponent<PlayerMaskHelper>();
        }

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

            bool gasMask = _playerMaskHelper.CurrentMask?.MaskType == MaskType.GAS;
            
            TakeDamage(hazard.DamagePerTick * (gasMask ? 0 : 1));
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

            float weatherDamage = Mathf.Ceil(Mathf.Abs(currentTemperature) / 5f);
            
            PlayerHeat = Mathf.Clamp(PlayerHeat - weatherDamage, 0, MaxHeat);
            
            if (PlayerHeat <= 0)
            {
                TakeDamage(weatherDamage);
            }
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            OnPlayerTakeDamage?.Invoke();

            if (Health <= 0)
            {
                Kill();
            }
        }

        public void Heal(float health)
        {
            float oldHealth = Health;

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
