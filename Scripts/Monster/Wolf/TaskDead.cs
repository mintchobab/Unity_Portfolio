using BehaviorTree;
using UnityEngine;

namespace lsy
{
    public class TaskDead : Node
    {
        private AttackMonsterBT monster;

        private bool isDead;

        private readonly float destroyTime = 3f;
        private float elapsedTime;


        public TaskDead(AttackMonsterBT monster)
        {
            this.monster = monster;
        }


        public override NodeState Evaluate()
        {
            if (!isDead)
            {
                isDead = true;
                elapsedTime = 0f;
                monster.Anim.SetTrigger(monster.HashDead);

                monster.GetComponent<Collider>().enabled = false;

                Rigidbody rigid = monster.GetComponent<Rigidbody>();
                if (rigid)
                    rigid.isKinematic = true;

                GameManager.Instance.DieWolf();
            }            

            elapsedTime += Time.deltaTime;
            if (elapsedTime > destroyTime)
            {
                Object.Destroy(monster.gameObject);
            }

            state = NodeState.Success;
            return state;
        }
    }
}
