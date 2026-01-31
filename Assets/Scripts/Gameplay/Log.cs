using System.Collections.Generic;
using Logbound.Gameplay;
using UnityEngine;

namespace Logbound
{
    public class Log : CarryableItem
    {
        public int HitsLeft = 3;

        [SerializeField] private List<GameObject> ObjectsToEnable;
        
        public override void Interact(PlayerInteraction playerInteraction)
        {
            HitsLeft--;

            if (HitsLeft > 1)
            {
                return;
            }

            foreach (GameObject obj in ObjectsToEnable)
            {
                obj.transform.SetParent(null);
                obj.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
