using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Logbound.Services;
using Logbound.Utilities;

namespace Logbound.Rats
{
    [System.Serializable]
    public class SpawnIntervalThreshold
    {
        [Tooltip("Max number of rats for this interval")]
        public int RatCountThreshold;
        [Tooltip("Spawn interval in seconds")]
        public float SpawnInterval;
    }
    
    public class RatController : Singleton<RatController>
    {
        [Header("Rat Prefab")]
        [SerializeField] private GameObject _ratPrefab;
        
        [Header("Rat GameObjects with SplineFollower")]
        [SerializeField] private List<SplineFollower> _ratObjects = new List<SplineFollower>();
        
        [Header("Available Splines")]
        [SerializeField] private List<SplineContainer> _splines = new List<SplineContainer>();
        
        [Header("Spawn Intervals")]
        [Tooltip("Spawn intervals based on active rat count thresholds")]
        [SerializeField] private List<SpawnIntervalThreshold> _spawnIntervals = new List<SpawnIntervalThreshold>
        {
            new SpawnIntervalThreshold { RatCountThreshold = 3, SpawnInterval = 60f },
            new SpawnIntervalThreshold { RatCountThreshold = 6, SpawnInterval = 50f },
            new SpawnIntervalThreshold { RatCountThreshold = 9, SpawnInterval = 40f },
            new SpawnIntervalThreshold { RatCountThreshold = 12, SpawnInterval = 30f },
            new SpawnIntervalThreshold { RatCountThreshold = int.MaxValue, SpawnInterval = 20f }
        };
        
        [Header("Max Rats")]
        [Tooltip("Maximum number of rats that can be spawned. Set to 0 for unlimited.")]
        [SerializeField] private int _maxRatCount = 20;
        
        [Header("Death Bonus")]
        [Tooltip("Seconds to add to spawn timer when a rat dies")]
        [SerializeField] private float _deathBonusTime = 5f;
        
        private float _spawnTimer;

        private void Start()
        {
            AssignRandomSplinesToRats();
            _spawnTimer = GetCurrentSpawnInterval();
            Rat.OnRatDied += OnRatDied;
        }
        
        private void OnDestroy()
        {
            Rat.OnRatDied -= OnRatDied;
        }
        
        private void Update()
        {
            if (_maxRatCount > 0 && _ratObjects.Count >= _maxRatCount)
            {
                return;
            }
            
            _spawnTimer -= Time.deltaTime;
            
            if (_spawnTimer <= 0f)
            {
                SpawnRat();
                _spawnTimer = GetCurrentSpawnInterval();
            }
        }

        public void AssignRandomSplinesToRats()
        {
            if (_splines.Count == 0)
            {
                Debug.LogWarning("No splines available to assign!");
                return;
            }

            foreach (SplineFollower ratObject in _ratObjects)
            {
                if (ratObject == null) continue;

                SplineFollower follower = ratObject.GetComponent<SplineFollower>();
                if (follower == null)
                {
                    Debug.LogWarning($"GameObject {ratObject.name} does not have a SplineFollower component!");
                    continue;
                }
                int randomIndex = Random.Range(0, _splines.Count);
                follower.SplineContainer = _splines[randomIndex];
                follower.Initialize();
            }
        }
        
        private void SpawnRat()
        {
            if (_ratPrefab == null)
            {
                Debug.LogWarning("Rat prefab is not assigned!");
                return;
            }
            
            if (_splines.Count == 0)
            {
                Debug.LogWarning("No splines available to assign to spawned rat!");
                return;
            }
            
            GameObject newRat = Instantiate(_ratPrefab, transform.position, Quaternion.identity);
            SplineFollower follower = newRat.GetComponent<SplineFollower>();
            if (follower == null)
            {
                Debug.LogWarning($"Spawned rat prefab does not have a SplineFollower component!");
                Destroy(newRat);
                return;
            }
            
            int randomIndex = Random.Range(0, _splines.Count);
            follower.SplineContainer = _splines[randomIndex];
            follower.Initialize();
            _ratObjects.Add(follower);
            
            Debug.Log($"Spawned rat. Active rats: {_ratObjects.Count}");
        }
        
        private float GetCurrentSpawnInterval()
        {
            int activeRatCount = _ratObjects.Count;
            
            if (_spawnIntervals.Count == 0)
            {
                return 60f; 
            }
            
            foreach (var threshold in _spawnIntervals)
            {
                if (activeRatCount <= threshold.RatCountThreshold)
                {
                    return threshold.SpawnInterval;
                }
            }
            return _spawnIntervals[^1].SpawnInterval;
        }
        
        public void RemoveRat(SplineFollower ratFollower)
        {
            if (_ratObjects.Contains(ratFollower))
            {
                _ratObjects.Remove(ratFollower);
                Debug.Log($"Removed rat from pool. Active rats: {_ratObjects.Count}");
            }
        }
        
        private void OnRatDied()
        {
            _spawnTimer += _deathBonusTime;
            Debug.Log($"Rat died! Added {_deathBonusTime}s to spawn timer. New timer: {_spawnTimer:F2}s");
        }
    }
}
