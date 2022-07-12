using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class CheckTakeDamage : Node
    {
        private MonsterBT monster;
        private MonsterController monsterController;

        public CheckTakeDamage(MonsterBT monster)
        {
            this.monster = monster;
            monsterController = monster.GetComponent<MonsterController>();
        }

        public override NodeState Evaluate()
        {
            bool isTakenDamage = monsterController.currentHp < monsterController.MaxHp ? true : false;

            // µµ¸Á°¡±â
            if (isTakenDamage)
            {
                state = NodeState.Success;
                return state;
            }

            state = NodeState.Failure;
            return state;
        }
    }
}
