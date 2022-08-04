using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class BossWolfBT : AttackMonsterBT
    {
        [field: SerializeField]
        public GameObject SkillIndicator { get; private set; }

        [field: SerializeField]
        public float CircularSkillRadius { get; private set; }

        [field: SerializeField]
        public float SectorAngle { get; private set; }

        public MeshRenderer SkillIndicatorMr { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            SkillIndicatorMr = SkillIndicator.GetComponent<MeshRenderer>();
        }


        protected override void OnDead()
        {
            base.OnDead();
            SkillIndicator.SetActive(false);
        }


        protected override Node SetUpTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckDead(this),
                    new TaskDead(this)
                }),
                //new Sequence(new List<Node>
                //{
                //    new CheckPushed(this),
                //    new TaskPushed(this)
                //}),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(this),
                    new TaskBossAttack(this)
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRange(this),
                    new TaskGoToTarget(this)
                }),
                new TaskIdle(this)
            });

            return root;
        }
    }
}
