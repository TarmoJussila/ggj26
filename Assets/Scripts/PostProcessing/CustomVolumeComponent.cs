using System;
using UnityEngine.Rendering;

namespace Logbound.PostProcessing
{
    [Serializable]
    public class CustomVolumeComponent : VolumeComponent
    {
        public bool Active() => true;
    }
}