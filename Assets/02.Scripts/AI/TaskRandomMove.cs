using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class TaskRandomMove : Node
    {
        private BirdBT monster;
        private Vector3 nextPos;

        float elapsedTime = 0f;
        float idleTime = 1f;

        private bool isFirst = true;


        public TaskRandomMove(BirdBT birdBT)
        {
            monster = birdBT;
        }


        public override NodeState Evaluate()
        {
            // 테스트용
            if (isFirst)
            {
                isFirst = false;
                nextPos = GetRandomNextPos();
            }

            // 1.다음 포지션으로 이동
            if (Vector3.Distance(monster.transform.position, nextPos) > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(nextPos - monster.transform.position).normalized;

                monster.transform.position = Vector3.MoveTowards(monster.transform.position, nextPos, monster.MoveSpeed * Time.deltaTime);
                monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, monster.RotSpeed * Time.deltaTime);

                monster.Anim.SetBool(monster.HashWalk, true);
            }
            // 3.일정시간 대기
            else
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= idleTime)
                {
                    elapsedTime = 0f;
                    nextPos = GetRandomNextPos();
                }

                monster.Anim.SetBool(monster.HashWalk, false);
            }            

            state = NodeState.Running;
            return state;
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
