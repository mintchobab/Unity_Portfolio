using BehaviorTree;
using System.Collections.Generic;

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
                    new CheckDead(this),
                    new TaskDead(this)
                }),
                new Sequence(new List<Node>
                {
                    new CheckPushed(this),
                    new TaskPushed(this)
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(this),
                    new TaskAttack(this)
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRange(this),
                    new TaskGoToTarget(this)
                }),
                new TaskIdle(this)
            });

            return root;
        }
    }
}
