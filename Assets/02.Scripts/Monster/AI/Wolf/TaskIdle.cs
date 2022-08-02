using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class TaskIdle : Node
    {
        private AttackMonsterBT monster;

        public TaskIdle(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);

            state = NodeState.Success;
            return state;
        }
    }
}
