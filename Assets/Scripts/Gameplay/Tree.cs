using System.Collections.Generic;
using Logbound.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logbound.Gameplay
{
    public class Tree : InteractableItem
    {
        public int HitsLeft = 3;

        [SerializeField] private List<GameObject> _branches;
        [SerializeField] private List<GameObject> _objectsToEnable;

        private void Start()
        {
            foreach (var branch in _branches)
            {
                branch.gameObject.SetActive(false);
            }

            _branches[Random.Range(0, _branches.Count)].SetActive(true);
        }

        public override void Interact(PlayerInteraction playerInteraction)
        {
            
        }

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

            foreach (GameObject obj in _objectsToEnable)
            {
                obj.transform.SetParent(null);
                obj.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public override bool CanInteractWithOtherItem(InteractableItem item)
        {
            return item is ToolAxe;
        }
    }
}
