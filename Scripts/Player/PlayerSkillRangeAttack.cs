using UnityEngine;

namespace lsy
{
    public class PlayerSkillRangeAttack : PlayerSkill
    {
        [SerializeField]
        private GameObject skillEffect;

        protected override void Awake()
        {
            base.Awake();
            onExecuteSkill += OnExecuteSkill;
        }


        private void OnDisable()
        {
            skillEffect.gameObject.SetActive(false);
        }


        private void OnExecuteSkill(Transform target)
        {
            Vector3 center = target.GetComponent<Collider>().bounds.center;
            skillEffect.transform.position = target.transform.position + (center - target.position);
            skillEffect.gameObject.SetActive(true);
        }
    }
}
