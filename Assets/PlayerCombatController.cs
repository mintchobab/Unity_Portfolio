using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class SkillData
    {
        public Sprite buttonSprite;

        public string name;
        public float coolTime;
    }

    public class PlayerCombatController : MonoBehaviour
    {
        // ★★★ 스킬에 대한 데이터 만들기 ★★★


        [SerializeField]
        private Sprite[] testSkillIcons;


        // 시작되고 거리가 너무 멀어지면 종료
        // 몬스터가 죽으면 다시 대상 찾기
        // 대상 없으면 종료
        public Action onStoppedCombat;

        private bool isCombating;

        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;






        private void Start()
        {
            List<SkillData> skillDatas = new List<SkillData>();

            SkillData skill1 = new SkillData()
            {
                buttonSprite = testSkillIcons[0],
                name = "1번 스킬",
                coolTime = 5f
            };

            SkillData skill2 = new SkillData()
            {
                buttonSprite = testSkillIcons[1],
                name = "3번 스킬",
                coolTime = 7f
            };

            SkillData skill3 = new SkillData()
            {
                buttonSprite = testSkillIcons[2],
                name = "2번 스킬",
                coolTime = 10f
            };

            skillDatas.Add(skill1);
            skillDatas.Add(skill2);
            skillDatas.Add(skill3);

            //inputUIController.ChangeSkill(skillDatas);
        }


        private void Update()
        {
            if (!isCombating)
                return;
        }


        public void StartCombat()
        {
            isCombating = true;

            // 상대방을 찾기
        }


        public void StopCombat()
        {
            isCombating = false;
            onStoppedCombat?.Invoke();
        }
    }
}
