using Logbound.Masks;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class PlayerMaskHelper : MonoBehaviour
    {
        public BasicMaskItem CurrentMask { get; private set; }
        
        public void WearMask(BasicMaskItem maskItem)
        {
            maskItem.transform.SetParent(transform);
            maskItem.transform.localPosition = Vector3.zero;
            maskItem.transform.forward = transform.forward;

            CurrentMask = maskItem;
            
            GetComponentInChildren<PlayerAnimator>().SetMaskType(maskItem.MaskType);
        }

        public void DropMask()
        {
            CurrentMask.transform.SetParent(null);
            CurrentMask.StopCarry();
            CurrentMask = null;
            GetComponentInChildren<PlayerAnimator>().SetMaskType(MaskType.NONE);
        }
    }
}
