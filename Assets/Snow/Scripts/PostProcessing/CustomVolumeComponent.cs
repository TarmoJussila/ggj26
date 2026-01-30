using System;
using UnityEngine.Rendering;

namespace PostProcessing
{
    [Serializable]
    public class CustomVolumeComponent : VolumeComponent
    {
        public bool Active() => true;
    }
}