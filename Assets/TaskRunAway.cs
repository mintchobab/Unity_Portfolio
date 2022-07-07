using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class TaskRunAway : Node
    {
        private BirdBT monster;
        private IDamageable damageable;

        private Vector3 nextPos;

        private bool isFirst = true;


        public TaskRunAway(BirdBT monster)
        {
            this.monster = monster;
            damageable = monster.GetComponent<IDamageable>();
        }


        public override NodeState Evaluate()
        {
            bool isDead = damageable.CheckIsDead();

            if (!isDead)
            {
                // 한번 도망가면 끝까지 도망감
                if (isFirst)
                {
                    isFirst = false;
                    nextPos = GetRandomNextPos();
                    monster.Anim.SetTrigger(monster.HashRun);
                }

                if (Vector3.Distance(monster.transform.position, nextPos) > 0.01f)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(nextPos - monster.transform.position).normalized;

                    monster.transform.position = Vector3.MoveTowards(monster.transform.position, nextPos, monster.RunSpeed * Time.deltaTime);
                    monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, monster.RotSpeed * Time.deltaTime);
                }
                else
                {
                    nextPos = GetRandomNextPos();
                }

                state = NodeState.Running;
                return state;
            }
            else
            {
                state = NodeState.Failure;
                return state;
            }
        }


        private Vector3 GetRandomNextPos()
        {
            Vector3 nextPos = Vector3.zero;

            while (true)
            {
                Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                direction *= Random.Range(monster.MinMoveDistance, monster.MaxMoveDistance);
                nextPos = monster.transform.position + direction;

                bool isInner = monster.AreaCollider.bounds.Contains(nextPos);

                // 방향 뒤집기..???
                if (!isInner)
                    continue;
                else
                    break;
            }

            return nextPos;
        }
    }
}

