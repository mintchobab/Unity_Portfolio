using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class CheckEnemyInFOVRange : Node
    {
        private AttackMonsterBT monster;

        public CheckEnemyInFOVRange(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            object o = GetData("target");

            if (o == null)
            {
                Collider[] colliders = Physics.OverlapSphere(monster.transform.position, monster.FovRange, monster.TargetableLayerMask);

                if (colliders.Length > 0)
                {
                    Transform target = null;

                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].gameObject.layer == monster.PlayerLayer)
                        {
                            parent.parent.SetData("target", colliders[i].transform);
                            target = colliders[i].transform;
                            break;
                        }
                    }

                    if (target == null)
                        parent.parent.SetData("target", colliders[0].transform);

                    monster.Anim.SetFloat(monster.HashMoveSpeed, 1f);

                    state = NodeState.Success;
                    return state;
                }

                state = NodeState.Failure;
                return state;
            }

            state = NodeState.Success;
            return state;
        }
    }
}
