using UnityEngine;

namespace Logbound.Gameplay
{
    public class Door : InteractableItem
    {
        public bool IsOpen { get; private set; }
        public bool AffectsHouseWarmth => _affectsHouseWarmth;

        [SerializeField] private ParticleSystem _openParticles;

        [SerializeField] bool _affectsHouseWarmth = true;

        [SerializeField] private int _openDirection = 1;

        private float _initialRotationY;
        private float _targetRotationY;
        private float _rotationY;

        private void Start()
        {
            _initialRotationY = transform.localRotation.eulerAngles.y;
            _targetRotationY = transform.localRotation.eulerAngles.y + 90f * _openDirection;
        }

        private void Update()
        {
            _rotationY = Mathf.MoveTowardsAngle(_rotationY, IsOpen ? _targetRotationY : _initialRotationY, Time.deltaTime * 3 * 90);
            transform.localRotation = Quaternion.Euler(0, _rotationY, 0);
        }

        public override void Interact(PlayerInteraction playerInteraction)
        {
            IsOpen = !IsOpen;

            if (_openParticles == null)
            {
                return;
            }

            if (IsOpen)
            {
                _openParticles.Play();
            }
            else
            {
                _openParticles.Stop();
            }
        }
    }
}
