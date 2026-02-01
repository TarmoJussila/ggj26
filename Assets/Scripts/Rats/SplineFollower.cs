using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Logbound.Rats
{
    public class SplineFollower : MonoBehaviour
    {
        [Header("Settings")]
        public SplineContainer SplineContainer;
        public float Speed = 5f;
        [SerializeField] private bool _loop = true;
        [SerializeField] private bool _faceForward = true;
        [SerializeField] private float _minSpeed = 3f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _maxScale = 2f;
        [SerializeField] private float _minScale = 1f;
    
        private float _distancePercentage = 0f;
        private float _splineLength;

        public void Initialize()
        {
            if (SplineContainer != null)
                _splineLength = SplineContainer.CalculateLength();
            Speed = Random.Range(_minSpeed, _maxSpeed);
            float randomScale = Random.Range(_minScale, _maxScale);
            transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }

        private void Update()
        {
            if (SplineContainer == null || _splineLength <= 0) return;

            _distancePercentage += (Speed * Time.deltaTime) / _splineLength;

            if (_loop)
            {
                _distancePercentage %= 1f; 
            }
            else
            {
                _distancePercentage = Mathf.Clamp01(_distancePercentage);
                if (_distancePercentage >= 1f) return;
            }

            transform.position = (Vector3)SplineContainer.EvaluatePosition(_distancePercentage);

            if (_faceForward)
            {
                float3 forward = SplineContainer.EvaluateTangent(_distancePercentage);
                float3 up = SplineContainer.EvaluateUpVector(_distancePercentage);
            
                if (!forward.Equals(float3.zero))
                {
                    transform.rotation = Quaternion.LookRotation(forward, up);
                }
            }
        }
    }
}