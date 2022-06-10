using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InteractGathering : InteractBase
    {
        protected override void SetInteractData()
        {
            InteractData = new InteractData()
            {
                InteractType = InteractType.Gathering,

                StartHash = Animator.StringToHash("gathering"),
                EndHash = Animator.StringToHash("endGathering"),

                InteractTime = 3f,
                InteractDistance = 1f,

                ButtonIconPath = ResourcePath.IconGathering,
            };
        }
    }
}
