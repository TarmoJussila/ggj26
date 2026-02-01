using Logbound.Gameplay;
using Unity.Mathematics;
using UnityEngine;

namespace Logbound.Tools
{
    public class ToolSpawner : ToolSwingable
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _uses;

        protected override void OnToolUsed(PlayerInteraction playerInteraction)
        {
            Instantiate(_prefab, playerInteraction.transform.position + playerInteraction.transform.forward, quaternion.identity);
            _uses--;
            if (_uses <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
