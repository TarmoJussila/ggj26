using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PlayerJoinHelper : MonoBehaviour
    {
        [SerializeField] private Camera howToJoinCamera;
        [SerializeField] private GameObject howToJoinCanvas;
        [SerializeField] private List<Vector3> _spawnPoints;

        private int _playerJoinedCount;

        public static event Action OnPlayerAdded;
        public static event Action OnPlayerRemoved;

        private void OnPlayerJoined()
        {
            _playerJoinedCount++;
            if (_playerJoinedCount == 1)
            {
                howToJoinCanvas.SetActive(false);
                howToJoinCanvas.SetActive(false);
            }
            OnPlayerAdded?.Invoke();
        }

        private void OnPlayerLeft()
        {
            OnPlayerRemoved?.Invoke();
        }

        private int SpawnIndex()
        {
            return (_spawnPoints.Count - 1) % _playerJoinedCount;
        }

        public Vector3 GetSpawnPoint()
        {
            return _spawnPoints[SpawnIndex()];
        }
    }
}
