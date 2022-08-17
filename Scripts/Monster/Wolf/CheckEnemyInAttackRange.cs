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

            if (monster.IsAttacking)
            {
                state = NodeState.Success;
                return state;
            }

            Transform target = (Transform)o;
            if (Vector3.Distance(monster.transform.position, target.position) <= monster.AttackRange)
            {
                monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);                

                state = NodeState.Success;
                return state;
            }

            monster.Anim.SetFloat(monster.HashMoveSpeed, 1f);

            state = NodeState.Failure;
            return state;

        }
    }
}
