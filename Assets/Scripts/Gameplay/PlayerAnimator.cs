using System.Collections.Generic;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private List<PlayerVisuals> _visuals;

        public void SetAnimation(Anim animation)
        {
            foreach (var vis in _visuals)
            {
                vis.SetAnimation(animation);
            }
        }
    }
}
