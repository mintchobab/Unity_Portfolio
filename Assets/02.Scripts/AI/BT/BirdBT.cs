using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    // 1.·£´ýÀÌµ¿
    // 2.µµ¸Á°¡±â??

    public class BirdBT : MonsterBT
    {
        [field: SerializeField]
        public BoxCollider AreaCollider { get; private set; }



        [field: SerializeField]
        public float MinMoveDistance { get; private set; } = 1f;

        [field: SerializeField]
        public float MaxMoveDistance { get; private set; } = 3f;



        public int HashWalk { get; private set; } = Animator.StringToHash("isWalk");
        public int HashRun { get; private set; } = Animator.StringToHash("run");
        public float RunSpeed { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            RunSpeed = MoveSpeed * 2f;
        }


        protected override Node SetUpTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckTakeDamage(this),
                    new TaskRunAway(this)
                }),
                new TaskRandomMove(this)
            });

            return root;
        }
    }
}
