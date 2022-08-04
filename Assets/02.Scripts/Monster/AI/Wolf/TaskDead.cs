using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskDead : Node
    {
        private AttackMonsterBT monster;

        private bool isDead;
        private float elapsedTime;


        public TaskDead(AttackMonsterBT monster)
        {
            this.monster = monster;
        }


        public override NodeState Evaluate()
        {
            // ó�� ���� �� �� �ؾ��Ұ͵�
            if (!isDead)
            {
                isDead = true;
                elapsedTime = 0f;
                monster.Anim.SetTrigger(monster.HashDead);
            }            

            elapsedTime += Time.deltaTime;

            //if (elapsedTime > ValueData.MonsterDestroyTime)
            //{
            //    Object.Destroy(monster.gameObject);
            //}

            state = NodeState.Success;
            return state;
        }
    }
}
