using System;
using UnityEngine;

namespace lsy
{
    [Serializable]
    public class SkillData
    {
        public Sprite skillIcon;

        public string skillName;
        public float coolTime;

        public int damage;
    }


    public class PlayerSkill : MonoBehaviour
    {
        public float[] damageApplyTimes;

        public float distanceToTarget;


        private int hashAttack  = Animator.StringToHash("attack");
        private int hashSkill01 = Animator.StringToHash("skill01");
        private int hashSkill02 = Animator.StringToHash("skill02");
        private int hashSkill03 = Animator.StringToHash("skill03");

        public SkillData SkillData { get; private set; }

        public void StartSkill()
        {
            //animator.Play()

            // 1. 애니메이션 변경
            // 2. 데미지 적용??
        }
    }

}
