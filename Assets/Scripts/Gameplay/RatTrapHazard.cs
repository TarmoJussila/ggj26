using System;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class RatTrapHazard : Hazard
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Rat") || other.CompareTag("Player")) // TODO: Player tag
            {
                Debug.Log("Rat Trap Triggered by Tag: " + other.tag + " Destroy trap");
                Destroy(gameObject);
            }
        }
    }
}
