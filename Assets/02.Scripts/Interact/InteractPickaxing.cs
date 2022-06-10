using UnityEngine;

namespace lsy
{
    public class InteractPickaxing : InteractBase
    {
        protected override void SetInteractData()
        {
            InteractData = new InteractData()
            {
                InteractType = InteractType.Pickaxing,

                ItemType = ItemType.Material,
                ItemId = 200,

                StartHash = Animator.StringToHash("pickaxing"),
                EndHash = Animator.StringToHash("endPickaxing"),                
                
                InteractTime = 3f,
                InteractDistance = 1.2f,

                ButtonIconPath = ResourcePath.IconPickaxing
            };
        }
    }
}
