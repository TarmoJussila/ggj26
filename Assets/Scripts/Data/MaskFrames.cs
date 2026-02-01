using System.Collections.Generic;
using Logbound.Masks;
using UnityEngine;

namespace Logbound.Data
{
    [CreateAssetMenu(fileName = "MaskFrames", menuName = "Scriptable Objects/MaskFrames")]
    public class MaskFrames : ScriptableObject
    {
        public MaskType MaskType;
        public List<Sprite> Frames;
    }
}
