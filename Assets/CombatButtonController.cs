using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class CombatButtonController : MonoBehaviour
    {
        [SerializeField]
        private SkillButton[] skillButtons;



        // 개수에 따라 달라지게>..?????? 이건 보류
        public void ChangeSkillButton(List<SkillData> skillDatas)
        {
            for (int i = 0; i < skillDatas.Count; i++)
            {
                skillButtons[i].ChangeButton(skillDatas[i].buttonSprite, skillDatas[i].name);
            }            
        }

    }
}
