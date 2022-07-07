using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskGoToTarget : Node
    {
        private AttackMonsterBT monster;
        private Transform lastTarget;

        private Vector3 lastPosition;

        private float elapsedTime = 0f;
        private float moveTime = 1f;

        public TaskGoToTarget(AttackMonsterBT monster)
        {
            this.monster = monster;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target != lastTarget)
            {
                lastTarget = target;
                lastPosition = target.position;
            }

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveTime)
            {
                elapsedTime = 0f;
                lastPosition = target.position;
            }

            if (Vector3.Distance(monster.transform.position, lastPosition) > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lastPosition - monster.transform.position).normalized;

                monster.transform.position = Vector3.MoveTowards(monster.transform.position, lastPosition, monster.MoveSpeed * Time.deltaTime);
                monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, monster.RotSpeed * Time.deltaTime);
            }
            else
            {
                monster.transform.position = lastPosition;

                elapsedTime = 0f;
                lastPosition = target.position;
            }

            //if (Vector3.Distance(monster.transform.position, target.position) > 0.01f)
            //{
            //    monster.transform.position = Vector3.MoveTowards(monster.transform.position, lastPosition, monster.Speed * Time.deltaTime);
            //    //monster.transform.LookAt(target.position);

            //    Quaternion lookRotation = Quaternion.LookRotation(lastPosition - monster.transform.position).normalized;
            //    monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, monster.Speed * Time.deltaTime);
            //}

            state = NodeState.Running;
            return state;
        }
    }
}
