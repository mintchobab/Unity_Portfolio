using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskAttackCoolTime : Node
    {
        private AttackMonsterBT monster;

        private float delayCount;

        public TaskAttackCoolTime(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            delayCount += Time.deltaTime;

            if (delayCount >= monster.AttackDelay)
            {
                delayCount = 0f;

                state = NodeState.Success;
                return state;
            }            

            state = NodeState.Running;
            return state;
        }

    }
}


