using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InteractPickaxing : InteractBase
    {
        protected override void SetInteractData()
        {
            interactData = new InteractData()
            {
                InteractType = InteractType.Pickaxing,

                StartHash = Animator.StringToHash("pickaxing"),
                EndHash = Animator.StringToHash("endPickaxing"),

                InteractTime = 3f,
                InteractDistance = 1.2f,

                ButtonIconPath = ResourcePath.IconPickaxing
            };
        }
    }
}
