using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class CheckAttacking : Node
    {
        private AttackMonsterBT monster;

        public CheckAttacking(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            return NodeState.Failure;
        }
    }
}
