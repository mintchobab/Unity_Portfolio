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
        // �ڡڡ� ��ų�� ���� ������ ����� �ڡڡ�


        [SerializeField]
        private Sprite[] testSkillIcons;


        // ���۵ǰ� �Ÿ��� �ʹ� �־����� ����
        // ���Ͱ� ������ �ٽ� ��� ã��
        // ��� ������ ����
        public Action onStoppedCombat;

        private bool isCombating;

        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;






        private void Start()
        {
            List<SkillData> skillDatas = new List<SkillData>();

            SkillData skill1 = new SkillData()
            {
                buttonSprite = testSkillIcons[0],
                name = "1�� ��ų",
                coolTime = 5f
            };

            SkillData skill2 = new SkillData()
            {
                buttonSprite = testSkillIcons[1],
                name = "3�� ��ų",
                coolTime = 7f
            };

            SkillData skill3 = new SkillData()
            {
                buttonSprite = testSkillIcons[2],
                name = "2�� ��ų",
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

            // ������ ã��
        }


        public void StopCombat()
        {
            isCombating = false;
            onStoppedCombat?.Invoke();
        }
    }
}
