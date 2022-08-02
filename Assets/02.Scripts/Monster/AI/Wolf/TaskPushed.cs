using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskPushed : Node
    {
        private AttackMonsterBT monster;

        public TaskPushed(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(monster.transform.position, monster.pushedPosition);

            if (distance > 0.01f)
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.pushedPosition, monster.MoveSpeed * Time.deltaTime);

                state = NodeState.Success;
                return state;
            }

            // 밀려나가기 끝
            monster.Anim.SetBool(monster.HashIsPushed, false);
            monster.DisableIsPushed();

            state = NodeState.Failure;
            return state;
        }
    }
}
