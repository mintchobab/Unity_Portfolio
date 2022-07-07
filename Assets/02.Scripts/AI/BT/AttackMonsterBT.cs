using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

namespace lsy
{
    public class AttackMonsterBT : MonsterBT
    {
        [field: SerializeField]
        public float FovRange { get; private set; } = 6f;

        [field: SerializeField]
        public float AttackRange { get; private set; } = 1f;


        public int PlayerLayer { get; private set; }
        public int PlayerLayerMask { get; private set; }
        public int TargetableLayerMask { get; private set; }

        public int HashIsRun { get; private set; } = Animator.StringToHash("isRun");
        public int HashIsAttack { get; private set; } = Animator.StringToHash("isAttack");

        protected override void Awake()
        {
            base.Awake();

            PlayerLayer = LayerMask.NameToLayer("Player");
            PlayerLayerMask = 1 << PlayerLayer;
            TargetableLayerMask = 1 << LayerMask.NameToLayer("NonAttackMonster") | PlayerLayerMask;
        }


        protected override Node SetUpTree()
        {
            return null;
        }
    }
}
