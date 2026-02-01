using Logbound.Gameplay;
using UnityEngine;

namespace Logbound.Rats
{
    public class CarryableRatTrap : CarryableItem
    {
        [SerializeField] private RatTrapHazard _ratTrapHazard;

        protected override void OnStartCarry()
        {
            _ratTrapHazard.SetActive();
        }

        // public override void Interact(PlayerInteraction playerInteraction)
        // {
        //     if (_ratTrapHazard.IsActive)
        //     {
        //         // maybe play a sound or something to indicate it's active
        //         return;
        //     }
        //
        //     base.Interact(playerInteraction);
        // }
    }
}
