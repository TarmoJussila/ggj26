using UnityEngine;

namespace Logbound.Gameplay
{
    public class Hazard : MonoBehaviour
    {
        [field: SerializeField] public int DamagePerTick { get; private set; }
    }
}
