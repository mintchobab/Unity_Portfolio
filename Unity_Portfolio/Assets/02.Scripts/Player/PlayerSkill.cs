using System;
using UnityEngine;

namespace lsy
{
    public class PlayerSkill : MonoBehaviour
    {
        [SerializeField]
        protected int skillId;        

        [SerializeField]
        protected string animationParameterName;

        [field: SerializeField]
        public float[] damageApplyPercents { get; private set; }

        [field: SerializeField]
        public float DistanceToTarget { get; private set; }

        public Action<Transform> onExecuteSkill;

        protected Transform child;

        public Skill SkillData { get; private set; }
        public Sprite SkillIcon { get; private set; }
        public bool IsCoolDown { get; set; }
        public int HashSkill { get; private set; }


        protected virtual void Awake()
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
