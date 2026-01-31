using System;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PlayerJoinHelper : MonoBehaviour
    {
        [SerializeField] private Camera howToJoinCamera;
        [SerializeField] private GameObject howToJoinCanvas;

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
    }
}
