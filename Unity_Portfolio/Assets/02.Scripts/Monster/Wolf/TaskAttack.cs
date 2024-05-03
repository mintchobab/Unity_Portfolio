using BehaviorTree;
using UnityEngine;

namespace lsy
{
    public class TaskAttack : Node
    {
        private AttackMonsterBT monster;
        private HpController hpController;
        private Transform lastTarget;

        private bool isWating = false;
        private bool canAttack = true;

        private float attackCounter = 0f;
        private float waitingCounter = 0f;


        public TaskAttack(AttackMonsterBT monster)
        {
            this.monster = monster;
        }


        private void Initialize()
        {
            canAttack = true;
            isWating = false;

            attackCounter = 0f;
            waitingCounter = 0f;
        }



        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != lastTarget)
            {
                hpController = target.GetComponent<HpController>();
                lastTarget = target;
            }

            // 공격 시작 시 처음 한번만 (애니메이션 때문에 프레임 넘김)
            if (!monster.IsAttacking)
            {
                monster.IsAttacking = true;
                monster.Anim.SetTrigger(monster.HashAtack);

                Vector3 lookDir = (target.position - monster.transform.position).normalized;
                lookDir.y = 0f;
                monster.transform.rotation = Quaternion.LookRotation(lookDir);

                state = NodeState.Success;
                return state;
            }


            float attackTime = monster.Anim.GetCurrentAnimatorStateInfo(0).length;
            attackCounter += Time.deltaTime;

            if (isWating)
            {
                waitingCounter += Time.deltaTime;

                if (waitingCounter >= monster.AttackDelay)
                {
                    Initialize();

                    monster.IsAttacking = false;
                    monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);

                    state = NodeState.Failure;
                    return state;
                }
            }

            if (canAttack)
            {
                if (attackCounter >= monster.AttackPercent * attackTime)
                {
                    canAttack = false;
                    isWating = true;

                    hpController.TakeDamage(monster.MonsterStat.offensivePower);
                    monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);

                    Managers.Instance.SoundManager.Play("Bark", SoundType.SFX);

                    if (hpController.IsDead)
                    {
                        ClearData("target");
                        state = NodeState.Success;
                        return state;
                    }
                }
            }

            state = NodeState.Success;
            return state;
        }
    }
}
