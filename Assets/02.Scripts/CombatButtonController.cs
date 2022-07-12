using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class CombatButtonController : MonoBehaviour
    {
        [SerializeField]
        private SkillButton normalAttackButton;

        [SerializeField]
        private SkillButton[] skillButtons;



        // ������ ���� �޶�����>..?????? �̰� ����
        public void ChangeSkillButton(SkillData normalAttackData, SkillData[] skillDatas)
        {
            //normalAttackButton.ChangeButton(normalAttackData.skillIcon)

            for (int i = 0; i < skillDatas.Length; i++)
            {
                skillButtons[i].ChangeButton(skillDatas[i].skillIcon, skillDatas[i].skillName);
            }            
        }

        public void ActivateCombatButton()
        {

        }


        public void TestButtonMove()
        {
            foreach(SkillButton button in skillButtons)
            {
                button.TestMove();
            }
        }
    }
}
