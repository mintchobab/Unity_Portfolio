using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskDead : Node
    {
        private MonsterController monsterController;
        private AttackMonsterBT monster;

        private bool isDead;
        private float elapsedTime;


        public TaskDead(MonsterController monsterController, AttackMonsterBT monster)
        {
            this.monsterController = monsterController;
            this.monster = monster;
        }


        public override NodeState Evaluate()
        {
            // 처음 실행 될 때 해야할것들
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
