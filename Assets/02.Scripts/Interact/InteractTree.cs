using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InteractTree : InteractBase
    {
        public override void Interact()
        {
            // 1. 캐릭터 도끼 애니메이션
            // 2. 일정 시간 지나면 나무가 쓰러짐?
            // 3. 나무가 쓰러지면 아이템 획득
        }

        protected override void SetButtonImage()
        {
            buttonImage = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.InteractAxing);
        }


    }
}
