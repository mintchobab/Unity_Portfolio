using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InteractTree : InteractBase
    {
        // 테스트용 Player
        private PlayerController playerController;


        protected override void Awake()
        {
            base.Awake();
            playerController = FindObjectOfType<PlayerController>();
            
        }


        public override void Interact()
        {
            playerController.StartChop();
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
