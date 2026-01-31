using UnityEngine;

namespace Logbound.Gameplay
{
    public abstract class InteractableItem : MonoBehaviour
    {
        public abstract void Interact(PlayerInteraction playerInteraction);

        public virtual void UseTool(PlayerInteraction playerInteraction)
        {
            
        }

        public virtual bool CanInteractWithOtherItem(InteractableItem otherItem)
        {
            return false;
        }

    }
}
