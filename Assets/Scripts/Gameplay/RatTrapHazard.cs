using System;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class RatTrapHazard : Hazard
    {
        public void Triggered()
        {
            Destroy(gameObject);
        }
    }
}
