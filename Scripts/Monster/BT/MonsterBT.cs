using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class MonsterBT : BehaviorTree.Tree
    {
        public Transform[] waypoints;

        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 1f;

        [field: SerializeField]
        public float RotSpeed { get; private set; } = 5f;        


        public Animator Anim { get; private set; }
        public Vector3 pushedPosition { get; private set; }

        public bool IsPushed { get; private set; }
        public float pushedTime { get; private set; } = 1f;
        

        protected virtual void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
        }


        protected override Node SetUpTree()
        {
            return null;
        }


        public void Pushed(Vector3 direction, float pushPower)
        {
            IsPushed = true;

            Vector3 dir = direction.normalized * pushPower;
            dir.y = 0f;

            pushedPosition = transform.position + dir;
        }


        public void DisableIsPushed()
        {
            IsPushed = false;
        }
    }
}
