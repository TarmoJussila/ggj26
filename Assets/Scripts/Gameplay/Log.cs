using System.Collections.Generic;
using Logbound.Tools;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class Log : CarryableItem
    {
        public int HitsLeft = 3;
        public bool CanHeatFirePlace = false;

        [SerializeField] private List<GameObject> ObjectsToEnable;

        public override void UseTool(PlayerInteraction playerInteraction)
        {
            if (!(playerInteraction.CurrentCarryItem is ToolAxe))
            {
                return;
            }

            HitsLeft--;

            if (HitsLeft > 1)
            {
                return;
            }

            foreach (GameObject obj in ObjectsToEnable)
            {
                obj.transform.SetParent(null);
                obj.SetActive(true);
                var rb = obj.GetComponent<Rigidbody>();
                rb.AddForce(Random.insideUnitSphere * (rb.mass / 5));
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerInteraction playerInteraction)
        {
            base.Interact(playerInteraction);

            if (playerInteraction.LastFoundInteractable is Fireplace fireplace)
            {
                fireplace.AddLog(this);
            }
        }

        public override bool CanInteractWithOtherItem(InteractableItem item)
        {
            return item is ToolAxe;
        }
    }
}
