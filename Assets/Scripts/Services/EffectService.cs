using Logbound.Utilities;
using UnityEngine;

namespace Logbound.Services
{
    public class EffectService : Singleton<EffectService>
    {
        [field: SerializeField] public Material SnowTrackerMaterial { get; private set; }
    }
}