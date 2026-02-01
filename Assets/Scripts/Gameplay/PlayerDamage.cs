using System;
using Logbound.Masks;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PlayerDamage : MonoBehaviour
    {
        public const float HeatPercentageOnRespawn = 0.25f;

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

        [SerializeField] private float _maskHealing;

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

            float damage = hazard.DamagePerTick;
            var mask = _playerMaskHelper.CurrentMask?.MaskType;

            if (mask == MaskType.GAS)
            {
                damage = 0;
            }
            else if (mask == MaskType.COVID)
            {
                damage /= 4;
            }

            TakeDamage(damage);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Take damage from rat trap.
            if (other.CompareTag("Hazard") && other.TryGetComponent(out RatTrapHazard hazard))
            {
                if (hazard.IsActive)
                {
                    TakeDamage(hazard.DamagePerTick);
                    hazard.Triggered();
                }
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            // Take damage from rat trap.
            if (other.gameObject.CompareTag("Hazard") && other.gameObject.TryGetComponent(out RatTrapHazard hazard))
            {
                TakeDamage(hazard.DamagePerTick);
                hazard.Triggered();
            }
        }

        private void Update()
        {
            var mask = _playerMaskHelper.CurrentMask?.MaskType;

            if (mask == MaskType.SLEEPING || mask == MaskType.SKINCARE)
            {
                Heal(_maskHealing * Time.deltaTime);
            }

            float currentTemperature = WeatherTransitionController.Instance.GetCurrentTemperature();
            if (currentTemperature >= 0)
            {
                return;
            }

            float weatherDamage = Mathf.Ceil(Mathf.Abs(currentTemperature) / 5f);

            if (mask == MaskType.SKIMASK)
            {
                weatherDamage /= 10;
            }

            if (mask == MaskType.COVID)
            {
                weatherDamage /= 2;
            }

            if (mask == MaskType.WELDING)
            {
                weatherDamage *= 1.5f;
            }

            PlayerHeat = Mathf.Clamp(PlayerHeat - weatherDamage, 0, MaxHeat);

            if (PlayerHeat <= 0)
            {
                TakeDamage(weatherDamage);
            }
        }

        public void TakeDamage(float damage)
        {
            if (Health <= 0)
            {
                return;
            }

            var mask = _playerMaskHelper.CurrentMask?.MaskType;

            if (mask == MaskType.PROTECTIVE || mask == MaskType.WELDING)
            {
                damage /= 2;
            }

            Health -= damage;

            OnPlayerTakeDamage?.Invoke();

            if (Health <= 0)
            {
                Kill();
            }
        }

        public void Resurrect()
        {
            Health = MaxHealth;
            PlayerHeat = MaxHeat * HeatPercentageOnRespawn;
            OnPlayerResurrect?.Invoke();
            OnPlayerHeal?.Invoke();
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
            GetComponent<PlayerInteraction>().DropPressed();
            GetComponent<PlayerInteraction>().DropPressed();
            
            OnPlayerDead?.Invoke();
        }
    }
}
