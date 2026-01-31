using Logbound.Gameplay;
using UnityEngine;

namespace Logbound
{
    public class ToolAxe : ToolSwingable
    {
        protected override void OnToolUsed(PlayerInteraction playerInteraction)
        {
            playerInteraction.LastFoundInteractable.UseTool(playerInteraction);
        }
    }
}
