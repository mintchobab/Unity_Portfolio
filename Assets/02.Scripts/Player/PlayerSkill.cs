using System;
using UnityEngine;

namespace lsy
{
    public class PlayerSkill : MonoBehaviour
    {
        [SerializeField]
        private int skillId;

        

        [SerializeField]
        private string animationParameterName;

        [field: SerializeField]
        public float[] damageApplyPercents { get; private set; }

        private Transform child;

        public float distanceToTarget;

        public int HashSkill { get; private set; }

        public bool IsCoolDown { get; set; }


        public Skill SkillData { get; private set; }
        public Sprite SkillIcon { get; private set; }


        private void Awake()
        {
            if (!animationParameterName.Equals(string.Empty))
                HashSkill = Animator.StringToHash(animationParameterName);

            child = transform.GetChild(0);

            SkillData = Managers.Instance.JsonManager.jsonSkill.skills.Find(x => x.id == skillId);
            SkillIcon = Managers.Instance.ResourceManager.Load<Sprite>($"{ResourcePath.Icon}/Icon_{SkillData._resourceName}");

            Deactivate();
        }


        public string GetSkillName()
        {
            return StringManager.GetLocalizedSkillName(SkillData.skillName);
        }


        public void Activate()
        {
            child.gameObject.SetActive(true);
        }


        public void Deactivate()
        {
            child.gameObject.SetActive(false);
        }
    }
}
