using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Logbound.Gameplay
{
    public class BillboardRotator : MonoBehaviour
    {
        public int PlayerIndex;

        private Transform _targetTransform;

        private void Awake()
        {
            PlayerJoinHelper.OnPlayerAdded += OnPlayerAdded;
        }

        private void OnDestroy()
        {
            PlayerJoinHelper.OnPlayerRemoved += OnPlayerAdded;
        }

        private void OnPlayerAdded()
        {
            var inputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);

            _targetTransform = inputs.FirstOrDefault(i => i.playerIndex == PlayerIndex)?.transform;
        }

        private void Update()
        {
            if (_targetTransform != null)
            {
                Vector3 forward =  _targetTransform.position - transform.position;
                forward.y = 0;
                transform.forward = forward;
            }
        }
    }
}
