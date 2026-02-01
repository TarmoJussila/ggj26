using Logbound.Gameplay;

namespace Logbound.Tools
{
    public class ToolAxe : ToolSwingable
    {
        protected override void OnToolUsed(PlayerInteraction playerInteraction)
        {
            playerInteraction.LastFoundInteractable.UseTool(playerInteraction);
        }
    }
}
