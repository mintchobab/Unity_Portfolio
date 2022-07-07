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
        

        protected virtual void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
        }


        // 하위에서 
        protected override Node SetUpTree()
        {
            return null;
        }
    }
}
