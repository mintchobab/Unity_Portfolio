using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InteractTree : InteractBase
    {
        public override void Interact()
        {
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
