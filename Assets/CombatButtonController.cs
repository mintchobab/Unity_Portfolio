using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class CombatButtonController : MonoBehaviour
    {
        [SerializeField]
        private SkillButton[] skillButtons;



        // ������ ���� �޶�����>..?????? �̰� ����
        public void ChangeSkillButton(List<SkillData> skillDatas)
        {
            for (int i = 0; i < skillDatas.Count; i++)
            {
                skillButtons[i].ChangeButton(skillDatas[i].buttonSprite, skillDatas[i].name);
            }            
        }

    }
}
