using UnityEngine;

namespace Logbound.Gameplay
{
    public abstract class InteractableItem : MonoBehaviour
    {
        public abstract void Interact(PlayerInteraction playerInteraction);
    }
}
