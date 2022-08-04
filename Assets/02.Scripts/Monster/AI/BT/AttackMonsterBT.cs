using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

namespace lsy
{
    public class AttackMonsterBT : MonsterBT, IHpHasable
    {
        [field: SerializeField]
        public float FovRange { get; private set; } = 6f;

        [field: SerializeField]
        public float AttackRange { get; private set; } = 1f;

        [field: SerializeField]
        public float AttackPercent { get; private set; }

        [field: SerializeField]
        public float AttackDelay { get; private set; }

        public HpController HpController { get; private set; }

        public bool IsAttacking { get; set; }

        public int PlayerLayer { get; private set; }
        public int PlayerLayerMask { get; private set; }
        public int TargetableLayerMask { get; private set; }


        public int HashMoveSpeed { get; private set; } = Animator.StringToHash("moveSpeed");
        public int HashAtack { get; private set; } = Animator.StringToHash("attack");
        public int HashTakeDamage { get; private set; } = Animator.StringToHash("takeDamage");
        public int HashDead { get; private set; } = Animator.StringToHash("dead");
        public int HashIsPushed { get; private set; } = Animator.StringToHash("isPushed");


        [SerializeField]
        private int monsterId;

        // json 데이터 에서 스텟을 가져와야함
        public Stat MonsterStat { get; private set; }


        protected override Node SetUpTree()
        {
            return null;
        }


        protected override void Awake()
        {
            base.Awake();

            HpController = GetComponent<HpController>();

            PlayerLayer = LayerMask.NameToLayer("Player");
            PlayerLayerMask = 1 << PlayerLayer;
            TargetableLayerMask = 1 << LayerMask.NameToLayer("NonAttackMonster") | PlayerLayerMask;

            HpController.onDead += OnDead;

            MonsterStat = Managers.Instance.JsonManager.jsonMonster.monsters.Find(x => x.id == monsterId).stat;
        }


        protected virtual void OnEnable()
        {
            HpController.onTakeDamage += OnTakeDamage;
        }


        protected virtual void OnDisable()
        {
            HpController.onTakeDamage -= OnTakeDamage;
        }


        private void OnTakeDamage()
        {
            if (!IsPushed)
                Anim.SetTrigger(HashTakeDamage);
        }


        public int GetMaxHp()
        {
            return MonsterStat.hp;
        }

        public int GetDefensivePower()
        {
            return MonsterStat.defensivePower;
        }


        protected virtual void OnDead()
        {
            Managers.Instance.QuestManager.KilledMonster(monsterId);
            GetComponent<Collider>().enabled = false;

            Invoke("DestroySelf", 3f);
        }

        protected void DestroySelf()
        {
            gameObject.SetActive(false);
        }
    }
}
