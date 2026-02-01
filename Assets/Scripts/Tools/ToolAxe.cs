using Logbound.Gameplay;
using UnityEngine;

namespace Logbound.Tools
{
    public class ToolAxe : ToolSwingable
    {
        [Header("References")]
        [SerializeField] private AudioSource _audioSource;
        
        protected override void OnToolUsed(PlayerInteraction playerInteraction)
        {
            playerInteraction.LastFoundInteractable.UseTool(playerInteraction);
            _audioSource.Play();
        }
    }
}
