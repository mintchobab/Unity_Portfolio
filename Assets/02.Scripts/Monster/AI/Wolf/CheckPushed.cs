using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class CheckPushed : Node
    {
        private AttackMonsterBT monster;

        public CheckPushed(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            if (monster.IsPushed)
            {
                monster.Anim.SetBool(monster.HashIsPushed, true);

                state = NodeState.Success;
                return state;
            }

            state = NodeState.Failure;
            return state;
        }
    }
}
