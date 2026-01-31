using System;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private float _lifetime;
        private float _age;
        [field: SerializeField] public int DamagePerTick { get; private set; }

        private void Update()
        {
            _age += Time.deltaTime;

            if (_age > _lifetime && _lifetime > 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
