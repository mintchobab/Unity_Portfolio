using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class CheckEnemyInAttackRange : Node
    {
        private AttackMonsterBT monster;

        public CheckEnemyInAttackRange(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            object o = GetData("target");
            if (o == null)
            {
                state = NodeState.Failure;
                return state;
            }

            Transform target = (Transform)o;
            if (Vector3.Distance(monster.transform.position, target.position) <= monster.AttackRange)
            {
                monster.Anim.SetBool(monster.HashIsRun, false);
                monster.Anim.SetBool(monster.HashIsAttack, true);

                state = NodeState.Success;
                return state;
            }

            monster.Anim.SetBool(monster.HashIsAttack, false);
            monster.Anim.SetBool(monster.HashIsRun, true);

            state = NodeState.Failure;
            return state;

        }
    }
}
