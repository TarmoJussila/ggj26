using System.Collections.Generic;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class Fireplace : InteractableItem
    {
        [SerializeField] private float _maxBurnTime;
        [SerializeField] private float _burnTimePerLog;
        [SerializeField] private float _playerHeatingEffect;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _audioVolume;

        public float BurnTimeLeft { get; private set; }

        [SerializeField] private List<ParticleSystem> _burnParticles;

        private void Start()
        {
            BurnTimeLeft = _maxBurnTime;
        }

        private void Update()
        {
            BurnTimeLeft -= Time.deltaTime;

            BurnTimeLeft = Mathf.Clamp(BurnTimeLeft, 0, _maxBurnTime);
            _audioSource.volume = _audioVolume * (BurnTimeLeft / _maxBurnTime);
            foreach (var particle in _burnParticles)
            {
                particle.transform.localScale = Vector3.one * BurnTimeLeft / _maxBurnTime;
            }
        }

        public float GetHeatNormalized() => Mathf.Clamp01(BurnTimeLeft / _maxBurnTime);

        public int GetPlayerHeatingEffect() => Mathf.CeilToInt(GetHeatNormalized() * _playerHeatingEffect);

        public override void Interact(PlayerInteraction playerInteraction) { }

        public override bool CanInteractWithOtherItem(InteractableItem otherItem)
        {
            return otherItem is Log { CanHeatFirePlace: true };
        }

        public void AddLog(Log log)
        {
            Destroy(log.gameObject);
            BurnTimeLeft += _burnTimePerLog;
        }
    }
}
