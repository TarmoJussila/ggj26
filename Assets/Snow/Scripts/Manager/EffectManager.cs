using Logbound.Utilities;
using UnityEngine;

namespace Manager
{
    public class EffectManager : Singleton<EffectManager>
    {
        [field: SerializeField] public Material SnowTrackerMaterial { get; private set; }
    }
}