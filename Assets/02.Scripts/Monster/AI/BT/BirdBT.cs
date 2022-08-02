using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class BirdBT : MonsterBT, IInteractable, ITargetable
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
                    new CheckInteract(this)
                }),
                //new TaskRandomMove(this)
            });

            return root;
        }


        public float GetInteractDistance()
        {
            throw new System.NotImplementedException();
        }

        public Transform GetTransform()
        {
            throw new System.NotImplementedException();
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }

        public Sprite LoadButtonImage()
        {
            throw new System.NotImplementedException();
        }
    }
}
