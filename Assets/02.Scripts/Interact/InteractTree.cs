using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InteractTree : InteractBase
    {
        // �׽�Ʈ�� Player
        private PlayerController playerController;


        protected override void Awake()
        {
            base.Awake();
            playerController = FindObjectOfType<PlayerController>();
            
        }


        public override void Interact()
        {
            playerController.StartChop();
            // 1. ĳ���� ���� �ִϸ��̼�
            // 2. ���� �ð� ������ ������ ������?
            // 3. ������ �������� ������ ȹ��
        }

        protected override void SetButtonImage()
        {
            buttonImage = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.InteractAxing);
        }


    }
}
