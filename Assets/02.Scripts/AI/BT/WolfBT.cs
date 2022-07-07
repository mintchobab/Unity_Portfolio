using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class WolfBT : AttackMonsterBT
    {
        protected override Node SetUpTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(this),
                    new TaskAttack(this),
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRange(this),
                    new TaskGoToTarget(this),
                }),
                //new TaskPatrol(transform, waypoints)
            });

            return root;
        }
    }
}
