using UnityEngine;

namespace Logbound.Gameplay
{
    public class Fridge : InteractableItem
    {
        [SerializeField] private GameObject _beerPrefab;

        public override void Interact(PlayerInteraction playerInteraction)
        {
            playerInteraction.GetComponentInChildren<PlayerAnimator>().SetAnimation(Anim.Drink, true);
            Instantiate(_beerPrefab, transform.position + transform.forward, Quaternion.Euler(Random.insideUnitSphere * 365));
            _beerPrefab.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.VelocityChange);
        }
    }
}
