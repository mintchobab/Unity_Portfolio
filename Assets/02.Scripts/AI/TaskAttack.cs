using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskAttack : Node
    {
        private Transform lastTarget;
        private IDamageable damageable;
        private AttackMonsterBT monster;

        private float attackTime = 5f;
        private float attackCounter = 0f;

        public TaskAttack(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != lastTarget)
            {
                damageable = target.GetComponent<IDamageable>();
                lastTarget = target;
            }

            attackCounter += Time.deltaTime;
            if (attackCounter >= attackTime)
            {
                damageable.TakeDamage(10);

                bool isDead = damageable.CheckIsDead();
                if (isDead)
                {
                    ClearData("target");
                    monster.Anim.SetBool(monster.HashIsAttack, false);
                    monster.Anim.SetBool(monster.HashIsRun, true);
                }
                else
                {
                    attackCounter = 0f;

                    // 공격 종료 후 플레이어가 범위안에 있으면 타겟 변경
                    if (target.gameObject.layer != monster.PlayerLayer)
                    {
                        Collider[] colliders = Physics.OverlapSphere(monster.transform.position, monster.FovRange, monster.PlayerLayerMask);

                        if (colliders.Length > 0)
                        {
                            parent.parent.SetData("target", colliders[0].transform);
                            state = NodeState.Failure;
                            return state;
                        }
                    }
                }
            }

            state = NodeState.Running;
            return state;
        }
    }
}
